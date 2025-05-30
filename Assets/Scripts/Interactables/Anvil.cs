using UnityEngine;

public class Anvil : Interactable
{
    [Header("Settings")]
    [SerializeField] private Item currency;
    [SerializeField] private string focusText = "Press [F] to upgrade Damage";
    [SerializeField] private float increment = 1.05f;
    [SerializeField] private int cost = 5;
    public override void OnFocus()
    {
         TriggerOnFocus(focusText + $"({cost} {currency.itemName}s)");
    }

    public override void OnInteract()
    {
        Weapon weapon = FindAnyObjectByType<Weapon>();
        if(weapon == null) return;

        if (InventoryManager.instance.RemoveItem(currency.id, cost))
        {
            weapon.UpgradeWeapon(increment);
            gameObject.SetActive(false);
            TriggerOnInteract("Damage upgraded");

        }
        else TriggerOnInteract("Not enough currency");
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }
}
