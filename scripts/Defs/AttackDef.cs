using Godot;
using CrystalsOfLiora.Core;

namespace CrystalsOfLiora.Defs;

[GlobalClass]
public partial class AttackDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public AttackKind AttackKind { get; set; } = AttackKind.MeleeArc;
    [Export] public int ElementKind { get; set; } = 0;
    [Export] public float BaseDamage { get; set; } = 10f;
    [Export] public StatsDef StatScaling { get; set; }
    [Export] public float CritChance { get; set; } = 0.05f;
    [Export] public float CritMultiplier { get; set; } = 1.5f;
    [Export] public float WindupS { get; set; } = 0.1f;
    [Export] public float ActiveS { get; set; } = 0.15f;
    [Export] public float CooldownS { get; set; } = 0.5f;
    [Export] public float LifetimeS { get; set; } = 0f;
    [Export] public TeamType TeamFilter { get; set; } = TeamType.Enemies;
    [Export] public HitShapeType HitShape { get; set; } = HitShapeType.Circle;
    [Export] public float Radius { get; set; } = 1.2f;
    [Export] public float Width { get; set; } = 1f;
    [Export] public float Length { get; set; } = 1f;
    [Export] public float AngleDeg { get; set; } = 90f;
    [Export] public float Range { get; set; } = 1f;
    [Export] public Vector3 HitOriginOffset { get; set; } = Vector3.Zero;
    [Export] public AttackMovementType MoveType { get; set; } = AttackMovementType.FollowCaster;
    [Export] public float Speed { get; set; } = 0f;
    [Export] public string VfxId { get; set; } = string.Empty;
    [Export] public string SfxId { get; set; } = string.Empty;
    [Export] public string AnimationId { get; set; } = string.Empty;
}
