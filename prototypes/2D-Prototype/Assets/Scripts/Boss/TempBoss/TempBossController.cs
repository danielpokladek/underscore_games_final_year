using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBossController : BossController
{
    [Header("Temp Boss Settings")]
    [SerializeField] private ParticleSystem firstStageDeathParticles;
    [SerializeField] private Transform      projectileSpawnPoint;
    [SerializeField] private GameObject     stageOneProjectile;
    [SerializeField] private GameObject     stageTwoProjectile;
    [SerializeField] private float          secondStageHealth = 50f;
    [SerializeField] private GameObject[]   enemySpawnPoints;
    [SerializeField] private GameObject     enemyPrefab;
    [SerializeField] private float          bulletSpeed;
    [SerializeField] private float          bulletDamage;
    

    private List<GameObject> projectiles;
    private bool             secondStage = false;
    
    override public void DealDamage(float damage)
    {
        base.DealDamage(damage);
        
        if (currentHealth <= secondStageHealth && !secondStage)
        {
            animator.SetTrigger("secondStage");
            canBeDamaged = false;
            secondStage = true;
        }
    }
    
    // Functions below are called by the animations of the boss object.
    // These functions shouldn't be called by the script.
    #region Animation Functions
    
    // Function called by animation.
    public void ThumpAttack_MoveToPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), .1f);
    }
    
    // Function called by animation.
    public void ThumpImpact()
    {
        // Instantiate some dust particles.
        // Check if player is in range, if they are, push them back from the impact zone.
        // Also damage the player if they are too close.
    }
    
    // Function called by animation.
    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(stageOneProjectile, projectileSpawnPoint.position, Quaternion.identity);
        BossNormalBullet bossBullet = projectile.GetComponent<BossNormalBullet>();
        
        bossBullet.Bullet(player.transform, bulletSpeed, bulletDamage);

        Destroy(projectile, 2.5f);
    }
    
    // Function called by animation.
    public void StageOneDeath()
    {
        Debug.Log("Stage one done!");

        ParticleSystem deathParticle = Instantiate(firstStageDeathParticles, transform.position, Quaternion.identity);
        
        Destroy(deathParticle, 4.0f);
    }
    
    // Function called by animation.
    public void ShootProjectile()
    {
        
    }
    
    // Function called by animation.
    public void SpawnEnemies()
    {
        foreach (GameObject _go in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, _go.transform.position, Quaternion.identity);
        }
    }
    #endregion
}
