using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSpiritPowerup : PowerupController
{
    private void Start()
    {
        LevelManager.instance.onEnemyKilledCallback += OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        playerController.playerStats.HealCharacter(10);
    }
}
