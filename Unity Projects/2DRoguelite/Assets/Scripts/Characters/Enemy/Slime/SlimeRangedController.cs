using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRangedController : EnemyRanged
{
    override protected void AttackPlayer()
    {
        base.AttackPlayer();

        GameObject proj = Instantiate(enemyProjectile, attackPoint.position, attackPoint.rotation);
        Projectile projScr = proj.GetComponent<Projectile>();
        Rigidbody2D projRB = proj.GetComponent<Rigidbody2D>();

        projRB.AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
        projScr.SetDamage(enemyStats.characterAttackDamage.GetValue());

        _attackDelay = 0;
        canAttack = false;
    }
}
