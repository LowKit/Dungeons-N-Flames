using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController instance;

    [Header("Dependecias")]
    public PlayerSettings settings;
    [SerializeField] private Camera playerCamera;

    [Header("Status")]
    [SerializeField] private Rigidbody2D rb;
    Vector2 input;

    [Header("Dash")]
    bool podeDash = true;
    bool estaADashar = false;


    public float currentHeath;
    public static Action<float, float> OnHealthChange;
    bool isDead = false;

    void Awake()
    {
        instance = this;
        currentHeath = settings.maxHealth;
        if (rb == null)
        {
            Debug.LogWarning("es burro, esqueces-te o rigid body em casa!!");
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; // Se o jogo estiver pausado, sai do update

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Movimento
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Dash
        if (Input.GetKeyDown(KeyCode.Space) && podeDash)
        {
            StartCoroutine(FazerDash());
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return; // Evita movimento durante pausa
        if (!estaADashar)
        {
            rb.linearVelocity = input * settings.velocidade;
        }
    }

    public void WarpTo(Vector3 position)
    {
        // Disable physics for a moment to avoid conflicts
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Set new position
        transform.position = position;
    }

    System.Collections.IEnumerator FazerDash()
    {
        podeDash = false;
        estaADashar = true;

        rb.linearVelocity = input * settings.dashVelocidade;

        yield return new WaitForSeconds(settings.dashDuracao);

        estaADashar = false;

        yield return new WaitForSeconds(settings.dashCooldown);
        podeDash = true;
    }


    public void ApplyDamage(float dmg)
    {
        currentHeath -= dmg;
        if (currentHeath <= 0 && !isDead)
        {
            currentHeath = 0;
            isDead = true;
            Debug.Log("morres-te, get better!!!");
        }
        OnHealthChange?.Invoke(currentHeath, settings.maxHealth);
    }
    public void HealPlayer(float amount)
    {
        currentHeath += amount;
        if (currentHeath >= settings.maxHealth)
        {
            currentHeath = settings.maxHealth;
        }

        OnHealthChange?.Invoke(currentHeath, settings.maxHealth);
    }

    public Vector3 GetMousePosition() => playerCamera.ScreenToWorldPoint(Input.mousePosition);
    public Vector3 GetDirectionToMouse() => (transform.position - GetMousePosition()).normalized;
}
