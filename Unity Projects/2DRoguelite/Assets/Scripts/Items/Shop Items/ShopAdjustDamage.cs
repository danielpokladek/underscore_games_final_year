using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustDamage : ShopItem
{
    [SerializeField] private float damageAmount;

    public override void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        playerController.DamageAmount += damageAmount;
        Destroy(gameObject);
    }
}
