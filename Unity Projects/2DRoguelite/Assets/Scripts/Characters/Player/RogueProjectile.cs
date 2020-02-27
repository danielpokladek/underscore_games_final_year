using UnityEngine;

public class RogueProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.enemyStats.DamageOverTime(5.0f, 1.0f);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
    }
}
