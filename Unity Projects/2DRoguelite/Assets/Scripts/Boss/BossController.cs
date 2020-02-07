using UnityEngine;
using Pathfinding;

public class BossController : MonoBehaviour
{
    [Header("Base Boss Settings")]
    [SerializeField] protected float             maxHealth = 100f;
    [SerializeField] protected ParticleSystem    deathParticles;
    [SerializeField] protected float             bossDamage;

    [HideInInspector] public AIPath aiPath;
    [HideInInspector] public bool moveEnabled { get; set; }

    // ----------------
    protected Animator  animator;
    protected float     currentHealth;
    protected bool      canBeDamaged;
    
    // ----------------------------------------
    protected Transform        player;
    protected PlayerController playerController;
    protected Rigidbody2D      playerRigidbody;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator      = GetComponent<Animator>();
        
        // --- Player Stuff --- //
        player             = GameObject.FindGameObjectWithTag("Player").transform;
        playerController   = player.GetComponent<PlayerController>();
        playerRigidbody    = player.GetComponent<Rigidbody2D>();

        aiPath = GetComponent<AIPath>();

        LevelManager.instance.currentState = LevelManager.DayState.Boss;
    }

    private void Update()
    {
        if (moveEnabled)
            aiPath.destination = GameManager.current.playerRef.transform.position;
        else
            aiPath.destination = transform.position;
    }

    virtual protected void DamagePlayer(float damageAmount)
    {
        playerController.playerStats.TakeDamage(damageAmount);
    }

    virtual public void DamageBoss(float damageAmount)
    {
        if (canBeDamaged)
            currentHealth -= damageAmount;
        
        if (currentHealth <= 0)
        {
            animator.SetTrigger("bossDeath");
            canBeDamaged = false;
        }
    }

    public void EnableDamage()
    {
        canBeDamaged = true;
    }

    public void BossDeath()
    {
        Debug.Log("Boss is dead. Congrats!");

        ParticleSystem deathParticle = Instantiate(deathParticles, transform.position, Quaternion.identity);

        SaveManager.current.Save();

        Destroy(deathParticle, 4.0f);
        Destroy(gameObject);
    }
    
    public float GetHealth() { return currentHealth; }
}
