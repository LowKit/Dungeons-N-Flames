using System.Collections.Generic;
using UnityEngine;

public class Loadout : MonoBehaviour
{
    [SerializeField] private Transform itemHolder;
    List<HandItem> items = new List<HandItem>();
    private void Start() 
    {
        InitializeItems();

        InventoryManager.instance.OnSelectionChange += ChangeSelectedItem;
    }
    private void OnDisable() 
    {
        InventoryManager.instance.OnSelectionChange -= ChangeSelectedItem;
    }

    private void ChangeSelectedItem(Item item)
    {
        DisableItems();
        HandItem handItem = GetItem(item);

        if(handItem == null) return;

        handItem.gameObject.SetActive(true);
    }
    private void InitializeItems()
    {
        foreach(Transform item in itemHolder)
        {
            if(item.TryGetComponent<HandItem>(out HandItem handItem))
            {
                items.Add(handItem);
            }
        }
    }

    private HandItem GetItem(Item item)
    {
        if(item == null) return null;

        foreach(HandItem handItem in items)
        {
            if(handItem.item.id == item.id) return handItem;
        }

        Debug.LogWarning("[Inventory] Couldnt equip in hand. (Not Found)");
        return null;
    }

    private void DisableItems()
    {
        foreach(Transform child in itemHolder)
        {
            child.gameObject.SetActive(false);
        }
    }
}

