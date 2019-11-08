using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : RangedController
{
    [Header("Archer Settings")]
    [SerializeField] private float bowDrawLength;
    
    // ------------------
    public float currentBowDraw;

    override protected void Update()
    {
        base.Update();
        
        if (currentBowDraw < bowDrawLength)
        {
            if (Input.GetButton("LMB"))
            {
                currentBowDraw += Time.deltaTime;
            }

            if (Input.GetButtonUp("LMB"))
            {
                currentBowDraw = 0;
            }
        }

        if (currentBowDraw >= bowDrawLength)
        {
            if (Input.GetButtonUp("LMB"))
            {
                PrimAttack();
                currentBowDraw = 0;
            }
        }
    }

    override protected void PrimAttack()
    {
        base.PrimAttack();

        ShootProjectile();
    }

    private void ShootProjectile()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject   tempProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D  projectileRB   = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerBullet bulletScript   = tempProjectile.GetComponent<PlayerBullet>();

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(playerDamage);
    }
}
