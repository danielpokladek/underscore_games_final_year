﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHeal : ShopItem
{
    [SerializeField] private float healAmount;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems(itemPrice))
            return;

        if (playerController.isHealed())
            return;

        playerController.HealPlayer(healAmount);
        Destroy(gameObject);
    }
}
