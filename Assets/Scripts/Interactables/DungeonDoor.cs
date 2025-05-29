using UnityEngine;

public class DungeonDoor : Interactable
{
    [SerializeField] private DungeonRoom dungeonRoom;

    [Header("Interaction Settings")]
    [SerializeField] private string focusText = "Leave Room";
    [SerializeField] private string cantLeaveText = "You cant leave the room right now.";
    [SerializeField] private string interactionSucessText = "Door opened";
    public override void OnFocus()
    {
        TriggerOnFocus(focusText);
    }
    public override void OnInteract()
    {
        bool canLeave = dungeonRoom.CanLeave();
        string tooltip = canLeave ? interactionSucessText : cantLeaveText;
        base.TriggerOnInteract(tooltip);

        if (canLeave) DungeonManager.instance.EnterRandomRoom();
        else Debug.Log("[Door] Cant leave right now");
    }

    public override void OnLoseFocus()
    {
        TriggerOnLoseFocus();
    }
}
