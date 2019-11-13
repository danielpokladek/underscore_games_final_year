using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyController
{
    [Header("Ranger Settings")]
    [SerializeField] protected float        shotDelay;
    [SerializeField] protected GameObject   projectilePrefab;

    // --- ARMS & GUN STUFF ---------------------------------
    [SerializeField] protected SpriteRenderer   weaponSprite;
    [SerializeField] protected Transform        firePoint;
    [SerializeField] protected Transform        armPivot;

    // --------------------------
    private Vector2 playerVector;
    private float   weaponAngle;

    protected override void Update()
    {
        base.Update();

        if (!canAttack)
        {
            currentDelay -= Time.deltaTime;

            if (currentDelay <= 0.0f)
                canAttack = !canAttack;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        DrawGunLayer();
        AttackPlayer();
    }

    private void DrawGunLayer()
    {
        playerVector = ((Vector2)playerTrans.position - (Vector2)transform.position).normalized;
        weaponAngle  = -1 * Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;

        armPivot.rotation               = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        weaponSprite.sortingOrder       = 5 - 1;

        if (weaponAngle > 0)
            weaponSprite.sortingOrder   = 5 + 1;
    }

    virtual protected void AttackPlayer()
    {
        if (canAttack && CanSeePlayer())
        {
            GameObject projectile       = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Bullet projectileBullet     = projectile.GetComponent<Bullet>();
            Rigidbody2D projectileRB    = projectile.GetComponent<Rigidbody2D>();

            projectileRB.AddForce(firePoint.up * 10, ForceMode2D.Impulse);
            projectileBullet.SetDamage(damageAmount);

            currentDelay = shotDelay;
            canAttack    = !canAttack;
        }
    }
}
