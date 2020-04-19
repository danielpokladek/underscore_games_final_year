using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteractions : MonoBehaviour
{
    private PlayerController playerController;

    private InteractableItem currentItem;
    private bool onItem;

    private DialogueTrigger currentNPC;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.onInteractCallback += PlayerInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentItem = other.gameObject.GetComponent<InteractableItem>();
        }

        if (other.CompareTag("NPC"))
        {
            currentNPC = other.gameObject.GetComponent<DialogueTrigger>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && currentItem != null)
        {
            onItem = false;
            currentItem = null;
        }

        if (other.CompareTag("NPC") && currentNPC != null)
        {
            currentNPC = null;
        }
    }

    private void PlayerInteract()
    {
        if (currentItem != null)
        {
            currentItem.Interact(playerController);
            UIManager.current.updateUICallback.Invoke();
        }

        if (currentNPC != null)
        {
            currentNPC.StartDialogue();
        }
    }
}
