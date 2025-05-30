using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Bow : Weapon
{
    [Header("Attack Settings")]
    [SerializeField] private WeaponSettings settings;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 2;
    [SerializeField] private float attackSize = 1;

    float lastAttackTime;

    private void Update()
    {
        // Ataque
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (IsInAttackCooldown(settings.cooldown)) return;

        Vector3 mousePos = PlayerController.instance.GetMousePosition();
        Vector2 direction = (mousePos - transform.position).normalized;

        CreateProjectile(transform.position, direction);

        lastAttackTime = Time.time;
    }

    private void CreateProjectile(Vector3 position, Vector2 direction)
    {
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = position;

        int id = PlayerController.instance.GetInstanceID();
        projectile.Initialize(id, direction, projectileSpeed, settings.dano, attackSize);
    }

    private bool IsInAttackCooldown(float cooldown) => Time.time - lastAttackTime < cooldown;
}
