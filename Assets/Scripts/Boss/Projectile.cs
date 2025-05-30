using UnityEngine;

public class Projectile : Weapon
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
    bool isActive = true;

    private void Start()
    {
        timer = lifeTime;
    }

    public void Initialize(int _owner, Vector3 _direction, float _speed, float _damage, float _attackSize)
    {
        ownerID = _owner;
        direction = _direction;
        speed = _speed;
        damage = _damage;

        RotateSprite();
        SetAttackSize(_attackSize);
        if (cast != null) AudioSource.PlayClipAtPoint(cast, transform.position);
    }

    private void Update()
    {
        if (isActive)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            CheckForDamageable();
        }

        timer -= Time.deltaTime;
        if (timer <= 0 && isActive) DestroyProjectile();
    }

    private void CheckForDamageable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius / 2, hitLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetInstanceID() == ownerID) continue;

            if (collider.TryGetComponent(out IDamageable entity))
            {
                Debug.Log(collider.name);
                entity.ApplyDamage(damage);
                DestroyProjectile();

                if (ownerID == PlayerController.instance.GetInstanceID())
                {
                    TriggerOnDamageDealt(damage);
                }
            }
        }
    }

    private void RotateSprite()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spriteTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void DestroyProjectile()
    {
        isActive = false;
        animator.SetTrigger("Explosion");
        if (explosion != null) AudioSource.PlayClipAtPoint(explosion, transform.position);
        Destroy(gameObject, 0.8f);
    }

    private void SetAttackSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }
    public void Redirect(Vector2 newDirection, int newOwnerID)
    {
        Debug.Log(newDirection + "speed: " + newDirection * speed + "speeeddd: " + (newDirection * speed).normalized);
        direction = newDirection.normalized;
        ownerID = newOwnerID;
        timer = lifeTime;
        RotateSprite();
    }
}
