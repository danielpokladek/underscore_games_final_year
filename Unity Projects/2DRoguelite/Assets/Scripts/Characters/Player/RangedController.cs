using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : PlayerController
{
    [SerializeField] protected SpriteRenderer weaponSprite;
    [SerializeField] protected Transform      firePoint;
    [SerializeField] protected GameObject     projectilePrefab;
    
    // ---------------------------
    protected bool    canShoot = false;
}
