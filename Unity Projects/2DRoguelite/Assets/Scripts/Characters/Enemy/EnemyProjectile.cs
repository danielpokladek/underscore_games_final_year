using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.playerStats.TakeDamage(projectileDamage);

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
