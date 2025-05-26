using UnityEngine;

[CreateAssetMenu(fileName = "Default Player", menuName = "PLayers/Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Status")]
    public float velocidade = 5f;
    public float maxHealth = 100;

    [Header("Dash")]
    public float dashVelocidade = 22f;
    public float dashDuracao = 0.15f;
    public float dashCooldown = 1f;

    [Header("Attack Settings")]
    public float dano = 25f;
    public float raioAtaque = 3f;
    public float anguloAtaque = 90f;
}
