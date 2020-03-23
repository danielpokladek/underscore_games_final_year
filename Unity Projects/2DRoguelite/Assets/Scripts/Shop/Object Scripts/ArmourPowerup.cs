using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPowerup : PowerupController
{
    private void Start()
    {
        playerController.onTakeDamageCallback += OnPlayerDamage;
    }

    private void OnPlayerDamage()
    {
        playerController.onTakeDamageCallback -= OnPlayerDamage;
        playerController.playerStats.HealCharacter(10);

        Destroy(gameObject);
    }
}
