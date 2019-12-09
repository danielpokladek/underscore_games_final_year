using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAdjustSpeed : ShopItem
{
    [SerializeField] private float moveSpeed;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        PurchaseItem(itemPrice);
        playerController.MovementSpd += moveSpeed;
        Destroy(gameObject);
    }
}
