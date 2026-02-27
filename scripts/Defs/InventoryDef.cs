using Godot;
using CrystalsOfLiora.Persistence;

namespace CrystalsOfLiora.Defs;

[GlobalClass]
public partial class InventoryDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public int SlotCount { get; set; } = 20;
    [Export] public Godot.Collections.Array<ItemStackState> StartingItems { get; set; } = [];
    [Export] public int Gold { get; set; } = 0;
    [Export] public Godot.Collections.Dictionary Meta { get; set; } = [];
}
