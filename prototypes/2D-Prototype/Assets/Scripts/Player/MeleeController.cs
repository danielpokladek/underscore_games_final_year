using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : PlayerController
{
    [Header("Melee Settings")]
    [SerializeField] protected float damageAmount;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected Transform playerArmPivot;
    [SerializeField] protected LayerMask enemiesLayer;

    private float timeBtwAttack;

    public override void Start()
    {
        base.Start();

        timeBtwAttack = attackDelay;
    }

    public override void Update()
    {
        base.Update();

        Aim();

        if (timeBtwAttack <= 0)
            return;
        else
            timeBtwAttack -= Time.deltaTime;
    }

    private void Aim()
    {
        float weaponAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArmPivot.transform.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.back);
    }

    public override void PrimAttack()
    {
        base.PrimAttack();

        Debug.Log("Swoooosh! Swing delay: " + attackDelay + " seconds!");

        if (timeBtwAttack <= 0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damageAmount);
                Debug.Log("Damaged enemy: " + enemiesToDamage[i].name + ". With " + damageAmount + " damage!");
            }

            timeBtwAttack = attackDelay;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
