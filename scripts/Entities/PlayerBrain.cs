using Godot;

namespace CrystalsOfLiora.Entities;

public partial class PlayerBrain : Node
{
    [Export] public NodePath CharacterPath { get; set; } = "../..";
    [Export] public NodePath CameraRigPath { get; set; } = "";

    private Character _character;

    public override void _Ready()
    {
        _character = GetNode<Character>(CharacterPath);
        _character.AddToGroup("player");
    }

    public override void _PhysicsProcess(double delta)
    {
        var input2D = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        var move = new Vector3(input2D.X, 0, input2D.Y);
        _character.SetMoveIntent(move);

        if (Input.IsActionJustPressed("jump")) _character.RequestJump();
        if (Input.IsActionJustPressed("attack")) _character.RequestAttack();
    }
}
