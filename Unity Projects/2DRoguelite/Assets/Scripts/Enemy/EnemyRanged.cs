using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyController
{
    [Header("Ranger Settings")]
    [SerializeField] protected GameObject   projectilePrefab;

    // --- ARMS & GUN STUFF ---------------------------------
    [SerializeField] protected SpriteRenderer   weaponSprite;

    override protected void Update()
    {
        base.Update();

        if (canAttack && CanSeePlayer())
            AttackPlayer();

        if (!canAttack)
        {
            currentAttackDelay += Time.deltaTime;

            if (currentAttackDelay >= attackDelay)
                canAttack = true;
        }
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override protected void AIAim()
    {
        base.AIAim();

        weaponSprite.sortingOrder       = 5 - 1;

        if (aimAngle > 0)
            weaponSprite.sortingOrder   = 5 + 1;
    }

    override protected void AttackPlayer()
    {
        GameObject projectile       = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
        Projectile projectileBullet = projectile.GetComponent<Projectile>();
        Rigidbody2D projectileRB    = projectile.GetComponent<Rigidbody2D>();

        projectileRB.AddForce(attackPoint.up * 10, ForceMode2D.Impulse);
        projectileBullet.SetDamage(currentDamage);

        currentAttackDelay = 0;
        canAttack          = false;
    }
}
