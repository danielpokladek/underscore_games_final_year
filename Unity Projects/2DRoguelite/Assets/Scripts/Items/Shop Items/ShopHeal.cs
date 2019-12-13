using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeal : ShopItem
{
    [SerializeField] private float healAmount;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        if (playerController.playerStats.IsHealed())
            return;

        PurchaseItem(itemPrice);
        
        playerController.playerStats.HealCharacter(healAmount);
        playerController.onGUIUpdateCallback.Invoke();
        
        Destroy(gameObject);
    }
}
