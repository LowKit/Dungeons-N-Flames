using System.ComponentModel;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private Item sword;
    [SerializeField] private Item bow;
    [SerializeField] private StartKit kit;


    public void PickBow()
    {
        if (InventoryManager.instance.AddItem(bow, 1))
        {
            Debug.Log("Arco adicionado!");
            kit.choosed();
        }


    }

    public void PickSword()
    {
        if (InventoryManager.instance.AddItem(sword, 1))
        {
            Debug.Log("Espada adicionada!");
            kit.choosed();
        }
    }
}
