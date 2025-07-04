using System;
using SABI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Dependencies")]
    [SerializeField] protected EnemySettings settings;
    [SerializeField] private NavMeshAgent navMeshAgent;

    float currentHeath;
    Transform playerTransform;
    PlayerController playerController;
    float lastAttackTime;
    [HideInInspector] public bool isDead = false;

    float difficultyMultiplier = 1;

    public event Action OnDeath;
    public event Action<float> OnHealthChange;
    private void Start()
    {
        playerController = GameObject.FindFirstObjectByType<PlayerController>();
        playerTransform = playerController.transform;

        UpdateSettings(difficultyMultiplier);
    }
    public void UpdateSettings(float multiplier)
    {
        if (settings == null)
        {
            Debug.LogWarning("[Enemy] Enemy Settings are null!");
            return;
        }

        difficultyMultiplier = multiplier;

        currentHeath = settings.maxHealth * difficultyMultiplier;
        navMeshAgent.speed = settings.walkSpeed * difficultyMultiplier;

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    private void Update()
    {
        if (DependenciesAreNull() || isDead) return;

        if (settings.canMove) MoveToPlayer();
        if (settings.canAttack) AttackPlayer();
    }

    private void MoveToPlayer()
    {
        float distance = DistanceToPlayer();

        if (isDead)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        if (distance <= settings.detectionDistance && distance > settings.attackDistance)
        {
            Vector3 position = new Vector3(playerTransform.position.x, playerTransform.position.y, 0);
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(position);
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void AttackPlayer()
    {
        float distance = DistanceToPlayer();

        if (distance <= settings.attackDistance)
        {
            // Stop moving
            navMeshAgent.isStopped = true;

            if (!IsInAttackCooldown(settings.attackCooldown))
            {
                playerController.ApplyDamage(settings.baseDamage * difficultyMultiplier);
                lastAttackTime = Time.time;
            }
        }
    }
    public void ApplyDamage(float damage)
    {
        currentHeath -= damage;
        if (currentHeath <= 0)
        {
            currentHeath = 0;
            OnEnemyDeath();
        }
        
        OnHealthChange?.Invoke(currentHeath);
    }

    public void UpdateHealth(float health)
    {
        currentHeath = health;
        if (currentHeath <= 0) OnDeath();
    }

    private void OnEnemyDeath()
    {
        if (isDead) return;

        isDead = true;
        DropItems();
        OnDeath?.Invoke();
        Destroy(gameObject, 0.35f);
        Debug.Log("[Enemy] Enemy died.");
    }
    private void DropItems()
    {
        if (settings.dropCount <= 0 || settings.dropType.IsNullOrEmpty()) return;
        if (Random.Range(0, settings.dropChance) > 1)
        {
            Debug.Log("[Enemy] Couldnt drop items (No chance) ");
            return;
        }

        GameObject[] prefabs = PrefabManager.Instance.GetRandomPrefabs(settings.dropType, settings.dropCount);

        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("[Enemy] Drop prefab list null or empty!");
            return;
        }

        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null)
            {
                Debug.Log("[Enemy] Couldnt drop items (Prefab is null).");
                continue;
            }

            GameObject currentDrop = Instantiate(prefab);
            Vector2 position = new Vector2(
                transform.position.x - Random.Range(-settings.dropRange, settings.dropRange),
                transform.position.y - Random.Range(-settings.dropRange, settings.dropRange)
            );
            currentDrop.transform.position = position;
        }
    }

    private bool DependenciesAreNull()
    {
        bool isNull = playerTransform == null || navMeshAgent == null || settings == null;

        if (isNull)
        {
            Debug.LogWarning("[Enemy] One of the dependencies is null!");
        }

        return isNull;
    }

    private float DistanceToPlayer() => Vector2.Distance(transform.position, playerTransform.position);
    private bool IsInAttackCooldown(float cooldown) => Time.time - lastAttackTime < cooldown * difficultyMultiplier;
}
