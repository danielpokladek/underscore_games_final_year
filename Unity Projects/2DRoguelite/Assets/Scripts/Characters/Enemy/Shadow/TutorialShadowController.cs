using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShadowController : EnemyRanged
{
    protected override void AttackPlayer()
    {
        GameObject proj = ObjectPooler.instance.PoolItem("shadowProj", transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
        proj.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());


        _attackDelay = 0;
        canAttack = false;
    }
}
