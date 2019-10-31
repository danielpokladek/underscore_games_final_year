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
    
    // ----------------
    protected Transform player;
    protected Animator  animator;
    protected float     currentHealth;
    protected bool      canBeDamaged;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        
        // Probably get this from the stage/gameManager once its been made.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    virtual public void DealDamage(float damageAmount)
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
