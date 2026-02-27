using Godot;
using CrystalsOfLiora.Components;
using CrystalsOfLiora.Defs;
using CrystalsOfLiora.Attacks;

namespace CrystalsOfLiora.Entities;

public partial class Character : CharacterBody3D, IDamageable
{
    [Signal] public delegate void CharacterDiedEventHandler(string entityId);
    [Signal] public delegate void CharacterDamagedEventHandler(string targetEntityId, string sourceEntityId, float amount);

    [Export] public CharacterDef Def { get; set; }
    [Export] public NodePath StatsPath { get; set; } = "Components/StatsComponent";
    [Export] public NodePath InventoryPath { get; set; } = "Components/InventoryComponent";
    [Export] public NodePath EquipmentPath { get; set; } = "Components/EquipmentComponent";
    [Export] public NodePath BrainPath { get; set; } = "Components/BrainComponent";
    [Export] public float Accel { get; set; } = 12f;
    [Export] public float Gravity { get; set; } = 20f;
    [Export] public PackedScene AttackScene { get; set; }

    public string EntityId { get; set; } = System.Guid.NewGuid().ToString("N");
    public string FactionId => Def?.FactionId ?? "neutral";

    private StatsComponent _stats;
    private InventoryComponent _inventory;
    private EquipmentComponent _equipment;
    private Vector3 _moveIntent;
    private bool _jumpRequested;
    private bool _attackRequested;

    public override void _Ready()
    {
        GD.Print($"[Character:{EntityId}] Ready. DefId={Def?.Id ?? "<null>"}");
        _stats = GetNode<StatsComponent>(StatsPath);
        _inventory = GetNode<InventoryComponent>(InventoryPath);
        _equipment = GetNode<EquipmentComponent>(EquipmentPath);
        if (Def != null) ApplyDefinition();
    }

    public void FinalizeSpawn(Vector3 pos, Vector3 rotationDeg)
    {
        GlobalPosition = pos;
        RotationDegrees = rotationDeg;
        SetupBrain();
    }

    public void SetupBrain()
    {
        if (Def?.BrainScene == null)
        {
            GD.Print($"[Character:{EntityId}] SetupBrain skipped: no BrainScene on def.");
            return;
        }

        var brainRoot = GetNode<Node>(BrainPath);
        foreach (var child in brainRoot.GetChildren()) child.QueueFree();
        var brain = Def.BrainScene.Instantiate();
        brainRoot.AddChild(brain);
        GD.Print($"[Character:{EntityId}] SetupBrain attached '{brain.Name}' from '{Def.BrainScene.ResourcePath}'.");
    }

    public void ApplyDefinition()
    {
        GD.Print($"[Character:{EntityId}] ApplyDefinition for DefId={Def?.Id ?? "<null>"}.");
        _stats.Configure(Def.Stats);
        _inventory.Configure(Def.Inventory);
        foreach (var item in Def.EquipmentSet) _equipment.Equip(item);
    }

    public void SetMoveIntent(Vector3 intent) => _moveIntent = intent;
    public void RequestJump() => _jumpRequested = true;
    public void RequestAttack() => _attackRequested = true;

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        if (!IsOnFloor()) Velocity += Vector3.Down * Gravity * dt;
        else if (_jumpRequested) Velocity = new Vector3(Velocity.X, Def?.Stats?.JumpSpeed ?? 8f, Velocity.Z);

        var target = _moveIntent.Normalized() * (Def?.Stats?.Speed ?? 6f);
        Velocity = new Vector3(
            Mathf.MoveToward(Velocity.X, target.X, Accel * dt),
            Velocity.Y,
            Mathf.MoveToward(Velocity.Z, target.Z, Accel * dt));

        MoveAndSlide();
        if (_attackRequested) SpawnAttack();

        _jumpRequested = false;
        _attackRequested = false;
    }

    private void SpawnAttack()
    {
        if (AttackScene == null || Def?.AbilitySet == null || Def.AbilitySet.Count == 0) return;
        if (AttackScene.Instantiate() is not Attack attack) return;
        GetTree().CurrentScene.AddChild(attack);
        attack.Configure(Def.AbilitySet[0], this, GlobalPosition, -GlobalBasis.Z);
    }

    public bool CanTakeDamage(string sourceFaction) => sourceFaction != FactionId;

    public void ReceiveDamage(float amount, Character source)
    {
        if (!CanTakeDamage(source?.FactionId ?? "neutral")) return;
        _stats.ApplyDamage(amount);
        EmitSignal(SignalName.CharacterDamaged, EntityId, source?.EntityId ?? string.Empty, amount);
        if (_stats.CurrentHp <= 0)
            EmitSignal(SignalName.CharacterDied, EntityId);
    }
}
