using Godot;

namespace CrystalsOfLiora.Maps;

public partial class SpawnPoint : Node3D
{
    [Export] public string SpawnId { get; set; } = string.Empty;
    [Export] public bool FacePlayerOnSpawn { get; set; } = true;
    [Export] public float FacingYawDeg { get; set; } = 0f;
}
