using Godot;
using CrystalsOfLiora.Persistence;
using CrystalsOfLiora.Maps;
using CrystalsOfLiora.Entities;

namespace CrystalsOfLiora.Core;

public partial class Game : Node
{
    [Export] public string StartingMapId { get; set; } = "part_01";
    [Export] public string StartingSpawnId { get; set; } = "start";
    [Export] public string StartingScenePath { get; set; } = "res://scenes/maps/Part01.tscn";
    [Export] public string PlayerDefId { get; set; } = "arin";

    public WorldState State { get; private set; } = new();

    private SceneFlow _sceneFlow;
    private SaveSystem _saveSystem;
    private EntityFactory _entityFactory;

    public override async void _Ready()
    {
        try
        {
            GD.Print("[Game] Ready start: wiring services.");
            _sceneFlow = GetNode<SceneFlow>("/root/SceneFlow");
            _saveSystem = GetNode<SaveSystem>("/root/SaveSystem");
            _entityFactory = GetNode<EntityFactory>("/root/EntityFactory");
            _sceneFlow.SceneLoaded += OnSceneLoaded;

            GD.Print("[Game] Loading world state from slot 0.");
            var savePath = "user://save_0.json";
            if (!FileAccess.FileExists(savePath))
            {
                GD.Print("[Game] No save file found for slot 0; using default WorldState.");
                State = new WorldState();
            }
            else
            {
                try
                {
                    State = _saveSystem.Load(0);
                    GD.Print("[Game] World state loaded successfully.");
                }
                catch (System.Exception ex)
                {
                    GD.PushError($"[Game] Load threw unexpectedly: {ex.Message}. Falling back to new WorldState.");
                    State = new WorldState();
                }
            }

            var scenePath = StartingScenePath;
            if (string.IsNullOrWhiteSpace(scenePath) || !ResourceLoader.Exists(scenePath))
            {
                GD.PushWarning($"[Game] StartingScenePath is invalid ('{scenePath}'). Falling back to default Part01.");
                scenePath = "res://scenes/maps/Part01.tscn";
            }

            GD.Print($"[Game] Changing map to {scenePath} (mapId={StartingMapId}, spawnId={StartingSpawnId}).");
            await _sceneFlow.ChangeMap(scenePath, StartingMapId, StartingSpawnId);
            GD.Print("[Game] ChangeMap awaited successfully.");
        }
        catch (System.Exception ex)
        {
            GD.PushError($"[Game] Unhandled exception during _Ready: {ex.Message}");
        }
    }

    private void OnSceneLoaded(string mapId, string spawnId)
    {
        GD.Print($"[Game] SceneLoaded received: mapId={mapId}, spawnId={spawnId}. Deferring player spawn.");
        CallDeferred(nameof(SpawnPlayerForLoadedScene), mapId, spawnId);
    }

    private void SpawnPlayerForLoadedScene(string mapId, string spawnId)
    {
        var map = GetTree().CurrentScene as MapRoot;
        if (map == null)
        {
            GD.PushWarning("[Game] Current scene is not MapRoot; player spawn skipped.");
            return;
        }

        var spawn = map.FindSpawn(spawnId) ?? map.FindSpawn(StartingSpawnId);
        var pos = spawn?.GlobalPosition ?? Vector3.Zero;
        var rot = new Vector3(0, spawn?.FacingYawDeg ?? 45f, 0);

        GD.Print($"[Game] Spawning player def '{PlayerDefId}' at {pos} yaw={rot.Y}.");
        var player = _entityFactory.SpawnEntity(PlayerDefId, pos, rot, map.GetNode("Entities"));
        if (player == null)
        {
            GD.PushWarning("[Game] Player spawn failed.");
            return;
        }

        GD.Print($"[Game] Player spawned with EntityId={player.EntityId}.");

        var cameraRig = map.GetNodeOrNull<Node3D>("CameraRig");
        if (cameraRig is CameraRig rig)
        {
            rig.TargetPath = rig.GetPathTo(player.GetNode<Node3D>("CameraTarget"));
            GD.Print($"[Game] CameraRig target path set to '{rig.TargetPath}'.");
            rig.ResolveTarget();
        }

        GD.Print($"[Game] Emitting PlayerSpawned for {player.EntityId} at {player.GlobalPosition} on map {map.MapId}.");
        _sceneFlow.EmitSignal(SceneFlow.SignalName.PlayerSpawned, player.EntityId, map.MapId, player.GlobalPosition);
    }

    public void CollectShard(string abilityId)
    {
        GD.Print($"[Game] CollectShard called: abilityId={abilityId}");
        State.ShardsCollected++;
        if (!State.UnlockedAbilityIds.Contains(abilityId))
            State.UnlockedAbilityIds.Add(abilityId);
    }
}
