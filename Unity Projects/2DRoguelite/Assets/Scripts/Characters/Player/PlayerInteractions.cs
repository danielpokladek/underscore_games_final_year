using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteractions : MonoBehaviour
{
    private PlayerController playerController;
    private ShopItem currentItem;

    private InteractableItem interactItem;

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
            interactItem = other.gameObject.GetComponent<InteractableItem>();
            interactItem.PlayerInRange = true;
        }

        if (other.CompareTag("ShopItem"))
        {
            currentItem = other.gameObject.GetComponent<ShopItem>();
            onItem      = true;
            currentItem.ShowName(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentItem == null)
            return;

        currentItem.ShowName(false);
        onItem      = false;
        currentItem = null;
    }

    private void PlayerInteract()
    {
        interactItem.Interact(playerController);


        // Not on item, no need to run it.
        if (currentItem == null || !onItem)
            return;

        currentItem.Interact(playerController);

    }
}
