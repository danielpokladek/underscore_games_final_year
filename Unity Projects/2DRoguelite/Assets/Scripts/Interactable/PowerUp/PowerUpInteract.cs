using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInteract : InteractableItem
{
    [Tooltip("This is the item which will be spawned on player upon interaction," +
        "this can be the energy balls, a magic barrier, etc.")]
    [SerializeField] private GameObject powerUpEffect;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems())
            return;

        GameObject temp = Instantiate(powerUpEffect, playerController.transform.position, Quaternion.identity);
        temp.transform.SetParent(playerController.powerUpContainer);

        Destroy(gameObject);
    }
}
