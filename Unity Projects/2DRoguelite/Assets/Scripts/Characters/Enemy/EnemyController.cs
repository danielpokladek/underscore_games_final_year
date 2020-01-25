using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("Toggle this bool to set the current enemy to a dummy AI." +
        "A dummy AI will not follow, or attack the player, and it will stand in same place." +
        "This is mainly used for debugging, and checking that the AI script is working." +
        "This can be used to test new effects, such as impact effects.")]
    [SerializeField] protected bool isDummy;

    [SerializeField] protected Transform armPivot;
    [SerializeField] protected Transform attackPoint;

    [SerializeField] protected LayerMask playerLayer;

    [Tooltip("This is enemy's maximum movement speed.")]
    [SerializeField] protected float moveSpeed = 2.5f;

    [Tooltip("This is enemy's maximum health points")]
    [SerializeField] protected float enemyHealth = 10f;

    [Tooltip("This is enemy's damage points; set to 0 (zero) for dummy AI")]
    [SerializeField] protected float damageAmount;

    [SerializeField] protected float attackDelay;

    [SerializeField] private GameObject healthDrop;
    [SerializeField] protected float dropPercentage;

    // --- MOVEMENT -----------
    protected Transform  playerTrans;
    protected GameObject playerGO;
    protected Vector2    stopPoint = new Vector2(0, 0);
    protected bool       movementEnabled = true;

    // --- ATTACK & HEALTH -------
    protected float currentHealth;
    protected float currentDamage;
    protected float currentAttackDelay;
    private float   bleedingLength;

    protected bool canAttack     = false;
    protected bool attackEnabled = true;
    protected bool isBleeding    = false;
    private float  bleedingTimer = 0;

    // --- PATHFINDING SETTINGS --------------------
    protected AIDestinationSetter aiDestinationSetter;
    protected AIPath aiPath;

    protected Vector2 playerVector;
    protected float   aimAngle;

    public delegate void OnEnemyDeath();
    public OnEnemyDeath onEnemyDeathCallback;

    // ---
    private LevelManager levelManager;

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

        currentDamage = damageAmount;

        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiPath              = GetComponent<AIPath>();

        aiDestinationSetter.target = playerTrans;
        aiPath.maxSpeed            = moveSpeed;

        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += NightBuff;
    }

    virtual protected void Update()
    {
        if (isDummy)
            return;

        if (isBleeding)
        {
            if (bleedingTimer >= bleedingLength)
            {
                isBleeding = false;
                CancelInvoke("BleedingDamage");
            }

            if (isBleeding == true)
                bleedingTimer += Time.deltaTime;
        }

        if (!movementEnabled)
            aiPath.maxSpeed = 0;
        else
            aiPath.maxSpeed = moveSpeed;
    }

    private void NightBuff()
    {
        if (levelManager.currentState == LevelManager.DayState.Midnight)
            currentDamage = damageAmount * 2;
        else
            currentDamage = damageAmount;
    }

    virtual protected void FixedUpdate()
    {
        if (isDummy)
            return;

        AIAim();
    }

    virtual protected void AttackPlayer()
    {
        if (!attackEnabled)
            return;
    }

    virtual protected void AIAim()
    {
        playerVector = ((Vector2)playerTrans.position - (Vector2)transform.position).normalized;
        aimAngle     = -1 * Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;

        armPivot.rotation = Quaternion.AngleAxis(aimAngle, Vector3.back);
    }

    /// <summary>
    /// While enemy is stunned, he can't move or attack, but can still take damage.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Stun(float timeDelay)
    {
        attackEnabled   = false;
        movementEnabled = false;

        yield return new WaitForSeconds(timeDelay);

        attackEnabled   = true;
        movementEnabled = true;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        GameUIManager.currentInstance.DamageIndicator(transform.position, damageAmount);

        if (currentHealth <= 0)
            KillCharacter();
    }

    public void BleedingEffect(float effectLength)
    {
        if (isBleeding)
            return;

        InvokeRepeating("BleedingDamage", 0.0f, 1.0f);

        bleedingTimer = 0;
        bleedingLength = effectLength;
        isBleeding = true;
    }

    virtual protected bool CanSeePlayer()
    {
        Vector2 rayDirection = (playerTrans.position - transform.position).normalized;

        RaycastHit2D rayHit2D = Physics2D.Raycast(transform.position, rayDirection, 10, playerLayer);

        if (rayHit2D.collider)
        {
            if (rayHit2D.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void KillCharacter()
    {
        if (Random.value >= dropPercentage)
        {
            Instantiate(healthDrop, transform.position, Quaternion.identity);
        }

        if (onEnemyDeathCallback != null)
            onEnemyDeathCallback.Invoke();

        Destroy(gameObject);
    }

    private void BleedingDamage()
    {
        TakeDamage(1);
    }
}
