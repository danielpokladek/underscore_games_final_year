using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyMovement))]
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

    [SerializeField] private GameObject[] gemDrops;

    [SerializeField] private SpriteRenderer weaponSprite;

    // --- ATTACK & HEALTH -------
    protected float currentHealth;
    protected float currentDamage;
    protected float currentAttackDelay;
    private float   bleedingLength;

    protected bool canAttack     = false;
    protected bool attackEnabled = true;
    protected bool isBleeding    = false;
    private float  bleedingTimer = 0;

    protected float   aimAngle;

    public delegate void OnEnemyDeath();
    public OnEnemyDeath onEnemyDeathCallback;

    // ---
    [HideInInspector] public EnemyMovement   enemyMovement;
    [HideInInspector] public EnemyStats      enemyStats;

    private LevelManager levelManager;

    private void Start()
    {
        enemyMovement   = GetComponent<EnemyMovement>();
        enemyStats      = GetComponent<EnemyStats>();

        InitiateEnemy();
        //enemyMovement.InitMovement();
    }

    private void InitiateEnemy()
    {
        // No need to initiate anything if the object is a dummy,
        //  so simply skip this part and move to the update loop.
        if (isDummy)
            return;

        if (!GameObject.FindGameObjectWithTag("Player"))
            Debug.LogError("Could not find a player, make sure they are tagged and present in the current scene.", gameObject);

        currentDamage = damageAmount;

        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += NightBuff;
    }

    virtual protected void Update()
    {
        if (isDummy)
            return;

        AIAim();

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
    }

    virtual protected void AttackPlayer()
    {
        if (!attackEnabled)
            return;
    }

    virtual protected void AIAim()
    {
        aimAngle          = -1 * Mathf.Atan2(enemyMovement.PlayerVector.y, enemyMovement.PlayerVector.x) * Mathf.Rad2Deg;
        armPivot.rotation = Quaternion.AngleAxis(aimAngle, Vector3.back);

        if (weaponSprite != null)
        {
            weaponSprite.sortingOrder = 5 - 1;

            if (aimAngle > 0)
                weaponSprite.sortingOrder = 5 + 1;
        }
    }

    ///// <summary>
    ///// While enemy is stunned, he can't move or attack, but can still take damage.
    ///// </summary>
    ///// <returns></returns>
    //public IEnumerator Stun(float timeDelay)
    //{
    //    attackEnabled   = false;
    //    enemyMovement.enableMovement = false;

    //    yield return new WaitForSeconds(timeDelay);

    //    attackEnabled   = true;
    //    enemyMovement.enableMovement = true;
    //}

    public void BleedingEffect(float effectLength)
    {
        if (isBleeding)
            return;

        InvokeRepeating("BleedingDamage", effectLength, 1.0f);

        bleedingTimer = 0;
        bleedingLength = effectLength;
        isBleeding = true;
    }

    virtual protected bool CanSeePlayer()
    {
        Vector2 rayDirection = (enemyMovement.PlayerTrans - (Vector2)transform.position).normalized;

        RaycastHit2D rayHit2D = Physics2D.Raycast(transform.position, rayDirection, 20, playerLayer);

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
        //TakeDamage(1);
    }
}
