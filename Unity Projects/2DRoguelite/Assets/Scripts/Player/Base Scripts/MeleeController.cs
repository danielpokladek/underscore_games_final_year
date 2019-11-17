﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : PlayerController
{
    [Header("Melee Settings")]
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask enemiesLayer;
    
    // ------------------------------
    protected float currentAttackDelay;
    protected Collider2D[] enemiesInRange;

    override protected void Update()
    {
        base.Update();

        if (currentAttackDelay >= attackDelay)
        {
            if (Input.GetButtonDown("LMB"))
                PrimAttack();
        }
        else
        {
            currentAttackDelay += Time.deltaTime;
        }
    }

    protected void DamageEnemy(GameObject enemyObject)
    {
        if (enemyObject.CompareTag("Enemy"))
        {
            enemyObject.GetComponent<EnemyController>().Damage(damageAmount);
        }
    }

    override protected void PlayerAim()
    {
        base.PlayerAim();

        enemiesInRange = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}