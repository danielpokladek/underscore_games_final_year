using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerProjectile : ItemPickup
{
    public float projectileSizeMultiplier;

    override protected void PlayerInteract(PlayerController playerController)
    {
        playerController.projectileSizeMultiplier = projectileSizeMultiplier;
        Destroy(gameObject);
    }
}
