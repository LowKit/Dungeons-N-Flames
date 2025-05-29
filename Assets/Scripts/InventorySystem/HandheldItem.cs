using UnityEngine;

public class HandheldItem : MonoBehaviour
{
    public Item item;
    public bool keepActive = false;

    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Loadout loadout;

    public virtual void OnActivate() { }
    public virtual void OnDeactivate(){} 
}
