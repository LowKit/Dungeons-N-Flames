

using System;

public abstract class Weapon : HandheldItem
{
    public static event Action<float> OnDamageDealt;

    protected void TriggerOnDamageDealt(float dmg)
    {
        OnDamageDealt?.Invoke(dmg);
    }

    public virtual void UpgradeWeapon(float multiplier) {}
}
