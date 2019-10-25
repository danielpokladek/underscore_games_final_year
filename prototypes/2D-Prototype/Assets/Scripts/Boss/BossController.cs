using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected GameObject[] spawnPoints;
    [SerializeField] protected GameObject enemyPrefab;

    [SerializeField] private float currentHealth;
    private Animator animator;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth <= (maxHealth /2))
            animator.SetTrigger("stageTwo");
    }

    public void DealDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public void AttackPlayer()
    {
        Debug.Log("Attack Player");
    }

    public void SpawnEnemies()
    {
        Debug.Log("Spawn Enemies");
    }

    public void BossDeath()
    {
        
    }
}
