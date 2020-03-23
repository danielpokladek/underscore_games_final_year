using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteractions : MonoBehaviour
{
    private PlayerController playerController;

    private InteractableItem currentItem;

    private bool onItem;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.onInteractCallback += PlayerInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentItem = other.gameObject.GetComponent<InteractableItem>();
            currentItem.PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentItem == null)
            return;

        onItem      = false;
        currentItem = null;
    }

    private void PlayerInteract()
    {
        if (currentItem == null)
            return;

        currentItem.Interact(playerController);
        UIManager.current.updateUICallback.Invoke();
    }
}
