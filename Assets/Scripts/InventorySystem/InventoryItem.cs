using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject toggle;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform currentParent;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        nameText.text = item.itemName;
        image.sprite = newItem.icon;
        RefreshCount();
        Deselect();
    }
    public void RefreshCount()
    {
        bool countActive = count > 1;

        if (countActive) nameText.text = $"{item.itemName} ({count})";
        else nameText.text = item.itemName;
    }
    public void Select()
    {
        toggle.SetActive(true);
    }
    public void Deselect()
    {
        toggle.SetActive(false);
    }
    public void DropItem()
    {
        InventoryManager.instance.DropItem(item);
    }
    // Detect click on this inventory item
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.instance.ChangeSelectedSlot(this);
        }
    }
}
