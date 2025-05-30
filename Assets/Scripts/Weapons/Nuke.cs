using UnityEngine;

public class Nuke : Weapon
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private WeaponSettings settings;
    [SerializeField] private Explosion explosion;
    
    private void Update()
    {
        // Ataque
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Explosion instance = Instantiate(explosion);
        instance.Initialize(settings, transform.position, hitLayer);
        InventoryManager.instance.DestroyItem(item);
    }
}
