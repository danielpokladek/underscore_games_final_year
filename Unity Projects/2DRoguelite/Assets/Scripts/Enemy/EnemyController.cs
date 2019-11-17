using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("This is enemy's maximum movement speed.")]
    [SerializeField] protected float moveSpeed = 2.5f;

    [Tooltip("This is enemy's maximum health points")]
    [SerializeField] protected float enemyHealth = 10f;
    
    [Tooltip("This is enemy's damage points; set to 0 (zero) for dummy AI")]
    [SerializeField] protected float damageAmount;

    [Tooltip("Toggle this bool to set the current enemy to a dummy AI." +
        "A dummy AI will not follow, or attack the player, and it will stand in same place." +
        "This is mainly used for debugging, and checking that the AI script is working." +
        "This can be used to test new effects, such as impact effects.")]
    [SerializeField] protected bool isDummy;

    // --- PLAYER & MOVEMENT -----------
    protected Transform     playerTrans;
    protected GameObject    playerGO;
    protected Vector2       stopPoint       = new Vector2(0, 0);

    // --- ATTACK & HEALTH -------
    protected float currentHealth;
    protected float currentDamage;
    protected float currentDelay;
    protected bool  canAttack       = false;
    protected bool  isBleeding      = false;

    // --- PATHFINDING SETTINGS --------------------
    protected AIDestinationSetter aiDestinationSetter;
    protected AIPath              aiPath;

    private void Start()
    {
        InitiateEnemy();
    }

    private void InitiateEnemy()
    {
        currentHealth = enemyHealth;

        // No need to initiate anything if the object is a dummy,
        //  so simply skip this part and move to the update loop.
        if (isDummy)
            return;

        if (!GameObject.FindGameObjectWithTag("Player"))
            Debug.LogError("Could not find a player, make sure they are tagged and present in the current scene.", gameObject);
        
        playerGO    = GameObject.FindGameObjectWithTag("Player");
        playerTrans = playerGO.transform;

        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiPath              = GetComponent<AIPath>();

        aiDestinationSetter.target  = playerTrans;
        aiPath.maxSpeed             = moveSpeed;
    }

    virtual protected void Update()
    {
        if (isDummy)
            return;

        if (LevelManager.instance.GetCurrentState == "Midnight")
            currentDamage = damageAmount * 2;
        else
            currentDamage = damageAmount;
    }

    virtual protected void FixedUpdate()
    {
        if (isDummy)
            return;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    virtual protected bool CanSeePlayer()
    {
        Vector2 rayDirection = (playerTrans.position - transform.position).normalized;

        RaycastHit2D rayHit2D = Physics2D.Raycast(transform.position, rayDirection, 10, LayerMask.GetMask("AI Raycast"));

        if (rayHit2D.collider)
        {
            if (rayHit2D.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }
}
