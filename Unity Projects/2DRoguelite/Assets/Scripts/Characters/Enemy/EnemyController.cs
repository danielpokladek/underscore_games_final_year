using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(EnemyStats))]
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
    public float dropPercentage;
    public GameObject[] gemDrops;
    [SerializeField] private SpriteRenderer weaponSprite;
    public SpriteRenderer enemySprite;

    private float bleedingLength;
    protected bool canAttack     = false;
    protected bool attackEnabled = true;
    protected bool isBleeding    = false;
    private float  bleedingTimer = 0;
    protected float   aimAngle;

    // --- --- ---
    public delegate void OnEnemyDeath();
    public OnEnemyDeath onEnemyDeathCallback;

    protected float _attackDelay;

    // ---
    [HideInInspector] public EnemyMovement   enemyMovement;
    [HideInInspector] public EnemyStats      enemyStats;

    private LevelManager levelManager;

    virtual protected void Start()
    {
        enemyMovement   = GetComponent<EnemyMovement>();
        enemyStats      = GetComponent<EnemyStats>();

        InitiateEnemy();
    }

    private void InitiateEnemy()
    {
        // No need to initiate anything if the object is a dummy,
        //  so simply skip this part and move to the update loop.
        if (isDummy)
            return;

        if (!GameObject.FindGameObjectWithTag("Player"))
            Debug.LogError("Could not find a player, make sure they are tagged and present in the current scene.", gameObject);

        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += NightBuff;

        onEnemyDeathCallback += DeathEffect;

        _attackDelay = enemyStats.characterAttackDelay.GetValue();
        canAttack = true;
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
        if (levelManager.currentState == LevelManager.DayState.Night)
            enemyStats.characterAttackDamage.AddModifier(enemyStats.characterAttackDamage.GetValue() * 2);
        else
            enemyStats.characterAttackDamage.RemoveModifier(enemyStats.characterAttackDamage.GetValue() * 2);
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
        aimAngle          = -1 * Mathf.Atan2(enemyMovement.playerVector.y, enemyMovement.playerVector.x) * Mathf.Rad2Deg;
        armPivot.rotation = Quaternion.AngleAxis(aimAngle, Vector3.back);

        if (weaponSprite != null)
        {
            weaponSprite.sortingOrder = 5 - 1;

            if (aimAngle > 0)
                weaponSprite.sortingOrder = 5 + 1;
        }
    }

    virtual protected bool CanSeePlayer()
    {
        Vector2 rayDirection = enemyMovement.playerVector;

        RaycastHit2D rayHit2D = Physics2D.Raycast(transform.position, rayDirection, 20, playerLayer);

        if (rayHit2D.collider)
        {
            if (rayHit2D.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void BleedingDamage()
    {
        //TakeDamage(1);
    }

    virtual public void DeathEffect()
    {
        StopAllCoroutines();
    }
}