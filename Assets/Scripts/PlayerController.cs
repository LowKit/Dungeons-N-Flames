using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependecias")]
    [SerializeField] private PlayerSettings settings;

    [Header("Status")]
    private Vector2 input;
    [SerializeField] private Rigidbody2D rb;

    [Header("Dash")]
    bool podeDash = true;
    bool estaADashar = false;

    [Header("Ataque")]
    public LayerMask inimigoLayer;
    public GameObject efeitoAtaquePrefab; // arrasta o prefab no Inspector
    public float currentHeath;

    void Start()
    {
        currentHeath = settings.maxHealth;
        if (rb == null)
        {
            Debug.LogWarning("es burro, esqueces-te o rigid body em casa!!");
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
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

        // Ataque
        if (Input.GetMouseButtonDown(0))
        {
            Atacar();
        }
    }

    void FixedUpdate()
    {
        if (!estaADashar)
        {
            rb.linearVelocity = input * settings.velocidade;
        }
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

    void Atacar()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcaoAtaque = (mousePos - transform.position).normalized;

        // Spawn do efeito visual
        Vector3 spawnPos = transform.position; //+ (Vector3)(direcaoAtaque * raioAtaque * 0.5f); // meio do raio
        float angulo = Mathf.Atan2(direcaoAtaque.y, direcaoAtaque.x) * Mathf.Rad2Deg;
        GameObject efeito = Instantiate(efeitoAtaquePrefab, spawnPos, Quaternion.Euler(0, 0, angulo));

        // Ajusta o tamanho do efeito para caber no raio de ataque
        efeito.transform.localScale = Vector3.one * settings.raioAtaque;

        // Detecção de inimigos
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(transform.position, settings.raioAtaque, inimigoLayer);

        foreach (Collider2D inimigo in inimigos)
        {
            Vector2 direcaoParaInimigo = (inimigo.transform.position - transform.position).normalized;
            float anguloEntre = Vector2.Angle(direcaoAtaque, direcaoParaInimigo);

            if (anguloEntre <= settings.anguloAtaque / 2f)
            {
                /*var script = inimigo.GetComponent<Inimigo>();
                if (script != null)
                    script.ReceberDano(dano);*/
            }
        }
    }
    public void ApplyDamage(float dmg)
    {
        currentHeath -= dmg;
        if (currentHeath <= 0){
            Debug.Log("morres-te, get better!!!");
            currentHeath = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.raioAtaque);
    }
}
