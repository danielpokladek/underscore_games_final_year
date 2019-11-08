using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : PlayerController
{
    [Header("Ranger Settings")]
    [SerializeField] protected GameObject     playerArm;
    [SerializeField] protected SpriteRenderer weaponSprite;
    [SerializeField] protected Transform      firePoint;
    [SerializeField] protected GameObject     projectilePrefab;
    
    // ---------------------------
    protected Vector2 shootDirection;
    protected bool    canShoot = false;

    override protected void Update()
    {
        base.Update();
        
        if (playerAlive)
            GunDrawLayer();
    }

    private void GunDrawLayer()
    {
        float weaponAngle             = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArm.transform.rotation  = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        
        weaponSprite.sortingOrder     = 5 - 1;

        if (weaponAngle > 0)
            weaponSprite.sortingOrder = 5 + 1;
    }
}
