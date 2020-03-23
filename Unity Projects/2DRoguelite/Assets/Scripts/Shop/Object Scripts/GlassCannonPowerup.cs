using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCannonPowerup : PowerupController
{
    void Start()
    {
        playerController.playerStats.characterHealth.AddModifier(-20);

        if (playerController.playerStats.currentHealth >
            playerController.playerStats.characterHealth.GetValue())
        {
            playerController.playerStats.SetHealth(
                playerController.playerStats.characterHealth.GetValue());
        }

        playerController.playerStats.characterAttackDamage.AddModifier(10);

        Destroy(gameObject);
    }
}
