using Godot;
using CrystalsOfLiora.Core;
using CrystalsOfLiora.Defs;

namespace CrystalsOfLiora.Components;

public partial class EquipmentComponent : Node
{
    private readonly Godot.Collections.Dictionary<EquipSlotType, ItemDef> _equipped = [];

    public void Equip(ItemDef item)
    {
        if (item == null || item.ItemType != ItemType.Wearable) return;
        _equipped[item.EquipSlot] = item;
    }

    public Godot.Collections.Array<AttackDef> GetGrantedAttacks()
    {
        var arr = new Godot.Collections.Array<AttackDef>();
        foreach (var kv in _equipped)
        {
            foreach (var attack in kv.Value.GrantsAbilities)
                arr.Add(attack);
        }
        return arr;
    }
}
