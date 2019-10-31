using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")] 
    [SerializeField] protected GameObject hitEffect;
    
    // ----------------------------
    protected Rigidbody2D bulletRb;
    protected float       bulletDamage;

    virtual protected void InitBullet()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}
