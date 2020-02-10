using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public bool isDummy { get; set; }

    // --- MOVEMENT ---
    private Transform   playerTrans;
    private GameObject  playerGO;

    private Vector2     stopPoint   = new Vector2(0, 0);
    private Vector2     playerVector;

    public bool        enableMovement { get; set; }

    private float       _attackDelay;
    private float       _aimAngle;

    private EnemyController     enemyController;
    private AIPath              aiPath;
    private AIDestinationSetter aiDest;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        aiDest          = GetComponent<AIDestinationSetter>();
        aiPath          = GetComponent<AIPath>();

        playerTrans     = GameObject.FindGameObjectWithTag("Player").transform;

        aiDest.target = playerTrans;
        aiPath.maxSpeed = enemyController.enemyStats.characterSpeed.GetValue();

        enableMovement = true;
    }

    private void Update()
    {
        if (!enableMovement)
            aiPath.maxSpeed = 0;
        else
            aiPath.maxSpeed = enemyController.enemyStats.characterSpeed.GetValue();

        GetPlayerVector();
    }

    private void GetPlayerVector()
    {
        playerVector = ((Vector2)playerTrans.position - (Vector2)transform.position).normalized;
    }

    public Vector2 PlayerVector { get; set; }
    public Vector2 PlayerTrans { get; set; }
}
