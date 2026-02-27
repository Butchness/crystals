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
            GD.Print("[SaveSystem] Autosave interval reached; saving slot 0.");
            Save(0, new WorldState());
        }
    }

    public bool Save(int slot, WorldState state)
    {
        try
        {
            var path = $"user://save_{slot}.json";
            GD.Print($"[SaveSystem] Saving slot {slot} to {path}.");
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                GD.PushError($"[SaveSystem] Save failed for slot {slot}: could not open file.");
                EmitSignal(SignalName.SaveCompleted, slot, false, "Could not open save file for writing.");
                return false;
            }

            file.StoreString(JsonSerializer.Serialize(state));
            EmitSignal(SignalName.SaveCompleted, slot, true, string.Empty);
            GD.Print($"[SaveSystem] Save completed for slot {slot}.");
            return true;
        }
        catch (Exception ex)
        {
            EmitSignal(SignalName.SaveCompleted, slot, false, ex.Message);
            GD.PushError($"[SaveSystem] Save failed for slot {slot}: {ex.Message}");
            return false;
        }
    }

    public WorldState Load(int slot)
    {
        var path = $"user://save_{slot}.json";
        GD.Print($"[SaveSystem] Loading slot {slot} from {path}.");
        if (!FileAccess.FileExists(path))
        {
            GD.Print($"[SaveSystem] No save file for slot {slot}; returning new WorldState.");
            return new WorldState();
        }

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        var state = JsonSerializer.Deserialize<WorldState>(json) ?? new WorldState();
        GD.Print($"[SaveSystem] Load complete for slot {slot}.");
        return state;
    }
}
