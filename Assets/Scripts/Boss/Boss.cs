using System;
using DG.Tweening;
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

    int id;

    private void OnEnable()
    {
        target = FindAnyObjectByType<PlayerController>().transform;
        id = gameObject.GetInstanceID();

        switch (fightStage)
        {
            case 0:
                attackPatterns = new Action[] { SimpleAttack, TripleAttack };
                currentStage = stages[0];
                break;
            case 1:
                attackPatterns = new Action[] { SimpleAttack, TripleAttack, RingAttack };
                currentStage = stages[1];
                break;
            case 2:
                attackPatterns = new Action[] { SimpleAttack, TripleAttack, RingAttack, ConvergingAttack };
                currentStage = stages[2];
                break;
        }

        base.UpdateHealth(currentStage.maxHealth);
        UpdateAttackRate(currentStage.attackCooldown);

        base.OnDeath += HandleBossDefeat;
    }
    private void OnDisable()
    {
        base.OnDeath -= HandleBossDefeat;
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
        CreateProjectile(transform.position, direction, 1.5f);
    }

    private void TripleAttack()
    {
        float[] anglesOffset = { -30f, 0f, 30f };

        foreach (float offset in anglesOffset)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;

            float radianAngle = angle * Mathf.Deg2Rad;
            Vector3 offsetDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0);

            CreateProjectile(transform.position, offsetDirection, 0.75f);
        }
    }

    private void ConvergingAttack()
    {
        int projectileCount = 8;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * (360f / projectileCount);
            float radianAngle = angle * Mathf.Deg2Rad;
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0) * 2f;

            Vector3 direction = (target.position - spawnPosition).normalized;

            CreateProjectile(spawnPosition, direction, 1f);
        }
    }

    private void RingAttack()
    {
        int projectileCountPerRing = 12;
        float radius = 1f;

        for (int i = 0; i < projectileCountPerRing; i++)
        {
            float angle = i * (360f / projectileCountPerRing);
            float radianAngle = angle * Mathf.Deg2Rad;

            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0) * radius;
            Vector3 direction = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0);

            CreateProjectile(spawnPosition, direction, 1);
        }
    }

    private void CreateProjectile(Vector3 position, Vector2 direction, float speedMultiplier)
    {
        GameObject projectileGM = Instantiate(projectilePrefab);
        Projectile projectile = projectileGM.GetComponent<Projectile>();
        projectileGM.transform.position = position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        projectile.Initialize(id, direction, projectileSpeed * speedMultiplier, settings.baseDamage, currentStage.attackSize, angle);
    }

    private void HandleBossDefeat()
    {
        Debug.Log($"[Boss] Stage {currentStage} Boss defeated.");
        CancelInvoke();
        fightStage++;
    }
}
[System.Serializable]
public class BossStage
{
    public int maxHealth = 100;
    public float attackCooldown = 1.0f;
    public float attackSize = 1;
}
