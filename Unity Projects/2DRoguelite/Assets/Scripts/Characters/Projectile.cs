using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Bullet Settings")] 
    [SerializeField] protected GameObject hitEffect;
    
    // ----------------------------
    protected Rigidbody2D projectileRB;
    protected float       projectileDamage;

    virtual protected void InitProjectile()
    {
        projectileRB = GetComponent<Rigidbody2D>();
    }

    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }
}
