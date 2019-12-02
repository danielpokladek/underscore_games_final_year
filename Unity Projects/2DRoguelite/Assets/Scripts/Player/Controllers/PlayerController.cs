using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("Speed at which the player will move")]
    [SerializeField] protected float playerMoveSpeed = 8.0f;

    [Tooltip("Player's maximum health points.")]
    [SerializeField] protected float playerHealth = 20;
    
    [Tooltip("Damage that the player will deal to the enemies, later this will be determined by the weapon.")]
    [SerializeField] protected float playerDamage;

    [Tooltip("This is player's 'arm' which will be used to aiming, shooting, etc. It rotates towards the mouse.")]
    [SerializeField] protected GameObject playerArm;

    [SerializeField] protected float attackDelay;

    // --- STUFF FOR ABILITIES ---
    [HideInInspector] public float projectileSizeMultiplier = 1;
    
    // ---------------------------
    protected Rigidbody2D playerRB;
    protected Vector2     playerInput;
    protected Camera      playerCamera;

    // -----------------------------
    protected Vector2 mousePosition = new Vector2(0, 0);
    protected Vector2 mouseVector   = new Vector2(0, 0);
    protected float   armAngle;
    protected bool    allowMovement;

    // ---
    private bool      canMove;
    private int       playerDirection;
    
    // -------------------------
    protected float currentHealth;
    protected float currentDamage;
    protected bool  playerAlive = true;

    // --- MANAGERS --- //
    protected GameUIManager gameUIManager;

    public delegate void OnGUIChange();
    public OnGUIChange onGUIChangeCallback;

    public delegate void OnPrimAttack();
    public OnPrimAttack onPrimAttackCallback;

    virtual public void Start()
    {
        InitiatePlayer();

        StartCoroutine(Init());
    }

    private void InitiatePlayer()
    {
        playerRB      = GetComponent<Rigidbody2D>();
        playerCamera  = Camera.main;

        currentDamage = playerDamage;
        currentHealth = playerHealth;
        playerAlive   = true;
        CanMove       = true;

        gameUIManager = GameUIManager.currentInstance;

        // ---
        allowMovement = true;
    }

    virtual protected void Update()
    {
        if (playerAlive)
        {
            GetMouseInput();
            DebugInputs();
        }
    }

    virtual protected void FixedUpdate()
    {
        if (playerAlive)
        {
            PlayerMovement();
            MoveCharacter(playerInput);
            PlayerAim();
        }
    }

    #region External Calls
    /// <summary>
    /// Temporarily disables player's movement and adds force to the Rigidbody2D in specified direcion and with specified force.
    /// </summary>
    /// <param name="forceDirection">Vector2 direction in which the force should be applied on player.</param>
    /// <param name="forceStrength">Amount of force that should be applied to the player.</param>
    /// <param name="moveDelay">Determines how long player's movement will be disabled for.</param>
    public IEnumerator AddForce(Vector2 forceDirection, float forceStrength, float moveDelay)
    {
        canMove = false;

        playerRB.AddForce(forceDirection * forceStrength);

        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    /// <summary>
    /// Heals player (adds health), by the amount specified in the parameter.
    /// </summary>
    /// <param name="healAmount">Amount of health points that will be added to the player's health.</param>
    public void HealPlayer(float healAmount)
    {
        // If player's health will be higher than max amount, set health to max.
        if ((currentHealth + healAmount) > playerHealth)
        {
            currentHealth = playerHealth;
            return;
        }

        currentHealth += healAmount;
        onGUIChangeCallback.Invoke();
    }

    /// <summary>
    /// Damages player (removes health), by the amount specified in the parameter.
    /// </summary>
    /// <param name="damageAmount">Amount of health points that will be deducted from the player.</param>
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Check player health
        if (currentHealth <= 0)
        {
            Debug.Log("Player is det. Try again?");
            playerAlive = false;
            this.gameObject.SetActive(false);
        }

        onGUIChangeCallback.Invoke();
    }
    #endregion

    private void PlayerMovement()
    {
        if (allowMovement)
        {
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.y = Input.GetAxisRaw("Vertical");
        }
    }

    private void GetMouseInput()
    {
        if (playerCamera)
            mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        mouseVector = (mousePosition - (Vector2) transform.position).normalized;
    }

    private void MoveCharacter(Vector2 input)
    {
        if (canMove)
            playerRB.MovePosition((Vector2) transform.position + (input * playerMoveSpeed * Time.deltaTime));
    }

    virtual protected void PlayerAim()
    {
        armAngle                     = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArm.transform.rotation = Quaternion.AngleAxis(armAngle, Vector3.back);
    }

    #region Getters/Setters
    public bool CanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    public float GetCurrentHealth
    {
        get { return currentHealth; }
    }

    public float GetMaxHealth { get { return playerHealth; } }
    public void SetMaxHealth(float value)
    { 
        playerHealth += value;
        
        onGUIChangeCallback.Invoke();
        
        if (currentHealth > playerHealth)
            currentHealth = playerHealth;
    }

    public float DamageAmount { get { return currentDamage; }   set { currentDamage = value; } }
    public float AttackDelay  { get { return attackDelay; }     set { attackDelay = value; } }
    public float MovementSpd  { get { return playerMoveSpeed; } set { playerMoveSpeed = value; } }
    #endregion

    #region Attack/Dodge/Skill Declarations
    virtual protected void PrimAttack()   { /* Only declaration for the function, needs to be defined per character. */
                                            onPrimAttackCallback.Invoke(); }

    virtual protected void SecAttack()    { /* Only declaration for the function, needs to be defined per character. */
                                            throw new System.NotImplementedException(); }

    virtual protected IEnumerator Dodge() { /* Only declaration for the function, needs to be defined per character. */
                                            throw new System.NotImplementedException(); }
    #endregion

    private void DebugInputs()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.M))
                TakeDamage(10);

            if (Input.GetKeyDown(KeyCode.N))
                HealPlayer(10);
        }
    }

    private IEnumerator Init()
    {
        yield return new WaitForFixedUpdate();
        onGUIChangeCallback.Invoke();
    }
}
    
