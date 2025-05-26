using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemySettings settings;
    [SerializeField] private NavMeshAgent navMeshAgent;

    Transform playerTransform;
    PlayerController playerController;
    float lastAttackTime;
    bool hasAttacked = false;
    private void Start()
    {
        playerController = GameObject.FindFirstObjectByType<PlayerController>();
        playerTransform = playerController.transform;

        ApplySettings();
    }
    private void ApplySettings()
    {
        if (settings == null)
        {
            Debug.LogWarning("[Enemy] Enemy Settings are null!");
            return;
        }

        navMeshAgent.speed = settings.walkSpeed;

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    private void Update()
    {
        if (DependenciesAreNull()) return;

        MoveToPlayer();
        AttackPlayer();
    }

    private void MoveToPlayer()
    {
        if (DistanceToPlayer() <= settings.detectionDistance)
        {
            Vector3 position = new Vector3(playerTransform.position.x, playerTransform.position.y, 0);
            navMeshAgent.SetDestination(position);
        }
    }

    private void AttackPlayer()
    {
        if (DistanceToPlayer() <= settings.attackDistance && !IsInAttackCooldown(settings.attackCooldown / 2) && !hasAttacked)
        {
            // Attack Player
            lastAttackTime = Time.time;
            hasAttacked = true;
        }
        else if (DistanceToPlayer() <= settings.attackDistance && !IsInAttackCooldown(settings.attackCooldown) && hasAttacked)
        {
            // Attack Player
            lastAttackTime = Time.time;
        }
        else if (DistanceToPlayer() > settings.attackDistance)
        {
            hasAttacked = false;
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
    private bool IsInAttackCooldown(float cooldown) => Time.time - lastAttackTime < cooldown;
}
