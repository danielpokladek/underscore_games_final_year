using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;

    [HideInInspector] public float damageAmount;

    private void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDamage(damageAmount);

            Debug.Log("Damaged enemy: " + other.name + ". With " + damageAmount + " damage!");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.TakeDamage(damageAmount);
        }

        if (other.gameObject.CompareTag("BossHitPoint"))
        {
            other.transform.parent.transform.parent.GetComponent<BossController>().DealDamage(damageAmount);
        }

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}
