using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerProjectile : InteractableItem
{
    public float projectileSizeMultiplier;

    override public void Interact(PlayerController playerController)
    {
        playerController.projectileSizeMultiplier = projectileSizeMultiplier;
        Destroy(gameObject);
    }
}
