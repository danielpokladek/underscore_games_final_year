using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyController
{
    [Header("Ranger Settings")]
    [SerializeField] protected GameObject   projectilePrefab;

    override protected void Update()
    {
        base.Update();
        
        if (canAttack)
            AttackPlayer();

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= enemyStats.characterAttackDelay.GetValue())
                canAttack = true;
        }
    }

    override protected void AttackPlayer()
    {
        //Debug.Log("Ranged");

        //base.AttackPlayer();

        //GameObject projectile       = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        //Projectile projectileBullet = projectile.GetComponent<Projectile>();
        //Rigidbody2D projectileRB    = projectile.GetComponent<Rigidbody2D>();

        //projectileRB.AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
        //projectileBullet.SetDamage(currentDamage);

        //currentAttackDelay = 0;
        //canAttack          = false;
    }
}
