using Godot;

namespace CrystalsOfLiora.Persistence;

[GlobalClass]
public partial class ItemStackState : Resource
{
    [Export] public string ItemDefId { get; set; } = string.Empty;
    [Export] public int Quantity { get; set; } = 1;
    [Export] public Godot.Collections.Dictionary Meta { get; set; } = [];
}

[GlobalClass]
public partial class InventoryState : Resource
{
    [Export] public int SlotCount { get; set; } = 20;
    [Export] public Godot.Collections.Array<ItemStackState> Stacks { get; set; } = [];
    [Export] public int Gold { get; set; } = 0;
    [Export] public Godot.Collections.Dictionary Meta { get; set; } = [];
}
