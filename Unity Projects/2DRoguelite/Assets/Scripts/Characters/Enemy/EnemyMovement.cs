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
    public Transform playerTrans;

    // --- Private ---
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

        playerTrans = GameManager.current.playerRef.transform;

        aiDest.target = playerTrans;
        aiPath.maxSpeed = enemyStats.characterSpeed.GetValue();
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
        playerVector = (playerTrans.position - transform.position).normalized;
    }
}
