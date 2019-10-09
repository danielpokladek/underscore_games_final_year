using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShootAI : MonoBehaviour
{
    [Header("AI Movement Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float stoppingDistance = 2.0f;
    [SerializeField] private float retreatDistance = 2.0f;
    [Header("AI Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotDelay = .8f;
    [SerializeField] private float bulletForce = 20.0f;

    private Transform player;
    private float _shotDelay;

    void Start()
    {
        // Slow, ideally have GameManager storing the player/players, and allow the AI to access that.
        // For this prototype it makes not much difference, but its something worth considering.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        _shotDelay = shotDelay;
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance &&
          Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -movementSpeed * Time.deltaTime);
        } 
    }

    private void Update()
    {
        if (_shotDelay <= 0)
        {
            GameObject instBullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 2.0f, 0), transform.rotation);
            Rigidbody2D rb = instBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
            _shotDelay = shotDelay;
        }
        else
        {
            _shotDelay -= Time.deltaTime;
        }
    }
}
