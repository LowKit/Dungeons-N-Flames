using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    public static event Action<Interactable> OnInteractEvent;
    public static event Action<Interactable,string> OnFocusEvent;
    public static event Action<Interactable> OnLoseFocusEvent;

    public virtual void Awake()
    {
        gameObject.layer = 8;
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();

    protected void TriggerOnFocus(string info)
    {
        OnFocusEvent?.Invoke(this, info);
    }
    protected void TriggerOnLoseFocus()
    {
        OnLoseFocusEvent?.Invoke(this);
    }
    protected void TriggerOnInteract()
    {
        OnInteractEvent?.Invoke(this);
    }
}
