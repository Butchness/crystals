using Godot;
using CrystalsOfLiora.Entities;

namespace CrystalsOfLiora.Maps;

public partial class MapRoot : Node3D
{
    [Export] public string MapId { get; set; } = string.Empty;
    [Export] public bool PersistEnabled { get; set; } = true;

    public override void _Ready()
    {
        GD.Print($"[MapRoot] Ready. MapId='{MapId}', PersistEnabled={PersistEnabled}");
    }

    public SpawnPoint FindSpawn(string spawnId)
    {
        GD.Print($"[MapRoot] FindSpawn('{spawnId}') called.");
        foreach (var child in GetNode("SpawnPoints").GetChildren())
        {
            if (child is SpawnPoint sp && sp.SpawnId == spawnId)
            {
                GD.Print($"[MapRoot] FindSpawn matched node '{sp.Name}' at {sp.GlobalPosition}");
                return sp;
            }
        }

        GD.PushWarning($"[MapRoot] FindSpawn could not find spawnId '{spawnId}'.");
        return null;
    }

    public Character GetPlayerCharacter() => GetTree().GetFirstNodeInGroup("player") as Character;
}
