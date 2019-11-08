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
    public float delayLength;
    
    private float timeBtwAttack;
    private float curretDelayAttack;

    override public void Start()
    {
        base.Start();

        timeBtwAttack = attackDelay;
    }

    override protected void Update()
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

    override protected void PrimAttack()
    {
        if (curretDelayAttack >= delayLength)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damageAmount);
                Debug.Log("Damaged enemy: " + enemiesToDamage[i].name + ". With " + damageAmount + " damage!");
            }

            curretDelayAttack = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
