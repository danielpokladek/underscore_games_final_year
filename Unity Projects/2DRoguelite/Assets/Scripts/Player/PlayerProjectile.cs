using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDamage(projectileDamage);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //gameUIManager.DamageIndicator(other.transform.position, projectileDamage);

            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
        
        if (other.gameObject.CompareTag("BossHitPoint"))
        {
            other.transform.parent.GetComponent<BossController>().DamageBoss(projectileDamage);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("BossProjectile"))
        {
            other.gameObject.GetComponent<BossBullet>().DamageBullet(projectileDamage);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
    }
}
