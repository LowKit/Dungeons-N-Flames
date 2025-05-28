using UnityEngine;
using UnityEngine.TextCore.Text;

public class Sword : HandheldItem
{
    [Header("Ataque")]
    [SerializeField] private LayerMask inimigoLayer;
    [SerializeField] private GameObject efeitoAtaquePrefab;
    [SerializeField] private WeaponSettings settings;
    [SerializeField] private ParticleSystem particlSystem;

    void Update()
    {
        // Ataque
        if (Input.GetMouseButtonDown(0))
        {
            Atacar();
        }
    }
    void Atacar()
    {
        Vector3 mousePos = PlayerController.instance.GetMousePosition();
        Vector2 direcaoAtaque = (mousePos - transform.position).normalized;
        particlSystem.Play();
        /* Spawn do efeito visual
        Vector3 spawnPos = transform.position; //+ (Vector3)(direcaoAtaque * raioAtaque * 0.5f); // meio do raio
        float angulo = Mathf.Atan2(direcaoAtaque.y, direcaoAtaque.x) * Mathf.Rad2Deg;
        GameObject efeito = Instantiate(efeitoAtaquePrefab, spawnPos, Quaternion.Euler(0, 0, angulo));

        // Ajusta o tamanho do efeito para caber no raio de ataque
        efeito.transform.localScale = Vector3.one * settings.raioAtaque;*/

        // Detec��o de inimigos
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(transform.position, settings.raioAtaque / 2, inimigoLayer);

        foreach (Collider2D inimigo in inimigos)
        {
            Vector2 direcaoParaInimigo = (inimigo.transform.position - transform.position).normalized;
            float anguloEntre = Vector2.Angle(direcaoAtaque, direcaoParaInimigo);

            if (anguloEntre <= settings.anguloAtaque / 2f)
            {
                Debug.Log("Hit: " + inimigo.name);
                IDamageable entity = inimigo.GetComponent<IDamageable>();
                entity.ApplyDamage(settings.dano);
            }
            else{
                Debug.Log("errei!");
            }
        }
    }
}
