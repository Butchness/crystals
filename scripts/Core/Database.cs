using Godot;
using System;
using System.Collections.Generic;
using CrystalsOfLiora.Defs;

namespace CrystalsOfLiora.Core;

public partial class Database : Node
{
    [Export] public string ItemsPath { get; set; } = "res://defs/items";
    [Export] public string CharactersPath { get; set; } = "res://defs/characters";
    [Export] public string AttacksPath { get; set; } = "res://defs/attacks";
    [Export] public string EffectsPath { get; set; } = "res://defs/effects";
    [Export] public string InventoriesPath { get; set; } = "res://defs/inventories";

    private readonly Dictionary<string, ItemDef> _items = new();
    private readonly Dictionary<string, CharacterDef> _characters = new();
    private readonly Dictionary<string, AttackDef> _attacks = new();
    private readonly Dictionary<string, EffectDef> _effects = new();
    private readonly Dictionary<string, InventoryDef> _inventories = new();

    public override void _Ready()
    {
        GD.Print("[Database] Initializing definition indexes...");
        IndexFolder(ItemsPath, _items, nameof(ItemDef));
        IndexFolder(CharactersPath, _characters, nameof(CharacterDef));
        IndexFolder(AttacksPath, _attacks, nameof(AttackDef));
        IndexFolder(EffectsPath, _effects, nameof(EffectDef));
        IndexFolder(InventoriesPath, _inventories, nameof(InventoryDef));
        GD.Print($"[Database] Ready. Loaded Items={_items.Count}, Characters={_characters.Count}, Attacks={_attacks.Count}, Effects={_effects.Count}, Inventories={_inventories.Count}");
    }

    public T GetById<T>(string id) where T : Resource
    {
        var result = typeof(T).Name switch
        {
            nameof(ItemDef) => _items.GetValueOrDefault(id) as T,
            nameof(CharacterDef) => _characters.GetValueOrDefault(id) as T,
            nameof(AttackDef) => _attacks.GetValueOrDefault(id) as T,
            nameof(EffectDef) => _effects.GetValueOrDefault(id) as T,
            nameof(InventoryDef) => _inventories.GetValueOrDefault(id) as T,
            _ => null
        };

        GD.Print($"[Database] GetById<{typeof(T).Name}>('{id}') => {(result == null ? "MISS" : "HIT")}");
        return result;
    }

    private static void IndexFolder<T>(string path, Dictionary<string, T> map, string label) where T : Resource
    {
        GD.Print($"[Database] Indexing {label} from {path}");
        var dir = DirAccess.Open(path);
        if (dir == null)
        {
            GD.PushWarning($"Database path not found: {path}");
            return;
        }

        var loaded = 0;
        dir.ListDirBegin();
        while (true)
        {
            var file = dir.GetNext();
            if (string.IsNullOrEmpty(file)) break;
            if (file.StartsWith(".", StringComparison.Ordinal) || dir.CurrentIsDir() || !file.EndsWith(".tres")) continue;
            var res = ResourceLoader.Load<T>($"{path}/{file}");
            if (res == null) continue;
            var id = (string)res.Get("Id");
            if (string.IsNullOrEmpty(id))
            {
                GD.PushWarning($"Missing id for resource {path}/{file}");
                continue;
            }
            if (!map.TryAdd(id, res)) GD.PushError($"Duplicate id '{id}' in {path}");
            else loaded++;
        }

        GD.Print($"[Database] Indexed {loaded} {label} definition(s) from {path}");
    }
}
