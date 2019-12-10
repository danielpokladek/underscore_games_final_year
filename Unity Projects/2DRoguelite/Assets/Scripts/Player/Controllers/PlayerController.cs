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

    [SerializeField] protected GameObject minimapThing;
    
    // --- STUFF FOR ABILITIES ---
    [HideInInspector] public float projectileSizeMultiplier = 1;
    
    // --- ACCESSED BY OTHERS ---
    [HideInInspector] public PlayerStats playerStats;
    
    // ---------------------------
    protected Rigidbody2D playerRB;
    protected Vector2     playerInput;
    protected Camera      playerCamera;

    private Animator playerAnim;
    private bool facingRight;

    // -----------------------------
    protected Vector2 mousePosition = new Vector2(0, 0);
    protected Vector2 mouseVector   = new Vector2(0, 0);
    protected float   armAngle;
    protected bool    allowMovement;

    // ---
    protected bool      canMove;
    private int       playerDirection;
    
    // -------------------------
    protected float currentHealth;
    protected float currentDamage;
    protected bool  playerAlive = true;

    // --- MANAGERS --- //
    protected GameUIManager gameUIManager;
    private LineRenderer lineRenderer;

    public delegate void OnGUIUpdate();
    public OnGUIUpdate onGUIUpdateCallback;

    public delegate void OnPrimAttack();
    public OnPrimAttack onPrimAttackCallback;

    public delegate void OnInteract();
    public OnInteract onInteractCallback;

    public delegate void OnItemInteract();
    public OnItemInteract onItemInteractCallback;

    virtual protected void Start()
    {
        InitiatePlayer();

        StartCoroutine(Init());
    }

    private void InitiatePlayer()
    {
        playerRB      = GetComponent<Rigidbody2D>();
        playerAnim    = GetComponent<Animator>();
        playerStats   = GetComponent<PlayerStats>();
        playerCamera  = Camera.main;

        currentDamage = playerDamage;
        currentHealth = playerHealth;
        playerAlive   = true;
        CanMove       = true;

        gameUIManager = GameUIManager.currentInstance;
        
        minimapThing.SetActive(false);

        // ---
        allowMovement = true;
    }

    virtual protected void Update()
    {
        if (playerAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (onInteractCallback != null)
                    onInteractCallback.Invoke();
            }

            GetMouseInput();
            DebugInputs();
            
            if (Input.GetKeyDown(KeyCode.K))
                playerStats.TakeDamage(10);
            
            // Temp minimap thing
            if (LevelManager.instance.currentState == LevelManager.DayState.Night)
            {               
                minimapThing.SetActive(true);

                Vector3 diff = GameManager.current.bossPortal.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                minimapThing.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }
            else
            {
                minimapThing.SetActive(false);
            }
        }
    }

    virtual protected void FixedUpdate()
    {
        if (playerAlive)
        {
            if (canMove)
            {
                PlayerMovement();
                MoveCharacter(playerInput);
                PlayerAim();
            }


            playerAnim.SetFloat("VMovement", mouseVector.y);
            playerAnim.SetFloat("moveMagnitude", playerInput.magnitude);
        }
    }

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

    public void AddCurrency(int currencyAmount)
    {
        GameManager.current.PlayerCurrency += currencyAmount;
        Debug.Log(GameManager.current.PlayerCurrency);
    }
    #endregion

    #region Getters/Setters
    public bool CanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    public bool isHealed()
    {
        if (currentHealth == playerHealth)
        {
            Debug.Log("You are fully healed!");
            return true;
        }

        return false;
    }

    public float GetCurrentHealth
    {
        get { return currentHealth; }
    }

    public float GetMaxHealth { get { return playerHealth; } }
    public void SetMaxHealth(float value)
    { 
        playerHealth += value;
        
        onGUIUpdateCallback.Invoke();
        
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
            if (Input.GetKeyDown(KeyCode.P))
                playerStats.HealCharacter(10);

            if (Input.GetKeyDown(KeyCode.O))
                playerStats.TakeDamage(10);
        }
    }

    private IEnumerator Init()
    {
        yield return new WaitForFixedUpdate();
        
        if (onGUIUpdateCallback != null)
            onGUIUpdateCallback.Invoke();
    }
}
    
