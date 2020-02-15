using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyController
{
    [Tooltip("If the player is within this distance, the enemy will be able to attack the player")]
    [SerializeField] protected float attackRange;

    protected Collider2D[] playerInRange;

    override protected void Update()
    {
        base.Update();
        AIAim();

        if (canAttack)
            AttackPlayer();

        if (!canAttack)
        {
            _attackDelay += Time.deltaTime;

            if (_attackDelay >= enemyStats.characterAttackDelay.GetValue())
                canAttack = true;
        }
    }

    override protected void AIAim()
    {
        base.AIAim();

        playerInRange = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
    }

    override protected void AttackPlayer()
    {
        base.AttackPlayer();

        if (Vector2.Distance((Vector2)enemyMovement.playerTrans.position, (Vector2)transform.position) < attackRange)
        {
            base.AttackPlayer();

            for (int i = 0; i < playerInRange.Length; i++)
            {
                playerInRange[i].GetComponent<PlayerController>().playerStats.TakeDamage(enemyStats.characterAttackDamage.GetValue());
            }

            _attackDelay = 0;
            canAttack = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
