

using System;

public class Weapon : HandheldItem
{
    public static event Action<float> OnDamageDealt;
}
