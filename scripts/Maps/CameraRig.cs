using Godot;

namespace CrystalsOfLiora.Maps;

public partial class CameraRig : Node3D
{
    [Export] public NodePath TargetPath { get; set; }
    [Export] public Vector3 Offset { get; set; } = new(8, 10, 8);
    [Export] public float FollowLerp { get; set; } = 8f;

    private Node3D _target;

    public override void _Ready()
    {
        GD.Print($"[CameraRig] Ready. TargetPath='{TargetPath}', Offset={Offset}, FollowLerp={FollowLerp}");
        ResolveTarget();
    }

    public void ResolveTarget()
    {
        if (TargetPath == null || TargetPath.IsEmpty)
        {
            GD.Print("[CameraRig] ResolveTarget: empty target path; camera has no target.");
            _target = null;
            return;
        }

        _target = GetNodeOrNull<Node3D>(TargetPath);
        GD.Print($"[CameraRig] ResolveTarget: path='{TargetPath}', found={_target != null}");
    }

    public override void _Process(double delta)
    {
        if (_target == null) return;
        var desired = _target.GlobalPosition + Offset;
        GlobalPosition = GlobalPosition.Lerp(desired, (float)delta * FollowLerp);
        LookAt(_target.GlobalPosition, Vector3.Up);
    }
}
