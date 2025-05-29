using System;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private string focusText = "Press [F] to pickup ";
    [SerializeField] private bool isVisible = true;
    public Item item;
    private void Start()
    {
        ChangeVisibility(isVisible);
    }
    public override void OnFocus()
    {
        if(!isVisible) return;
        
        base.TriggerOnFocus(focusText + item.itemName);
    }

    public override void OnInteract()
    {
        if(!isVisible) return;

        // If inventory isnt full, do the following.
        if(InventoryManager.instance.AddItem(item, 1))
        {
            base.TriggerOnInteract("Picked up " + item.itemName);

            Destroy(gameObject);
        }
        else
        {
            base.TriggerOnFocus("Inventory Full");
        }
    }

    public override void OnLoseFocus()
    {
        base.TriggerOnLoseFocus();
    }

    public void ChangeVisibility(bool visibility)
    {
        isVisible = visibility;
        //Debug.Log("Changing item visibility: " + item.itemName + ", " + visibility);
    }
}
