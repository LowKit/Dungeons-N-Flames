

using System;

public class Weapon : HandheldItem
{
    public static event Action<float> OnDamageDealt;

    protected void TriggerOnDamageDealt(float dmg)
    {
        OnDamageDealt?.Invoke(dmg);
    }
}
