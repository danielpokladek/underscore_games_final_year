using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustSpeed : ShopItem
{
    [SerializeField] private float movementModifier;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        
        playerController.playerStats.characterSpeed.AddModifier(movementModifier);
        playerController.onGUIUpdateCallback.Invoke();
        
        Destroy(gameObject);
    }
}
