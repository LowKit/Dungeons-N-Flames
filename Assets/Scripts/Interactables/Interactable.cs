using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    public static event Action<string> OnInteractEvent;
    public static event Action<string> OnFocusEvent;
    public static event Action OnLoseFocusEvent;

    public virtual void Awake()
    {
        gameObject.layer = 8;
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();

    protected void TriggerOnFocus(string info)
    {
        OnFocusEvent?.Invoke(info);
    }
    protected void TriggerOnLoseFocus()
    {
        OnLoseFocusEvent?.Invoke();
    }
    protected void TriggerOnInteract(string info)
    {
        OnInteractEvent?.Invoke(info);
    }

    void OnDestroy()
    {
        TriggerOnLoseFocus();
    }
}
