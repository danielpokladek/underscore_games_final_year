using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [Header("Bullet Settings")] 
    [SerializeField] protected GameObject hitEffect;
    [Tooltip("This is the time after which the projectile will destroy itself.")]
    [SerializeField] private float projectileDecay;
    [Tooltip("This is the tag which is used in object pooling, it should be the same as in object pooling script.")]
    [SerializeField] private string projectileTag;
    
    // ----------------------------
    protected Rigidbody2D projectileRB;
    protected float       projectileDamage;

    public void OnObjectSpawn()
    {
        gameObject.SetActive(true);
        Invoke("DestroyProjectile", projectileDecay);
    }

    private void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
    }

    virtual protected void InitProjectile() { }

    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }

    protected void DestroyProjectile()
    {
        projectileRB.velocity = Vector3.zero;
        ObjectPooler.instance.AddItem(projectileTag, this.gameObject);
        gameObject.SetActive(false);
    }
}
