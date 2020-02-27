using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustSpeed : InteractableItem
{
    [SerializeField] private float movementModifier;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems())
            return;

        PurchaseItem(itemPrice);
        
        playerController.playerStats.characterSpeed.AddModifier(movementModifier);
        playerController.onUIUpdateCallback.Invoke();
        
        Destroy(gameObject);
    }
}
