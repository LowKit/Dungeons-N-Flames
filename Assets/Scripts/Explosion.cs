using UnityEngine;

public class Explosion : Weapon
{
    [Header("Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float destroyDelay = 0.8f;
    [SerializeField] private AudioClip explosion;
    public void Initialize(WeaponSettings settings, Vector3 position, LayerMask mask)
    {
        animator.SetTrigger("Explosion");
        CheckForDamageables(settings, position, mask);
        Destroy(gameObject, destroyDelay);
        transform.position = position;
        transform.localScale = Vector2.one * settings.raioAtaque / 1.5f;

        if (explosion != null)
        {
            AudioSource.PlayClipAtPoint(explosion, transform.position);
        }
    }
    private void CheckForDamageables(WeaponSettings settings, Vector3 position, LayerMask mask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, settings.raioAtaque / 2, mask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable entity))
            {
                entity.ApplyDamage(settings.dano * DungeonManager.instance.difficultyMultipler);
                TriggerOnDamageDealt(settings.dano);
            }
        }
    }
}
