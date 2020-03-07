using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeal : InteractableItem
{
    [SerializeField] private float healAmount;

    override public void Interact(PlayerController playerController)
    {
        if (CheckGems() == false)
            return;

        if (playerController.playerStats.IsHealed())
        {
            Debug.Log("Health full, maybe save it for later?");
            return;
        }

        PurchaseItem(itemPrice);
        
        playerController.playerStats.HealCharacter(healAmount);
        //playerController.onItemInteractCallback.Invoke();
        
        Destroy(gameObject);
    }
}
