using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBossController : BossController
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject stageOneProjectile;

    private List<GameObject> projectiles;
    
    public void MoveToPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        //transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), .1f);
    }

    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(stageOneProjectile, projectileSpawnPoint.position, Quaternion.identity);
        tempBossBullet proRB = projectile.GetComponent<tempBossBullet>();

        proRB.target = player.transform;
        proRB.bulletSpeed = 2.5f;
        
        //Vector2 direction = (projectileSpawnPoint.position - player.position).normalized;
        //proRB.AddForce(direction, ForceMode2D.Impulse);

        Destroy(projectile, 2.5f);
    }

    public void ThumpImpact()
    {
        // Instantiate some dust particles.
        // Check if player is in range, if they are, push them back from the impact zone.
        // Also damage the player if they are too close.
    }
}
