using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAddHealth : ShopItem
{
    [SerializeField] private float healthModifier;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        
        playerController.playerStats.characterHealth.AddModifier(healthModifier);
        playerController.onItemInteractCallback.Invoke();
        
        Destroy(gameObject);
    }
}
