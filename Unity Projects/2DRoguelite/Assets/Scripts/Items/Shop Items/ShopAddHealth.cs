using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAddHealth : ShopItem
{
    [SerializeField] private float healthAmount;

    public override void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        playerController.SetMaxHealth(healthAmount);
        Destroy(gameObject);
    }
}
