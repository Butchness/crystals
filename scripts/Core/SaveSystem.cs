using Godot;
using System;
using System.Text.Json;
using CrystalsOfLiora.Persistence;

namespace CrystalsOfLiora.Core;

public partial class SaveSystem : Node
{
    [Signal] public delegate void SaveCompletedEventHandler(int slot, bool success, string error);

    [Export] public int SaveSlotCount { get; set; } = 3;
    [Export] public bool AutosaveEnabled { get; set; } = true;
    [Export] public float AutosaveIntervalS { get; set; } = 120f;

    private float _autosaveTimer;

    public override void _Process(double delta)
    {
        if (!AutosaveEnabled) return;
        _autosaveTimer += (float)delta;
        if (_autosaveTimer >= AutosaveIntervalS)
        {
            _autosaveTimer = 0;
            Save(0, new WorldState());
        }
    }

    public bool Save(int slot, WorldState state)
    {
        try
        {
            var path = $"user://save_{slot}.json";
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
            file.StoreString(JsonSerializer.Serialize(state));
            EmitSignal(SignalName.SaveCompleted, slot, true, string.Empty);
            return true;
        }
        catch (Exception ex)
        {
            EmitSignal(SignalName.SaveCompleted, slot, false, ex.Message);
            return false;
        }
    }

    public WorldState Load(int slot)
    {
        var path = $"user://save_{slot}.json";
        if (!FileAccess.FileExists(path)) return new WorldState();
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        return JsonSerializer.Deserialize<WorldState>(json) ?? new WorldState();
    }
}
