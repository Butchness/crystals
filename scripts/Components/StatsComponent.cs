using Godot;
using CrystalsOfLiora.Defs;

namespace CrystalsOfLiora.Components;

public partial class StatsComponent : Node
{
    public StatsDef BaseStats { get; private set; }
    public float CurrentHp { get; private set; }
    public float CurrentMana { get; private set; }

    public void Configure(StatsDef stats)
    {
        BaseStats = stats;
        CurrentHp = stats?.Health ?? 1;
        CurrentMana = stats?.Mana ?? 0;
    }

    public void ApplyDamage(float amount) => CurrentHp = Mathf.Max(0, CurrentHp - amount);
    public void ApplyHeal(float amount) => CurrentHp = Mathf.Min(BaseStats?.Health ?? CurrentHp, CurrentHp + amount);
}
