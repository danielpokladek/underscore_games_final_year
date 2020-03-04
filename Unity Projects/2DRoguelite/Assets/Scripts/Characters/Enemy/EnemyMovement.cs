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
    private AIPath aiPath;
    private AIDestinationSetter aiDest;
    private EnemyStats enemyStats;
    private Rigidbody2D enemyRB;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        aiDest = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        enemyRB = GetComponent<Rigidbody2D>();

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

    public IEnumerator AddForce(Vector2 force, float strenght)
    {
        enableMovement = false;
        aiPath.enabled = false;

        enemyRB.velocity = Vector2.zero;
        enemyRB.AddForce(force * strenght, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.4f);

        aiPath.enabled = true;
        enableMovement = true;
    }
}
