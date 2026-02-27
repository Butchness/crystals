using Godot;

namespace CrystalsOfLiora.Entities;

public partial class PlayerBrain : Node
{
    [Export] public NodePath CharacterPath { get; set; } = "../../..";
    [Export] public NodePath CameraRigPath { get; set; } = "";

    private Character _character;

    public override void _Ready()
    {
        _character = GetNodeOrNull<Character>(CharacterPath);
        if (_character == null)
            _character = GetParent()?.GetParent()?.GetParent() as Character;
        if (_character == null)
        {
            GD.PushWarning($"PlayerBrain could not resolve Character at path '{CharacterPath}'");
            return;
        }

        _character.AddToGroup("player");
        GD.Print($"[PlayerBrain] Bound to Character EntityId={_character.EntityId} and added to player group.");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_character == null) return;

        var input2D = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        var move = new Vector3(input2D.X, 0, input2D.Y);
        _character.SetMoveIntent(move);

        if (Input.IsActionJustPressed("jump")) _character.RequestJump();
        if (Input.IsActionJustPressed("attack")) _character.RequestAttack();
    }
}
