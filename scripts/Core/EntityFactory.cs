using Godot;
using CrystalsOfLiora.Entities;
using CrystalsOfLiora.Defs;

namespace CrystalsOfLiora.Core;

public partial class EntityFactory : Node
{
    [Export] public PackedScene CharacterScene { get; set; }

    private Database _database;

    public override void _Ready()
    {
        _database = GetNode<Database>("/root/Database");
        GD.Print("[EntityFactory] Ready and linked to /root/Database.");
    }

    public Character SpawnEntity(string characterDefId, Vector3 pos, Vector3 rotationDeg, Node parent = null)
    {
        GD.Print($"[EntityFactory] Spawn requested: def='{characterDefId}', pos={pos}, rot={rotationDeg}");
        var def = _database.GetById<CharacterDef>(characterDefId);
        if (def == null)
        {
            GD.PushWarning($"[EntityFactory] Character definition not found: '{characterDefId}'");
            return null;
        }

        if (CharacterScene == null)
        {
            GD.PushWarning("[EntityFactory] CharacterScene is null; spawn aborted.");
            return null;
        }

        var character = CharacterScene.Instantiate<Character>();
        character.Def = def;

        var targetParent = parent ?? GetTree().CurrentScene;
        GD.Print($"[EntityFactory] Adding entity '{character.EntityId}' to parent '{targetParent?.Name ?? "<null>"}'.");
        targetParent.AddChild(character);
        character.GlobalPosition = pos;
        character.RotationDegrees = rotationDeg;
        GD.Print($"[EntityFactory] Spawned entity '{character.EntityId}' at {character.GlobalPosition}. Deferring SetupBrain.");
        character.CallDeferred(Character.MethodName.SetupBrain);
        return character;
    }
}
