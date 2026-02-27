using Godot;

namespace CrystalsOfLiora.Defs;

[GlobalClass]
public partial class StatsDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public int Health { get; set; } = 100;
    [Export] public int Mana { get; set; } = 50;
    [Export] public int Strength { get; set; } = 10;
    [Export] public int Willpower { get; set; } = 10;
    [Export] public int Agility { get; set; } = 10;
    [Export] public float Speed { get; set; } = 6f;
    [Export] public float CritChance { get; set; } = 0.05f;
    [Export] public float CritMod { get; set; } = 1.5f;
    [Export] public float HpGen { get; set; } = 0f;
    [Export] public float ManaGen { get; set; } = 0f;
    [Export] public float Cdr { get; set; } = 0f;
    [Export] public float JumpSpeed { get; set; } = 8f;
    [Export] public Godot.Collections.Array<string> Tags { get; set; } = [];
}
