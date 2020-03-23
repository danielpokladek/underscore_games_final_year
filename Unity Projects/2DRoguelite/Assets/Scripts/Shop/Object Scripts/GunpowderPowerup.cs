using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunpowderPowerup : PowerupController
{
    [SerializeField] LayerMask enemyLayer;

    private void Start()
    {
        playerController.onTakeDamageCallback += OnPlayerDamage;
    }

    private void OnPlayerDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 5.0f);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<EnemyController>())
                enemy.GetComponent<EnemyController>().enemyStats.TakeDamage(
                    playerController.playerStats.characterAttackDamage.GetValue());
        }
    }
}
