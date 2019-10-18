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

    private Vector2 shootDir;

    public override void PrimAttack()
    {
        base.PrimAttack();

        ShootProjectile();
    }

    public override void Update()
    {
        base.Update();

        GunDrawLayer();

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void GunDrawLayer()
    {
        float weaponAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArmPivot.transform.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        weaponRend.sortingOrder = 0 - 1;

        if (weaponAngle > 0)
            weaponRend.sortingOrder = 0 + 1;

    }

    private void ShootProjectile()
    {
        shootDir = mousePosition - playerRB.position;
        float proAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg - 90f;
        //firePoint.GetComponent<Rigidbody2D>().rotation = proAngle;

        GameObject instProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D instRB = instProjectile.GetComponent<Rigidbody2D>();
        instRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
    }
}
