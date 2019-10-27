using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class BossController : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] protected Collider2D[] hitPoints;
    
    protected Transform player;
    
    [SerializeField] private float currentHealth;
    private Animator animator;
    
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void DealDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 50)
        {
            animator.SetTrigger("stageTwo");

            foreach (Collider2D col in hitPoints)
                col.enabled = false;
        }

        if (currentHealth <= 0)
            animator.SetTrigger("dead");
    }

    public void BossDeath()
    {
        Debug.Log("Boss is dead. Congrats!");

        ParticleSystem deathParticle = Instantiate(deathParticles, transform.position, Quaternion.identity);

        Destroy(deathParticle, 4.0f);
        Destroy(gameObject);
    }
}
