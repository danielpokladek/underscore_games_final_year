using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMovement : InteractableItem
{
    [Header("Movement Properties:")]
    public float movementModifier;

    override public void Interact(PlayerController playerController)
    {
        playerController.playerStats.characterSpeed.AddModifier(movementModifier);
    }
}
