using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMovement : ItemPickup
{
    [Header("Movement Properties:")]
    public float movementModifier;

    override protected void PlayerInteract(PlayerController playerController)
    {
        playerController.playerStats.characterSpeed.AddModifier(movementModifier);
    }
}
