using Godot;
using CrystalsOfLiora.Core;

namespace CrystalsOfLiora.Defs;

[GlobalClass]
public partial class EffectDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public float DurationS { get; set; } = 0f;
    [Export] public float TickRateHz { get; set; } = 0f;
    [Export] public EffectMode Mode { get; set; } = EffectMode.Additive;
    [Export] public StatsDef StatDelta { get; set; }
    [Export] public SpecialType Special { get; set; } = SpecialType.None;
    [Export] public Godot.Collections.Array<string> Tags { get; set; } = [];
}
