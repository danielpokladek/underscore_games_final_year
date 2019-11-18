using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class BossController : MonoBehaviour
{
    [Header("Base Boss Settings")]
    [SerializeField] protected float             maxHealth = 100f;
    [SerializeField] protected ParticleSystem    deathParticles;
    [SerializeField] protected float             bossDamage;
    
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

        LevelManager.instance.currentState = LevelManager.DayState.Boss;
    }

    virtual protected void DamagePlayer(float damageAmount)
    {
        playerController.TakeDamage(damageAmount);
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

        Destroy(deathParticle, 4.0f);
        Destroy(gameObject);
    }
    
    public float GetHealth() { return currentHealth; }
}
