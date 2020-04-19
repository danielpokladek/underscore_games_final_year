using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : EnemyRanged
{
    [SerializeField] private Animator wandAnimator;
    [SerializeField] private Transform wandTrans;
    [SerializeField] private float attackCharge;

    private bool inShot = false;

    override protected void AttackPlayer()
    {
        if (inShot)
            return;

        inShot = true;

        StartCoroutine(ShadowAttackCoroutine());
        canAttack = false;
    }

    private IEnumerator ShadowAttackCoroutine()
    {
        wandAnimator.SetTrigger("chargingShot");

        yield return new WaitForSeconds(attackCharge);

        wandAnimator.SetTrigger("chargeFinished");
        
        canAttack = false;

        wandAnimator.ResetTrigger("chargingShot");
        yield break;
    }

    public void ShadowAttack()
    {
        wandAnimator.ResetTrigger("chargingShot");
        wandAnimator.ResetTrigger("chargeFinished");

        base.AttackPlayer();

        GameObject proj = ObjectPooler.instance.PoolItem("shadowProj", transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
            proj.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());

        _attackDelay = 0;
        canAttack   = false;
        inShot     = false;
    }

    override public void DeathEffect()
    {
        if (Random.value < 0.75f)
            return;

        int bullAmount = 8;

        float angleStep = (0 - 360) / bullAmount;
        float angle = 0;

        for (int i = 0; i < bullAmount; i++)
        {
            float bulDriX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulMoveVector = new Vector3(bulDriX, bulDirY, 0.0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = ObjectPooler.instance.PoolItem("shadowProj", transform.position, Quaternion.identity);
                bul.GetComponent<Rigidbody2D>().AddForce(bulDir * 5, ForceMode2D.Impulse);
                bul.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());

            angle += angleStep;
        }
    }
}
