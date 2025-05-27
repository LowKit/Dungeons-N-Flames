using UnityEngine;

public class HealthPotion : HandheldItem
{
    [SerializeField] private int healAmount = 35;   
    public override void OnActivate()
    {
        playerController.ApplyDamage(-healAmount);
        InventoryManager.instance.DestroyItem(item);
        base.OnActivate();
    }
}
