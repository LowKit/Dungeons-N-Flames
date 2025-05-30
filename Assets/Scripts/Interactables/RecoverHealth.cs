using UnityEngine;

public class RecoverHealth : Interactable
{
    bool canuse = true;
    [Header("Interaction Settings")]
    [SerializeField] private string focusText = "Click F to Recover HP";
    [SerializeField] private int healAmount = 1;
    float maxHP = 0f;

    public override void OnFocus()
    {
        TriggerOnFocus(focusText);
    }
    public override void OnInteract()
    {
        if (canuse)
        {
            maxHP = PlayerController.instance.settings.maxHealth;
            float healAmount = maxHP * 0.3f;

            PlayerController.instance.HealPlayer(healAmount);
            canuse = false;
        }
        else TriggerOnInteract("You cant Sleep Again!");
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }

    public void ToggleInteractable(bool toggle)
    {
        canuse = toggle;
    }

}
