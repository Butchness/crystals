namespace CrystalsOfLiora.Entities;

public interface IDamageable
{
    bool CanTakeDamage(string sourceFaction);
    void ReceiveDamage(float amount, Character source);
}
