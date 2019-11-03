using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBossController : BossController
{
    [Header("Temp Boss Settings")]
    [SerializeField] private ParticleSystem firstStageDeathParticles;
    [SerializeField] private Transform      projectileSpawnPoint;
    [SerializeField] private float          thumpRange;
    [SerializeField] private float          thumpForce;
    [SerializeField] private GameObject     stageOneProjectile;
    [SerializeField] private GameObject     stageTwoProjectile;
    [SerializeField] private float          secondStageHealth = 50f;
    [SerializeField] private GameObject[]   enemySpawnPoints;
    [SerializeField] private GameObject     enemyPrefab;
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
            
            StartCoroutine(PushBack(direction, thumpForce));
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
        foreach (GameObject _go in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, _go.transform.position, Quaternion.identity);
        }
    }

    IEnumerator PushBack(Vector2 direction, float force)
    {
        playerController.CanMove = false;
        playerRigidbody.AddForce(direction * force);
        
        yield return new WaitForSeconds(.5f);
        playerController.CanMove = true;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, thumpRange);
    }
}
