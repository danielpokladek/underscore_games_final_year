using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedSkullPowerup : PowerupController
{
    private bool modifierAdded;

    private void Start()
    {
        playerController.onTakeDamageCallback += OnPlayerDamage;
    }

    private void OnPlayerDamage()
    {
        if (playerController.playerStats.lowHealth)
        {
            playerController.playerStats.characterAttackDamage.AddModifier(10);
            modifierAdded = true;
        }
        else
        {
            if (modifierAdded)
            {
                playerController.playerStats.characterAttackDamage.RemoveModifier(10);
                modifierAdded = false;
            }
            else
            {
                return;
            }
        }
    }
}
