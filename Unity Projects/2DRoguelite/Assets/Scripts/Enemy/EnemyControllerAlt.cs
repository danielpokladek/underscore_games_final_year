using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyControllerAlt : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected float moveSpeed = 8.0f;
    [SerializeField] protected float maxHealth = 100f;

    [Header("Pathfinding Settings")]
    [SerializeField] protected Transform aiTarget;
    [SerializeField] protected float nextWptDist = 3f;  // Next Waypoint Distance
    [SerializeField] protected float minimumDistance = 3f;
    [SerializeField] protected float maximumDistance = 10f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker aiSeeker;
    private Rigidbody2D enemyRB;

    private float currentHealth;

    private void Start()
    {
        InitiateEnemy();
        InvokeRepeating("UpdatePath", 0f, .2f);
    }

    private void InitiateEnemy()
    {
        aiSeeker = GetComponent<Seeker>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRB.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        if (Vector2.Distance(enemyRB.position, aiTarget.position) > maximumDistance)
            return;
        else if (Vector2.Distance(enemyRB.position, aiTarget.position) < minimumDistance)
            return;
        else
            enemyRB.AddForce(force);

        float distance = Vector2.Distance(enemyRB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWptDist)
            currentWaypoint++;
    }

    private void UpdatePath()
    {
        if (aiSeeker.IsDone())
            aiSeeker.StartPath(enemyRB.position, aiTarget.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw gizmo for minimum distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);

        // Draw gizmo for maximum distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maximumDistance);
    }
}
