using Godot;
using CrystalsOfLiora.Defs;
using CrystalsOfLiora.Persistence;

namespace CrystalsOfLiora.Components;

public partial class InventoryComponent : Node
{
    public InventoryState State { get; private set; } = new();

    public void Configure(InventoryDef def)
    {
        State = new InventoryState { SlotCount = def?.SlotCount ?? 20, Gold = def?.Gold ?? 0 };
        if (def == null) return;
        foreach (var stack in def.StartingItems)
        {
            State.Stacks.Add(new ItemStackState { ItemDefId = stack.ItemDefId, Quantity = stack.Quantity, Meta = stack.Meta.Duplicate() });
        }
    }

    public bool AddItem(string itemDefId, int qty)
    {
        foreach (var stack in State.Stacks)
        {
            if (stack.ItemDefId == itemDefId)
            {
                stack.Quantity += qty;
                return true;
            }
        }

        if (State.Stacks.Count >= State.SlotCount) return false;
        State.Stacks.Add(new ItemStackState { ItemDefId = itemDefId, Quantity = qty });
        return true;
    }
}
