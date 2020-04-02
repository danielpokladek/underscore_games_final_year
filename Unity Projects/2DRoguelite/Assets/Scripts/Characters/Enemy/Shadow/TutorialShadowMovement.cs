using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TutorialShadowMovement : EnemyMovement
{
    public GameObject target;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        aiDest = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        enemyRB = GetComponent<Rigidbody2D>();

        playerTrans = target.gameObject.transform;

        aiDest.target = playerTrans;
        aiPath.maxSpeed = enemyStats.characterSpeed.GetValue();
    }
}
