using Godot;
using CrystalsOfLiora.Core;

namespace CrystalsOfLiora.Spawners;

public partial class EntitySpawner : Node3D
{
    [Export] public string CharacterDefId { get; set; } = string.Empty;
    [Export] public SpawnMode Mode { get; set; } = SpawnMode.FixedCount;
    [Export] public SpawnBucket Bucket { get; set; } = SpawnBucket.Entities;
    [Export] public bool Enabled { get; set; } = true;
    [Export] public bool SpawnOnReady { get; set; } = true;
    [Export] public float PlayerRange { get; set; } = -1;
    [Export] public int TotalToSpawn { get; set; } = 1;
    [Export] public int MaxAlive { get; set; } = 1;
    [Export] public int Stock { get; set; } = 0;
    [Export] public float RespawnDelayS { get; set; } = 5f;
    [Export] public bool RequireClearSpawnArea { get; set; } = false;
    [Export] public NodePath SpawnAreaPath { get; set; }

    private EntityFactory _factory;
    private int _spawnedTotal;
    private int _alive;
    private float _respawnTimer;

    public override void _Ready()
    {
        _factory = GetNode<EntityFactory>("/root/EntityFactory");
        if (SpawnOnReady && Enabled) TrySpawn();
    }

    public override void _Process(double delta)
    {
        if (!Enabled) return;
        if (PlayerRange >= 0)
        {
            var player = GetTree().GetFirstNodeInGroup("player") as Node3D;
            if (player == null || player.GlobalPosition.DistanceTo(GlobalPosition) > PlayerRange) return;
        }

        _respawnTimer -= (float)delta;
        if (_respawnTimer <= 0) TrySpawn();
    }

    private void TrySpawn()
    {
        if (Mode == SpawnMode.FixedCount && _spawnedTotal >= TotalToSpawn) return;
        if (_alive >= MaxAlive) return;
        if (Mode == SpawnMode.Stocked && Stock <= 0) return;

        var character = _factory.SpawnEntity(CharacterDefId, GlobalPosition, RotationDegrees, GetParent());
        if (character == null) return;

        _alive++;
        _spawnedTotal++;
        if (Mode == SpawnMode.Stocked) Stock--;
        character.CharacterDied += _ => { _alive = Mathf.Max(0, _alive - 1); _respawnTimer = RespawnDelayS; };
    }
}
