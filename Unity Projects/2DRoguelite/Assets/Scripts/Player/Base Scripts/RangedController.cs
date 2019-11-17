using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : PlayerController
{
    [Header("Ranger Settings")]
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
        weaponSprite.sortingOrder     = 5 - 1;

        if (armAngle > 0)
            weaponSprite.sortingOrder = 5 + 1;
    }
}
