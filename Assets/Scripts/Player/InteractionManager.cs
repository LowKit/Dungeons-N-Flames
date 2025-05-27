using SABI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private bool debugRay = true;
    [SerializeField] private KeyCode interactionKey;

    Interactable currentInteractable;
    private void Update()
    {
        Interactable interactable = CheckForInteractable();
        HandleInteractables(interactable);
        HandleInteractionInput();
    }

    private void HandleInteractables(Interactable interactable)
    {
        if (!IsCurrentInteractable(interactable))
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnLoseFocus();
            }

            currentInteractable = interactable;

            if (currentInteractable != null)
            {
                currentInteractable.OnFocus();
            }
        }
    }
    private void HandleInteractionInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.OnInteract();
        }
    }

    private Interactable CheckForInteractable()
    {
        Vector3 mouseWorldPos = playerController.GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, interactionLayer);

        if (debugRay) Debug.DrawRay(mouseWorldPos, Vector3.forward * 0.1f, hit.collider ? Color.green : Color.red);

        if (hit.collider != null && hit.collider.TryGetComponent<Interactable>(out var interactable))
        {
            return interactable;
        }

        return null;
    }
    private bool IsCurrentInteractable(Interactable interactable)
    {
        if (currentInteractable == null || interactable == null)
            return false;

        return currentInteractable.gameObject.GetInstanceID() == interactable.gameObject.GetInstanceID();
    }
}
