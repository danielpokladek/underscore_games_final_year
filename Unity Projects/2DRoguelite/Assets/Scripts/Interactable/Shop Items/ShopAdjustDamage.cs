using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustDamage : InteractableItem
{
    [SerializeField] private float damageModifier;

    public override void Interact(PlayerController playerController)
    {
        if (!CheckGems())
            return;

        playerController.playerStats.characterAttackDamage.AddModifier(damageModifier);
        
        PurchaseItem(itemPrice);
        Destroy(gameObject);
    }
}
