using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMovement : ItemPickup
{
    [Header("Movement Properties:")]
    public float movementSpeed;

    override protected void PlayerInteract(PlayerController playerController)
    {
        playerController.MovementSpd = movementSpeed;
    }
}
