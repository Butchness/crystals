using Godot;
using System.Threading.Tasks;

namespace CrystalsOfLiora.Core;

public partial class SceneFlow : Node
{
    [Signal] public delegate void SceneLoadedEventHandler(string mapId, string spawnId);
    [Signal] public delegate void PlayerSpawnedEventHandler(string entityId, string mapId, Vector3 position);

    [Export] public TransitionStyle DefaultTransitionStyle { get; set; } = TransitionStyle.Fade;
    [Export] public float SpawnInputLockS { get; set; } = 0.15f;

    public async Task ChangeMap(string scenePath, string mapId, string spawnId)
    {
        GD.Print($"[SceneFlow] ChangeMap start: scenePath={scenePath}, mapId={mapId}, spawnId={spawnId}");
        if (string.IsNullOrWhiteSpace(scenePath) || !ResourceLoader.Exists(scenePath))
        {
            GD.PushError($"[SceneFlow] ChangeMap aborted; scene path is invalid: '{scenePath}'");
            return;
        }

        Input.MouseMode = Input.MouseModeEnum.Confined;
        GetTree().Paused = true;
        GD.Print("[SceneFlow] Tree paused; waiting for transition delay.");
        await ToSignal(GetTree().CreateTimer(0.15f), SceneTreeTimer.SignalName.Timeout);
        GD.Print($"[SceneFlow] Changing scene to {scenePath}.");
        GetTree().ChangeSceneToFile(scenePath);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        GD.Print($"[SceneFlow] Scene frame ready; emitting SceneLoaded ({mapId}, {spawnId}).");
        EmitSignal(SignalName.SceneLoaded, mapId, spawnId);
        await ToSignal(GetTree().CreateTimer(SpawnInputLockS), SceneTreeTimer.SignalName.Timeout);
        GetTree().Paused = false;
        GD.Print("[SceneFlow] ChangeMap complete; tree unpaused.");
    }
}
