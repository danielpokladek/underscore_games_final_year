using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!other.GetComponent<PlayerStats>().canTakeDamage)
                return;

            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.playerStats.TakeDamage(projectileDamage);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            DestroyProjectile();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            DestroyProjectile();
        }
    }
}
