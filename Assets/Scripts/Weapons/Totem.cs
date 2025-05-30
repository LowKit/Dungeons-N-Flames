using UnityEngine;

public class Totem : HandheldItem
{
    [SerializeField] private int healAmount = 100;
    private void Awake()
    {
        base.keepActive = true;
        PlayerController.OnHealthChange += CheckHealth;
    }
    private void OnDestroy()
    {
        PlayerController.OnHealthChange -= CheckHealth;
    }

    public void CheckHealth(float health, float maxHealth)
    {
        float ratio = health / maxHealth;
        if (ratio <= 0.3f)
        {
            PlayerController.instance.HealPlayer(healAmount);
            InventoryManager.instance.DestroyItem(item);
        }
    }
}
