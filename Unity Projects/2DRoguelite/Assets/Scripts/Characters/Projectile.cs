using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [Header("Bullet Settings")] 
    [SerializeField] protected GameObject hitEffect;
    [Tooltip("This is the time after which the projectile will destroy itself.")]
    [SerializeField] protected float projectileDecay;
    [Tooltip("This is the tag which is used in object pooling, it should be the same as in object pooling script.")]
    [SerializeField] protected string projectileTag;
    
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

        if (string.IsNullOrEmpty(projectileTag))
            Debug.LogError("Projectile tag not set at: " + gameObject.name, gameObject);
    }

    virtual protected void InitProjectile() { }

    virtual public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }

    virtual protected void DestroyProjectile()
    {
        projectileRB.velocity = Vector3.zero;
        ObjectPooler.instance.AddItem(projectileTag, this.gameObject);
        gameObject.SetActive(false);
    }
}
