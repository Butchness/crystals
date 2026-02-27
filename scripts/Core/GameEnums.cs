using Godot;

namespace CrystalsOfLiora.Core;

public enum ControlType { None, Player, AI }
public enum ItemType { None, Food, Wearable, Tool, Misc }
public enum EquipSlotType { None, Head, Chest, Legs, Feet, Weapon, Aux }
public enum AttackKind { None, MeleeArc, Projectile, AoE, Beam, Summon }
public enum AttackMovementType { None, Linear, Homing, FollowCaster }
public enum HitShapeType { None, Circle, Square, Cone }
public enum TeamType { None, Any, Allies, Enemies, Self }
public enum SpecialType { None, Stun, Silence, Invulnerable, Knockdown }
public enum EffectMode { Additive, Multiplicative, Override }
public enum SpawnMode { FixedCount, Persistent, Stocked }
public enum SpawnBucket { Entities, Vfx, Interactables }
public enum TransitionStyle { None, Fade }
