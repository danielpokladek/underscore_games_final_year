using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("This is the maximum speed at which the enemy will be able to move.")]
    [SerializeField] protected float movementSpeed = 2.5f;
    [Tooltip("This is enemy's maximum health points")]
    [SerializeField] protected float maximumHealth = 10f;
    [SerializeField] protected float shootDelay = .5f;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float damageAmount;

    [Header("Other")]
    [SerializeField] protected Transform aiArmPivot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected SpriteRenderer weaponRend;

    [Header("Temporary")]
    public Transform stopPoint;
    public Transform playerTransform;
    public bool isDummy;

    private float currentHealth;
    private float delay;
    private bool canShoot = false;

    private AIPath aiPath;
    private AIDestinationSetter aiDestinationSetter;

    private void Start()
    {
        // REMOVE THIS LATER ON - THIS IS BAD DANIEL.
        if (GameObject.FindGameObjectWithTag("Player"))
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        else playerTransform = stopPoint;

        InitiateEnemy();
    }

    private void InitiateEnemy()
    {
        if (!isDummy)
        {
            aiPath = GetComponent<AIPath>();
            aiDestinationSetter = GetComponent<AIDestinationSetter>();

            aiPath.maxSpeed = movementSpeed;
            aiDestinationSetter.target = playerTransform;
        }

        currentHealth = maximumHealth;
        delay = shootDelay;
    }

    public virtual void Update()
    {
        if (isDummy)
            return;

        GunDrawLayer();
        AiRaycast();
        ShootDelay();
    }

    public void TakeDamage(float dmgAmount)
    {
        currentHealth = currentHealth - dmgAmount;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void GunDrawLayer()
    {
        if (!playerTransform)
            return;
        
        Vector2 playerVector = ((Vector2) playerTransform.position - (Vector2) transform.position).normalized;
        float weaponAngle = -1 * Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;
        aiArmPivot.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        weaponRend.sortingOrder = 0 - 1;

        if (weaponAngle > 0)
            weaponRend.sortingOrder = 0 + 1;
    }

    private void AiRaycast()
    {
        Debug.DrawLine(firePoint.position, playerTransform.position, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(firePoint.position, playerTransform.position);

        if (rayHit.collider)
        {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Player"))
                ShootPlayer();
            else
                return;
        }
    }

    private void ShootPlayer()
    {
        if (canShoot)
        {
            GameObject instBullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            Rigidbody2D instRB = instBullet.GetComponent<Rigidbody2D>();
            PlayerBullet bullet = instBullet.GetComponent<PlayerBullet>();

            instRB.AddForce(firePoint.up * 10, ForceMode2D.Impulse);
            bullet.SetDamage(damageAmount);

            canShoot = false;
            delay = shootDelay;
        }
    }

    private void ShootDelay()
    {
        delay -= Time.deltaTime;

        if (delay <= 0.0f)
            canShoot = true;
    }
}
