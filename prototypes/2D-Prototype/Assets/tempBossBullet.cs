using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class tempBossBullet : MonoBehaviour
{
    private float maxHealth = 20f;
    
    private GameObject player;
    private float currentHealth;

    [HideInInspector] public Transform target;
    [HideInInspector] public float bulletSpeed;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player");

        currentHealth = maxHealth;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, (bulletSpeed * Time.deltaTime));
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        transform.GetComponent<Rigidbody2D>().rotation = angle;
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(10f);
            Destroy(gameObject);
        }
    }
}
