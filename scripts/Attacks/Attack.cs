using Godot;
using System.Collections.Generic;
using CrystalsOfLiora.Core;
using CrystalsOfLiora.Defs;
using CrystalsOfLiora.Entities;

namespace CrystalsOfLiora.Attacks;

public partial class Attack : Node3D
{
    private AttackDef _def;
    private Character _caster;
    private Area3D _hitbox;
    private CollisionShape3D _shape;
    private Vector3 _forward;
    private float _elapsed;
    private readonly HashSet<ulong> _hitSet = [];

    public override void _Ready()
    {
        _hitbox = GetNode<Area3D>("Hitbox");
        _shape = GetNode<CollisionShape3D>("Hitbox/CollisionShape3D");
        _hitbox.BodyEntered += OnBodyEntered;
    }

    public void Configure(AttackDef def, Character caster, Vector3 origin, Vector3 forward)
    {
        _def = def;
        _caster = caster;
        _forward = forward.Normalized();
        GlobalPosition = origin + (_forward * def.Range) + def.HitOriginOffset;
        BuildShape();
        _hitbox.Monitoring = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_def == null) return;
        _elapsed += (float)delta;
        _hitbox.Monitoring = _elapsed >= _def.WindupS && _elapsed <= _def.WindupS + _def.ActiveS;

        if (_def.MoveType == AttackMovementType.Linear)
            GlobalPosition += _forward * _def.Speed * (float)delta;
        else if (_def.MoveType == AttackMovementType.FollowCaster && IsInstanceValid(_caster))
            GlobalPosition = _caster.GlobalPosition + (_forward * _def.Range) + _def.HitOriginOffset;

        var ttl = _def.LifetimeS > 0 ? _def.LifetimeS : _def.WindupS + _def.ActiveS;
        if (_elapsed > ttl) QueueFree();
    }

    private void BuildShape()
    {
        Shape3D s = _def.HitShape switch
        {
            HitShapeType.Square => new BoxShape3D { Size = new Vector3(_def.Width, 2f, _def.Length) },
            _ => new CylinderShape3D { Radius = _def.Radius, Height = 2f }
        };
        _shape.Shape = s;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body == _caster || !_hitbox.Monitoring) return;
        if (_hitSet.Contains(body.GetInstanceId())) return;
        if (body is not IDamageable target || body is not Character targetChar) return;
        if (_def.HitShape == HitShapeType.Cone && !IsInsideCone(body.GlobalPosition)) return;

        if (!target.CanTakeDamage(_caster.FactionId)) return;
        _hitSet.Add(body.GetInstanceId());
        target.ReceiveDamage(_def.BaseDamage, _caster);
    }

    private bool IsInsideCone(Vector3 point)
    {
        var to = (point - GlobalPosition).Normalized();
        var dot = Mathf.Clamp(_forward.Dot(to), -1f, 1f);
        var angle = Mathf.RadToDeg(Mathf.Acos(dot));
        return angle <= _def.AngleDeg * 0.5f;
    }
}
