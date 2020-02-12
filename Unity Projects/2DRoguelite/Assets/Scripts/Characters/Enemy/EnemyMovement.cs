using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public bool isDummy { get; set; }

    // --- Public ---
    public float _aimAngle { get; private set; }
    public Vector2 playerVector { get; private set; }

    // --- Private ---
    private Transform playerTrans;
    public bool enableMovement;

    // --- --- ---
    private AIDestinationSetter aiDest;
    private AIPath              aiPath;
    private EnemyStats          enemyStats;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        aiDest = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        aiDest.target = playerTrans;
        aiPath.maxSpeed = enemyStats.characterSpeed.GetValue();

        enableMovement = true;
    }

    private void Update()
    {
        GetPlayerVector();

        if (!enableMovement)
            aiPath.maxSpeed = 0;
        else
            aiPath.maxSpeed = enemyStats.characterSpeed.GetValue();
    }

    private void GetPlayerVector()
    {
        playerVector = ((Vector2)playerTrans.position - (Vector2)transform.position).normalized;
    }

    public Vector2 PlayerVector { get; set; }
    public Vector2 PlayerTrans { get; set; }

    public bool EnableMovement { get; set; }
}
