using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentacao")]
    public float velocidade = 5f;
    private Vector2 input;
    [SerializeField] private Rigidbody2D rb;

    [Header("Dash")]
    public float dashVelocidade = 15f;
    public float dashDuracao = 0.2f;
    public float dashCooldown = 1f;
    private bool podeDash = true;
    private bool estaADashar = false;

    [Header("Ataque")]
    public float raioAtaque = 1.5f;
    public float anguloAtaque = 90f;
    public LayerMask inimigoLayer;
    public int dano = 1;

    void Start()
    {
        if(rb == null)
        {
            Debug.LogWarning("es burro, esqueces-te o rigid body em casa!!");
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
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
            rb.linearVelocity = input * velocidade;
        }
    }

    System.Collections.IEnumerator FazerDash()
    {
        podeDash = false;
        estaADashar = true;

        rb.linearVelocity = input * dashVelocidade;

        yield return new WaitForSeconds(dashDuracao);

        estaADashar = false;

        yield return new WaitForSeconds(dashCooldown);
        podeDash = true;
    }

    void Atacar()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcaoAtaque = (mousePos - transform.position).normalized;

        Collider2D[] inimigos = Physics2D.OverlapCircleAll(transform.position, raioAtaque, inimigoLayer);

        foreach (Collider2D inimigo in inimigos)
        {
            Vector2 direcaoParaInimigo = (inimigo.transform.position - transform.position).normalized;
            float angulo = Vector2.Angle(direcaoAtaque, direcaoParaInimigo);

            if (angulo <= anguloAtaque / 2f)
            {
                //var script = inimigo.GetComponent<Inimigo>();
                //if (script != null)
                    //script.ReceberDano(dano);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);
    }
}
