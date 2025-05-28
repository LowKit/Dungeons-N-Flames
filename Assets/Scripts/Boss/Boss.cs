using System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Boss : Enemy, IDamageable
{
    [Header("Dependencies")]
    [SerializeField] private Animator animator;
    [SerializeField] private Light2D mainLight;
    [SerializeField] private AudioSource audioSource;

    [Header("Projectiles")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileRate = 1f;
    [SerializeField] private AudioClip fireballClip;

    [Header("Boss Settings")]
    [SerializeField] private BossStage[] stages = new BossStage[0];

    public static int fightStage = 0;
    Action[] attackPatterns = new Action[0];
    BossStage currentStage;
    Transform target;

    private void OnEnable()
    {
        target = FindAnyObjectByType<PlayerController>().transform;

        switch(fightStage)
        {
            case 0: 
                attackPatterns = new Action[] { SimpleAttack }; 
                currentStage = stages[0];
                break;
        }

        base.UpdateHealth(currentStage.maxHealth);
        UpdateAttackRate(currentStage.attackCooldown);
    }
    private void UpdateAttackRate(float rateMultiplier)
    {
        CancelInvoke(nameof(Attack));
        InvokeRepeating(nameof(Attack), projectileRate * rateMultiplier, projectileRate * rateMultiplier);
    }

    private void Attack()
    {
        int randomIndex = UnityEngine.Random.Range(0, attackPatterns.Length);
        attackPatterns[randomIndex].Invoke();
        audioSource.PlayOneShot(fireballClip);
    }

    private void SimpleAttack()
    {
        if (target == null)
        {
            Debug.LogWarning("[Boss] Target is null!");
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        CreateProjectile(direction, 1.5f);
    }

    private void CreateProjectile(Vector2 direction, float speedMultiplier)
    {
        GameObject projectileGM = Instantiate(projectilePrefab);
        Projectile projectile = projectileGM.GetComponent<Projectile>();
        projectileGM.transform.position = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.RotateSprite(angle);
        projectile.SetAttackSize(currentStage.attackSize);

        projectile.speed = projectileSpeed * speedMultiplier;
        projectile.direction = direction;
    }
}
[System.Serializable]
public class BossStage
{
    public int maxHealth = 100;
    public float attackCooldown = 1.0f;
    public float attackSize = 1;
}
