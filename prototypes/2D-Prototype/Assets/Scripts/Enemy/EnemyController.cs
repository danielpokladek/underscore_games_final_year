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
    [SerializeField] protected float maximumHealth = 100f;
    [SerializeField] protected float shootDelay = .5f;
    [SerializeField] protected GameObject projectile;

    [Header("Other")]
    [SerializeField] protected Transform aiArmPivot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected SpriteRenderer weaponRend;

    [Header("Temporary")]
    public Transform playerTransform;

    private float currentHealth;
    [SerializeField] private float delay;
    private AIPath aiPath;
    private bool canShoot = false;

    private void Start()
    {
        InitiateEnemy();
    }

    private void InitiateEnemy()
    {
        aiPath = GetComponent<AIPath>();

        aiPath.maxSpeed = movementSpeed;

        currentHealth = maximumHealth;
        delay = shootDelay;
    }

    public virtual void Update()
    {
        GunDrawLayer();
        AIRaycast();
        ShootDelay();
    }

    private void GunDrawLayer()
    {
        Vector2 playerVector = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        float weaponAngle = -1 * Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;
        aiArmPivot.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.back);
        weaponRend.sortingOrder = 0 - 1;

        if (weaponAngle > 0)
            weaponRend.sortingOrder = 0 + 1;

    }

    private void AIRaycast()
    {
        Debug.DrawLine(firePoint.position, playerTransform.position, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(firePoint.position, playerTransform.position);

        if (rayHit.collider != null)
        {
            Debug.Log(rayHit.collider.name);

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
            instRB.AddForce(firePoint.up * 10, ForceMode2D.Impulse);

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
