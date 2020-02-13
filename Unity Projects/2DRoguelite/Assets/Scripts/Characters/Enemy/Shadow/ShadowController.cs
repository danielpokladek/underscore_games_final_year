using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : EnemyRanged
{
    [SerializeField] private Animator wandAnimator;
    [SerializeField] private Transform wandTrans;
    [SerializeField] private float attackCharge;

    private bool wandAim = false;

    override protected void AttackPlayer()
    {
        StartCoroutine(ShadowAttackCoroutine());
        canAttack = false;
    }

    private IEnumerator ShadowAttackCoroutine()
    {
        wandAnimator.SetTrigger("chargingShot");

        yield return new WaitForSeconds(attackCharge);

        wandAnimator.SetTrigger("chargeFinished");

        wandAim = true;
        canAttack = false;

        wandAnimator.ResetTrigger("chargingShot");

        yield break;
    }

    public void ShadowAttack()
    {
        Debug.Log("Shadow");

        wandAnimator.ResetTrigger("chargingShot");
        wandAnimator.ResetTrigger("chargeFinished");

        base.AttackPlayer();
        
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);

        Projectile projectileBullet = projectile.GetComponent<Projectile>();
        Rigidbody2D projectileRB    = projectile.GetComponent<Rigidbody2D>();

        projectileRB.AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
        projectileBullet.SetDamage(currentDamage);

        currentAttackDelay = 0;
        canAttack = false;
        //wandAim = false;
    }

    private void OnGUI()
    {
        Debug.DrawLine(transform.position, enemyMovement.playerVector, Color.red, 2, false);
    }
}
