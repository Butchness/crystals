using Godot;
using CrystalsOfLiora.Core;

namespace CrystalsOfLiora.Defs;

[GlobalClass, Tool]
public partial class ItemDef : Resource
{
    [Export] public string Id { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public ItemType ItemType { get; set; } = ItemType.Misc;
    [Export] public bool Stackable { get; set; } = true;
    [Export] public int MaxStack { get; set; } = 99;
    [Export] public Texture2D Icon { get; set; }
    [Export] public int Value { get; set; } = 0;
    [Export] public Godot.Collections.Array<string> Tags { get; set; } = [];

    [ExportGroup("Food")]
    [Export] public Godot.Collections.Array<EffectDef> ConsumeEffects { get; set; } = [];
    [Export] public float ConsumeCooldownS { get; set; } = 0f;

    [ExportGroup("Wearable")]
    [Export] public EquipSlotType EquipSlot { get; set; } = EquipSlotType.None;
    [Export] public StatsDef StatModifiers { get; set; }
    [Export] public Godot.Collections.Array<AttackDef> GrantsAbilities { get; set; } = [];

    [ExportGroup("Tool")]
    [Export] public Godot.Collections.Array<AttackDef> AttackSet { get; set; } = [];
    [Export] public Godot.Collections.Dictionary ToolProperties { get; set; } = [];

    [ExportGroup("Misc")]
    [Export] public Godot.Collections.Array<EffectDef> UseEffects { get; set; } = [];
    [Export] public bool Consumable { get; set; } = false;

    public override Godot.Collections.Array<Godot.Collections.Dictionary> _GetPropertyList()
    {
        var list = base._GetPropertyList();
        SetUsage(list, "ConsumeEffects", ItemType == ItemType.Food);
        SetUsage(list, "ConsumeCooldownS", ItemType == ItemType.Food);
        SetUsage(list, "EquipSlot", ItemType == ItemType.Wearable);
        SetUsage(list, "StatModifiers", ItemType == ItemType.Wearable);
        SetUsage(list, "GrantsAbilities", ItemType == ItemType.Wearable);
        SetUsage(list, "AttackSet", ItemType == ItemType.Tool);
        SetUsage(list, "ToolProperties", ItemType == ItemType.Tool);
        SetUsage(list, "UseEffects", ItemType == ItemType.Misc);
        SetUsage(list, "Consumable", ItemType == ItemType.Misc);
        return list;
    }

    private static void SetUsage(Godot.Collections.Array<Godot.Collections.Dictionary> props, string name, bool enabled)
    {
        foreach (var p in props)
        {
            if (p.TryGetValue("name", out var value) && value.AsStringName() == name)
            {
                p["usage"] = enabled ? (int)PropertyUsageFlags.Default : (int)PropertyUsageFlags.NoEditor;
                return;
            }
        }
    }
}
