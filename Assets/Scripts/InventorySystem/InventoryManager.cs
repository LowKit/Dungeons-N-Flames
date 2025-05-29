using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    List<InventoryItem> inventorySlots = new List<InventoryItem>();
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
    [SerializeField] private Transform itemHolder;
    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private int maxSlots;
    [SerializeField] private LayerMask distanceCheckLayerMask;
    public float maxDropDistance = 1.5f;

    public Item testItem;

    public int selectedSlot = -1;

    public event Action<Item> OnSelectionChange;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddItem(testItem, 1);
        }

        if (!string.IsNullOrEmpty(Input.inputString))
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= inventorySlots.Count)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public bool AddItem(Item item, int amount)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i];

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStackSize)
            {
                itemInSlot.count += amount;
                itemInSlot.RefreshCount();
                OnItemAdded?.Invoke(item);
                return true;
            }
        }
        if (inventorySlots.Count < maxSlots)
        {
            SpawnItem(item, amount);
            return true;
        }

        Debug.Log("[Inventory] Couldnt add item to Inventory (No Slots available.");

        return false;
    }

    void SpawnItem(Item item, int count)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, itemHolder);

        inventoryItem.count = count;
        inventoryItem.InitializeItem(item);
  
        inventorySlots.Add(inventoryItem);
        OnItemAdded?.Invoke(inventoryItem.item);

        Debug.Log("[Loadout] Item spawned: " + item.itemName);
    }

    public void ChangeSelectedSlot(int newValue)
    {
        // Check if newValue is within valid range (for virtual slots)
        if (newValue < 0 || newValue >= maxSlots)
        {
            Debug.LogWarning("[InventoryManager] Selected slot index out of range.");
            return;
        }

        // If already selected, deselect it
        if (selectedSlot == newValue)
        {
            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Count && inventorySlots[selectedSlot] != null)
            {
                inventorySlots[selectedSlot].Deselect();
            }
            selectedSlot = -1;
            OnSelectionChange?.Invoke(null);
            return;
        }

        // Deselect currently selected slot
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Count && inventorySlots[selectedSlot] != null)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        // Select new slot if it has an item
        if (newValue < inventorySlots.Count && inventorySlots[newValue] != null)
        {
            inventorySlots[newValue].Select();
            OnSelectionChange?.Invoke(inventorySlots[newValue].item);
        }
        else
        {
            // If slot exists virtually but no item
            OnSelectionChange?.Invoke(null);
        }

        // Update selectedSlot
        selectedSlot = newValue;
    }

    public void ChangeSelectedSlot(InventoryItem selectedItem)
    {
        // If the clicked item is already selected, deselect it
        int newSelectedIndex = inventorySlots.IndexOf(selectedItem);
        if (newSelectedIndex == selectedSlot)
        {
            inventorySlots[selectedSlot].Deselect();
            selectedSlot = -1; // Reset the selected slot
            OnSelectionChange?.Invoke(null); // Notify that no item is selected
            return;
        }

        // Deselect the currently selected slot
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Count)
        {
            inventorySlots[selectedSlot].Deselect();
            OnSelectionChange?.Invoke(null);
        }

        // Select the new slot
        if (newSelectedIndex != -1)
        {
            inventorySlots[newSelectedIndex].Select();
            selectedSlot = newSelectedIndex;
            OnSelectionChange?.Invoke(selectedItem.item); // Notify about the new selection
        }
    }


    public void DestroyItem(InventoryItem inventoryItem)
    {
        inventorySlots.Remove(inventoryItem);
        Destroy(inventoryItem.gameObject);
        OnItemRemoved?.Invoke(inventoryItem.item);
    }
    public void DestroyItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item.id == item.id)
            {
                InventoryItem inventoryItem = inventorySlots[i];
                DestroyItem(inventoryItem);
                return;
            }
        }
    }

    /*
    public void DropItem(Item item)
    {
        InventoryItem selectedItem = inventorySlots[selectedSlot];
        if (selectedItem.item.id != item.id)
        {
            Debug.Log("[InventoryManager] Incorrect Item! (Not Equipped)");
            return;
        }

        GameObject itemPrefab = GetItemPrefab(item.id);
        if (itemPrefab != null)
        {
            
            Transform orientation = PlayerController.instance.orientation;
            Vector3 direction = orientation.forward;
            float distance = GetItemSpawnDistance(orientation.position, direction,maxDropDistance);
            
            Vector3 dropPosition = orientation.position + direction * distance;
            Instantiate(itemPrefab,dropPosition,Quaternion.identity);
            
        }

        // Update inventory and notify hand inventory
        inventorySlots.Remove(selectedItem);
        Destroy(selectedItem.gameObject);
        OnSelectionChange?.Invoke(null);
    }
    */


    private float GetItemSpawnDistance(Vector3 position, Vector3 direction, float maxDistance)
    {
        bool foundHit = Physics.Raycast(position, direction, out RaycastHit hit, maxDistance, distanceCheckLayerMask);
        return foundHit ? hit.distance / 2 : maxDistance;
    }

    public Item GetSelectedItem()
    {
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Count)
            return null;

        InventoryItem itemInSlot = inventorySlots[selectedSlot];
        return itemInSlot != null ? itemInSlot.item : null;
    }
}
