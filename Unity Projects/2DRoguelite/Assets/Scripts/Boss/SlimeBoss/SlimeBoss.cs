using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SlimeBoss : BossController
{
    [Header("Temp Boss Settings")]
    [SerializeField] private ParticleSystem firstStageDeathParticles;
    [SerializeField] private Transform      projectileSpawnPoint;
    [SerializeField] private float          thumpRange;
    [SerializeField] private float          thumpForce;
    [SerializeField] private GameObject     stageOneProjectile;
    [SerializeField] private GameObject     stageTwoProjectile;
    [SerializeField] private float          secondStageHealth = 50f;
    [SerializeField] private GameObject[]   meleeEnemies;
    [SerializeField] private GameObject[]   rangedEnemies;
    [SerializeField] private GameObject     meleeEnemy;
    [SerializeField] private GameObject     rangedEnemy;
    [SerializeField] private float          bulletSpeed;

    private List<GameObject> projectiles;
    private bool             secondStage = false;
    
    override public void DamageBoss(float damage)
    {
        base.DamageBoss(damage);
        
        if (currentHealth <= secondStageHealth && !secondStage)
        {
            animator.SetTrigger("secondStage");
            canBeDamaged = false;
            secondStage = true;
        }
    }
    
    #region Animation Functions
    public void ThumpAttack_MoveToPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
    
    public void ThumpImpact()
    {
        // Instantiate some dust particles upon hit.
        
        if (Vector2.Distance(transform.position, player.position) < thumpRange)
        {         
            DamagePlayer(bossDamage);

            Vector2 direction = player.position - transform.position;
            direction = direction.normalized;

            playerController.playerMovement.AddForce(direction, thumpForce, .5f);
        }
    }
    
    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(stageOneProjectile, projectileSpawnPoint.position, Quaternion.identity);
        BossNormalBullet bossBullet = projectile.GetComponent<BossNormalBullet>();
        
        bossBullet.Bullet(player.transform, bulletSpeed, bossDamage);
        Destroy(projectile, 2.5f);
    }
    
    public void StageOneDeath()
    {
        Debug.Log("Stage one done!");

        ParticleSystem deathParticle = Instantiate(firstStageDeathParticles, transform.position, Quaternion.identity);
        Destroy(deathParticle, 4.0f);
    }
    
    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(stageTwoProjectile, transform.position + new Vector3(0, .5f, 0), Quaternion.identity);
        BossHoamingBullet bossBullet = projectile.GetComponent<BossHoamingBullet>();

        bossBullet.Bullet(player.transform, bulletSpeed, bossDamage);
        //Destroy(projectile, 4.5f);
    }
    
    public void SpawnEnemies()
    {
        foreach (GameObject _go in meleeEnemies)
        {
            Instantiate(meleeEnemy, _go.transform.position, Quaternion.identity);
        }

        foreach (GameObject _go in meleeEnemies)
        {
            Instantiate(rangedEnemy, _go.transform.position, Quaternion.identity);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, thumpRange);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Boss HP: " + currentHealth);
        if (!secondStage)
            GUI.Label(new Rect(10, 25, 200, 20), "Stage 1!");
        else
            GUI.Label(new Rect(10, 25, 200, 20), "Stage 2!");
    }
}
