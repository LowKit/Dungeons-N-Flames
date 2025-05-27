using SABI;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public string id;
    public string itemName;
    public string itemDescription;
    public int value;
    public int maxStackSize;
    public Sprite icon;
}
