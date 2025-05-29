using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;
    private float speed = 1f;
    private float damage = 1;
    private int ownerID;
    private Vector3 direction;

    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float overlapRadius = 0.55f;
    [SerializeField] private AudioClip cast;
    [SerializeField] private AudioClip explosion;
    float timer;
    bool canMove = true;
    bool canDamage = true;

    private void Start()
    {
        timer = lifeTime;
    }

    public void Initialize(int _owner, Vector3 _direction, float _speed, float _damage, float _attackSize, float _spriteAngle)
    {
        ownerID = _owner;
        direction = _direction;
        speed = _speed;
        damage = _damage;

        RotateSprite(_spriteAngle);
        SetAttackSize(_attackSize);
        if(cast != null) AudioSource.PlayClipAtPoint(cast, transform.position);
    }

    private void Update()
    {
        if (canMove && canDamage)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            CheckForDamageable();
        }

        timer -= Time.deltaTime;
        if (timer <= 0) DestroyProjectile();
    }

    private void CheckForDamageable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius / 2, hitLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetInstanceID() == ownerID) continue;

            if (collider.TryGetComponent(out IDamageable entity))
            {
                entity.ApplyDamage(damage);
                DestroyProjectile();
            }
        }
    }

    private void RotateSprite(float angle)
    {
        spriteTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void DestroyProjectile()
    {
        canMove = false;
        canDamage = false;
        animator.SetTrigger("Explosion");
        if(explosion != null) AudioSource.PlayClipAtPoint(explosion, transform.position);
        Destroy(gameObject, 0.8f);
    }

    private void SetAttackSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }
}
