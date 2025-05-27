using System.Collections.Generic;
using UnityEngine;

public class Loadout : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform itemHolder;
    List<HandheldItem> items = new List<HandheldItem>();
    private void Start()
    {
        //InitializeItems();

        InventoryManager.instance.OnItemRemoved += DestroyItem; 
        InventoryManager.instance.OnSelectionChange += ChangeSelectedItem;
        InventoryManager.instance.OnItemAdded += SpawnItem;
    }
    private void OnDisable()
    {
        InventoryManager.instance.OnItemRemoved -= DestroyItem; 
        InventoryManager.instance.OnSelectionChange -= ChangeSelectedItem;
        InventoryManager.instance.OnItemAdded -= SpawnItem;
    }

    private void ChangeSelectedItem(Item item)
    {
        DisableItems();
        HandheldItem handItem = GetItem(item);

        if (handItem == null)
        {
            Debug.Log("[Loadout] Failed to equip, GameObject is null!");
            return;
        }

        handItem.gameObject.SetActive(true);
        handItem.OnActivate();
    }
    private void InitializeItem(GameObject instance, Item item)
    {
        if (instance.TryGetComponent(out HandheldItem handItem))
        {
            handItem.playerController = playerController;
            handItem.loadout = this;
            handItem.item = item;
            items.Add(handItem);
        }
        else
        {
            Debug.Log("[Loadout] Instantiated item: " + item.name + ", does not derive from Handheld Component.");
        }
    }

    private void SpawnItem(Item item)
    {
        if (item == null)
        {
            Debug.Log("[Loadout] Item is null, cant add item.");
            return;
        }

        GameObject prefab = PrefabManager.Instance.GetObjectPrefab("handheld", item.id);

        if (prefab == null)
        {
            Debug.Log("[Loadout] Prefab is null, cant add item.");
            return;
        }

        GameObject instance = Instantiate(prefab, itemHolder);
        InitializeItem(instance, item);
    }
    private void DestroyItem(Item item)
    {
        Debug.Log(item);

        if (item == null)
        {
            Debug.Log("[Loadout] Item is null, cant remove item.");
            return;
        }

        HandheldItem handheldItem = GetItem(item);

        if (handheldItem == null)
        {
            Debug.Log("[Loadout] Failed to remove, HandheldItem is null!");
            return;
        }
        Debug.Log(items.Count);
        items.Remove(handheldItem);
        Debug.Log(items.Count);
        Destroy(handheldItem.gameObject);
    }

    private HandheldItem GetItem(Item item)
    {
        if (item == null) return null;

        foreach (HandheldItem handItem in items)
        {
            if (handItem.item.id == item.id) return handItem;
        }

        Debug.LogWarning("[Inventory] Couldnt find item in list. (Not Found)");
        return null;
    }

    private void DisableItems()
    {
        foreach (Transform child in itemHolder)
        {
            child.gameObject.SetActive(false);
        }
    }
}

