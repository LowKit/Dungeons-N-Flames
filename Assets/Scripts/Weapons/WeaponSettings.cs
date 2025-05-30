using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Weapons/WeaponSettings")]
public class WeaponSettings : ScriptableObject
{
    [Header("Attack Settings")]
    public float dano = 25f;
    public float raioAtaque = 3f;
    public float anguloAtaque = 90f;
    public float cooldown = 0.5f;
}
