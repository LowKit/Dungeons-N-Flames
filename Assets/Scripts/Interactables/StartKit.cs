using UnityEngine;

public class StartKit : Interactable
{
    bool canuse = true;
    [Header("Interaction Settings")]
    [SerializeField] private string focusText = "Click F to Choose Your Equipment";
    [SerializeField] private string cantLeaveText = "You cant Choose Your Equipment again!";
    public GameObject chooseKit;
    public override void OnFocus()
    {
        TriggerOnFocus(focusText);
    }
    public override void OnInteract()
    {
        if (canuse) chooseKit.SetActive(true);
        else Debug.Log("You cant Choose Your Equipment again!");
        
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }
}
