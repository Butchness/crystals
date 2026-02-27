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
        Input.MouseMode = Input.MouseModeEnum.Confined;
        GetTree().Paused = true;
        await ToSignal(GetTree().CreateTimer(0.15f), SceneTreeTimer.SignalName.Timeout);
        GetTree().ChangeSceneToFile(scenePath);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        EmitSignal(SignalName.SceneLoaded, mapId, spawnId);
        await ToSignal(GetTree().CreateTimer(SpawnInputLockS), SceneTreeTimer.SignalName.Timeout);
        GetTree().Paused = false;
    }
}
