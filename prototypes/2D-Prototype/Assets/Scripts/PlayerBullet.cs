using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
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
            enemyController.TakeDamage(bulletDamage);

            Debug.Log("Damaged enemy: " + other.name + ". With " + bulletDamage + " damage!");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.TakeDamage(bulletDamage);
        }

        if (other.gameObject.CompareTag("BossHitPoint"))
            other.transform.parent.GetComponent<BossController>().DealDamage(bulletDamage);

        if (other.gameObject.CompareTag("BossProjectile"))
            other.gameObject.GetComponent<BossBullet>().DamageBullet(5.0f);
        
        // Instantiate a hit effect.
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // ???
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}
