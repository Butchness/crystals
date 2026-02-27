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
    }

    public Character SpawnEntity(string characterDefId, Vector3 pos, Vector3 rotationDeg, Node parent = null)
    {
        var def = _database.GetById<CharacterDef>(characterDefId);
        if (def == null || CharacterScene == null) return null;

        var character = CharacterScene.Instantiate<Character>();
        character.Def = def;

        var targetParent = parent ?? GetTree().CurrentScene;
        targetParent.CallDeferred(Node.MethodName.AddChild, character);
        character.CallDeferred(Character.MethodName.FinalizeSpawn, pos, rotationDeg);
        return character;
    }
}
