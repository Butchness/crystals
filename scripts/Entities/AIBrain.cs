using Godot;

namespace CrystalsOfLiora.Entities;

public partial class AIBrain : Node
{
    [Export] public NodePath CharacterPath { get; set; } = "../../..";
    [Export] public float WanderIntervalS { get; set; } = 2f;

    private Character _character;
    private Vector3 _intent = Vector3.Zero;
    private float _timer;
    private readonly RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        _character = GetNodeOrNull<Character>(CharacterPath);
        if (_character == null)
            _character = GetParent()?.GetParent()?.GetParent() as Character;

        _timer = WanderIntervalS;
        if (_character == null)
            GD.PushWarning($"AIBrain could not resolve Character at path '{CharacterPath}'");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_character == null) return;

        _timer -= (float)delta;
        if (_timer <= 0)
        {
            _timer = WanderIntervalS;
            _intent = new Vector3(_rng.RandfRange(-1, 1), 0, _rng.RandfRange(-1, 1));
        }

        _character.SetMoveIntent(_intent);
    }
}
