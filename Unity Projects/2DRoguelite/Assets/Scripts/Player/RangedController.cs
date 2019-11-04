using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : PlayerController
{
    [Header("Ranger Settings")]
    public GameObject playerArmPivot;
    public SpriteRenderer weaponRend;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float damageAmount;
    
    private Vector2 shootDir;

    override protected void PrimAttack()
    {       
        base.PrimAttack();
        ShootProjectile();
    }

    override public void Update()
    {
        base.Update();
        GunDrawLayer();
    }

    private void GunDrawLayer()
    {
        float weaponAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArmPivot.transform.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        weaponRend.sortingOrder = 5 - 1;

        if (weaponAngle > 0)
            weaponRend.sortingOrder = 5 + 1;
    }

    private void ShootProjectile()
    {        
        shootDir = mousePosition - playerRB.position;
        float proAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg - 90f;
        //firePoint.GetComponent<Rigidbody2D>().rotation = proAngle;

        GameObject instProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D instRB = instProjectile.GetComponent<Rigidbody2D>();
        PlayerBullet instBullet = instProjectile.GetComponent<PlayerBullet>();

        instRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        instBullet.SetDamage(damageAmount);
    }
}
