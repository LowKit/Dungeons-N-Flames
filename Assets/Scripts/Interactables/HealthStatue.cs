using UnityEngine;

public class HealthStatue : Interactable
{
    [Header("Settings")]

    [SerializeField] private Item currency;
    [SerializeField] private string focusText = "Press [F] to upgrade Max Health ";
    [SerializeField] private float multiplier = 1.05f;
    [SerializeField] private int cost = 5;
    public override void OnFocus()
    {
        TriggerOnFocus(focusText + $"({cost} {currency.itemName}s)");
    }

    public override void OnInteract()
    {
        if (InventoryManager.instance.RemoveItem(currency.id, cost))
        {
            PlayerController.instance.settings.maxHealth *= multiplier;
            PlayerController.instance.HealPlayer(0);
            gameObject.SetActive(false);
            TriggerOnInteract("Max Health upgraded");

        }
        else TriggerOnInteract("Not enough currency");
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }
}
