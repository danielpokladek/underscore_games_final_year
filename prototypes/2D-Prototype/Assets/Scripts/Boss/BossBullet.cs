using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : Bullet
{
    [Tooltip("Health points of the bullet. Set to 0 to make the bullet indestructible.")]
    [SerializeField] protected float bulletHealth;
    
    // --------------------------------
    protected Transform   bulletTarget;
    protected float       bulletSpeed;
    protected float       currentHealth;
    
    // -------------------------
    protected Vector2 direction;
    protected float   angle;
    
    /// <summary>
    /// Assign bullet's parameters using this function.
    /// </summary>
    /// <param name="target">This is the target the bullet will seek towards to. Use GameObject's transform as this parameter.</param>
    /// <param name="speed">This is the speed at which the bullet will travel.</param>
    /// <param name="damage">This is the damage the bullet will deal.</param>
    virtual public void Bullet(Transform target, float speed, float damage)
    {
        bulletTarget = target;
        bulletSpeed  = speed;
        bulletDamage = damage;

        InitBullet();
    }

    override protected void InitBullet()
    {
        base.InitBullet();
        
        currentHealth = bulletHealth;
    }

    /// <summary>
    /// Damage the bullet.
    /// </summary>
    /// <param name="damage">Damage amount that will be dealt to the bullet.</param>
    virtual public void DamageBullet(float damage)
    {
        currentHealth -= damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(bulletDamage);
            DestroyBullet(0);
        }
    }

    virtual protected void DestroyBullet(float delay)
    {
        if (delay == 0)
        {
            GameObject hitFx = Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(hitFx, 0.5f);
            Destroy(gameObject);
        }
        else
        {
            GameObject hitFx = Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(hitFx, (delay + 0.5f));
            Destroy(gameObject, delay);
        }
    }
}
