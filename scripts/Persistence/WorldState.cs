using Godot;

namespace CrystalsOfLiora.Persistence;

[GlobalClass]
public partial class ActiveEffectState : Resource
{
    [Export] public string EffectId { get; set; } = string.Empty;
    [Export] public float RemainingS { get; set; }
    [Export] public float NextTickS { get; set; }
}

[GlobalClass]
public partial class AbilityState : Resource
{
    [Export] public Godot.Collections.Array<string> Unlocked { get; set; } = [];
    [Export] public Godot.Collections.Array<string> Equipped { get; set; } = [];
    [Export] public Godot.Collections.Dictionary<string, float> Cooldowns { get; set; } = [];
}

[GlobalClass]
public partial class CharacterState : Resource
{
    [Export] public string EntityId { get; set; } = string.Empty;
    [Export] public string CharacterDefId { get; set; } = string.Empty;
    [Export] public Vector3 Position { get; set; } = Vector3.Zero;
    [Export] public float RotationY { get; set; }
    [Export] public int Level { get; set; } = 1;
    [Export] public float CurrentHp { get; set; } = 1f;
    [Export] public float CurrentMana { get; set; }
    [Export] public InventoryState Inventory { get; set; } = new();
    [Export] public Godot.Collections.Dictionary<int, string> Equipped { get; set; } = [];
    [Export] public string FactionId { get; set; } = "neutral";
    [Export] public AbilityState AbilityState { get; set; } = new();
    [Export] public Godot.Collections.Array<ActiveEffectState> ActiveEffects { get; set; } = [];
    [Export] public Godot.Collections.Dictionary AiState { get; set; } = [];
}

[GlobalClass]
public partial class ObjectStateEnvelope : Resource
{
    [Export] public string Type { get; set; } = string.Empty;
    [Export] public Godot.Collections.Dictionary Data { get; set; } = [];
}

[GlobalClass]
public partial class MapState : Resource
{
    [Export] public string MapId { get; set; } = string.Empty;
    [Export] public bool Visited { get; set; }
    [Export] public Godot.Collections.Dictionary<string, ObjectStateEnvelope> ObjectStates { get; set; } = [];
    [Export] public Godot.Collections.Dictionary<string, CharacterState> CharacterStates { get; set; } = [];
    [Export] public Godot.Collections.Dictionary<string, bool> OneTimeTriggers { get; set; } = [];
    [Export] public long LastVisitedTime { get; set; }
}

[GlobalClass]
public partial class WorldState : Resource
{
    [Export] public int SaveVersion { get; set; } = 1;
    [Export] public Godot.Collections.Dictionary GlobalFlags { get; set; } = [];
    [Export] public int CompletedParts { get; set; }
    [Export] public int ShardsCollected { get; set; }
    [Export] public Godot.Collections.Array<string> UnlockedAbilityIds { get; set; } = [];
    [Export] public CharacterState PlayerState { get; set; } = new();
    [Export] public Godot.Collections.Dictionary<string, MapState> MapStates { get; set; } = [];
}
