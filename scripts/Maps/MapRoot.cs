using Godot;
using CrystalsOfLiora.Entities;

namespace CrystalsOfLiora.Maps;

public partial class MapRoot : Node3D
{
    [Export] public string MapId { get; set; } = string.Empty;
    [Export] public bool PersistEnabled { get; set; } = true;

    public SpawnPoint FindSpawn(string spawnId)
    {
        foreach (var child in GetNode("SpawnPoints").GetChildren())
        {
            if (child is SpawnPoint sp && sp.SpawnId == spawnId) return sp;
        }
        return null;
    }

    public Character GetPlayerCharacter() => GetTree().GetFirstNodeInGroup("player") as Character;
}
