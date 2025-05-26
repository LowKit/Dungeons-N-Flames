using UnityEngine;

[CreateAssetMenu(fileName = "Default Enemy", menuName = "Enemies/Enemy Settings", order = 1)]
public class EnemySettings : ScriptableObject
{
    [Header("Status")]
    public int maxHealth = 100;

    [Header("Movement Settings")]
    public float walkSpeed = 5;

    [Header("Attack Settings")]
    public float baseDamage = 25;
    public float attackCooldown = 0.5f;
    public float attackDistance = 3;

    [Header("Detection Settings")]
    public float detectionDistance = 7.5f;
}
