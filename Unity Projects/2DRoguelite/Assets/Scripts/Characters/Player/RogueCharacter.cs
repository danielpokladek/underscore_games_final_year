﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCharacter : MeleeController
{
    [Header("Rogue Settings")]
    [SerializeField] private GameObject rangerArrow;

    // ------------------------------
    private bool showDebug = true;

    override protected void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F3))
            showDebug = !showDebug;
    }

    override protected void PrimAttack()
    {
        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            DamageEnemy(enemiesInRange[i].gameObject);
        }

        currentAttackDelay = 0;
    }

    override protected void SecAttack()
    {
        GameObject projectile           = Instantiate(rangerArrow, attackPoint.position, attackPoint.rotation);
        Rigidbody2D projectileRB        = projectile.GetComponent<Rigidbody2D>();

        projectileRB.AddForce(attackPoint.up * 20, ForceMode2D.Impulse);
    }
}
