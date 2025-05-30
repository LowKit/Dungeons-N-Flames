using UnityEngine;

public class HealthPotion : HandheldItem
{
    [SerializeField] private int healAmount = 35;   
    public override void OnActivate()
    {
        playerController.HealPlayer(healAmount);
        InventoryManager.instance.DestroyItem(item);
        base.OnActivate();
    }
}
