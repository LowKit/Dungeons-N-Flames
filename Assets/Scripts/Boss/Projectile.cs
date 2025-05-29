using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public float speed = 1f;
    public float damage = 1;
    [HideInInspector] public Vector3 direction;

    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float raycastDistance = 0.55f;

    public Collider2D collider2d;

    float timer;
    bool canMove = true;

    private void Start()
    {
        timer = lifeTime;
    }

    private void Update()
    {
       if(canMove) transform.Translate(direction * speed * Time.deltaTime);
       CheckForDamageable();
        timer -= Time.deltaTime;
        if (timer <= 0) DestroyProjectile();
    }

    private void CheckForDamageable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, hitLayer);

        if(hit.collider != null && hit.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamage(damage);
            DestroyProjectile();
        }

    }

    public void RotateSprite(float angle)
    {
        spriteTransform.localRotation = Quaternion.Euler(0,0,angle);
    }

    public void DestroyProjectile()
    {
        collider2d.enabled = false;
        canMove = false;
        animator.SetTrigger("Explosion");
        Destroy(gameObject,0.8f);
    }

    public void SetAttackSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }
}
