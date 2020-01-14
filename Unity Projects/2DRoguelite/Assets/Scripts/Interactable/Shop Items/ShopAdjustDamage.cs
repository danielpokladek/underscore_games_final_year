using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustDamage : ShopItem
{
    [SerializeField] private float damageModifier;

    public override void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        
        playerController.playerStats.characterAttackDamage.AddModifier(damageModifier);
        playerController.onGUIUpdateCallback.Invoke();
        
        Destroy(gameObject);
    }
}
