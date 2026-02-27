using Godot;
using CrystalsOfLiora.Core;

namespace CrystalsOfLiora.Defs;

[GlobalClass]
public partial class CharacterDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public PackedScene BrainScene { get; set; }
    [Export] public ControlType BrainType { get; set; } = ControlType.AI;
    [Export] public StatsDef Stats { get; set; }
    [Export] public InventoryDef Inventory { get; set; }
    [Export] public string FactionId { get; set; } = "neutral";
    [Export] public Godot.Collections.Array<AttackDef> AbilitySet { get; set; } = [];
    [Export] public Godot.Collections.Array<ItemDef> EquipmentSet { get; set; } = [];
    [Export] public string VisProfile { get; set; } = string.Empty;
    [Export] public string LootId { get; set; } = string.Empty;
}
