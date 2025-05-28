using UnityEngine;

public class LootChest : Interactable, IDamageable
{
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Chest Settings")]
    [SerializeField] private string chestName = "Simple Chest";
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Sprite openedSprite;
    [SerializeField] private Sprite closedSprite;

    [Header("Loot Settings")]
    [SerializeField] private int maxItemCount = 2;
    [SerializeField] private string itemDropType;
    [SerializeField] private float dropRange = 1.5f;

    int health;

    private void Start()
    {
        health = Random.Range(1, maxHealth);
        ChangeSprite(closedSprite);
    }
    public void ApplyDamage(float damage)
    {
        health = Mathf.Max(health-1, 0);
        if(health <= 0)
        {
            DropItems();
            ChangeSprite(openedSprite);
        }
    }

    public override void OnFocus()
    {
        TriggerOnFocus(chestName);
    }

    public override void OnInteract()
    {
        base.TriggerOnFocus($"HP: {health}/{maxHealth}");
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }

    private void DropItems()
    {
       int dropCount = Random.Range(0, maxItemCount);
       if (dropCount <= 0) return;

        GameObject[] prefabs = PrefabManager.Instance.GetRandomPrefabs(itemDropType, dropCount);

        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("[Enemy] Drop prefab list null or empty!");
            return;
        }

        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null)
            {
                Debug.Log("[Enemy] Couldnt drop items (Prefab is null).");
                continue;
            }

            GameObject currentDrop = Instantiate(prefab);
            Vector2 position = new Vector2(
                Random.Range(-dropRange, dropRange),
                Random.Range(-dropRange, dropRange)
            );
            currentDrop.transform.position = position;
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        if(sprite != null) spriteRenderer.sprite = sprite;
    }
}
