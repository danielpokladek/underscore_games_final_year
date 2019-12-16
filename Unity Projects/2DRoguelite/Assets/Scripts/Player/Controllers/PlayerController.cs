using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Settings")]   
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

    // --- Animators ---
                     protected Animator playerAnim;
    [SerializeField] protected Animator attackAnim;

    // -----------------------------
    protected Vector2 mousePosition = new Vector2(0, 0);
    protected Vector2 mouseVector   = new Vector2(0, 0);
    protected float   armAngle;
    protected bool    allowMovement;

    // ---
    protected bool    canMove;
    private int       playerDirection;
    
    // -------------------------
    protected bool playerAlive = true;
    [HideInInspector] public bool foundPortal = false;

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
            
            /*
             * TEMPORARY BOSS PORTAL SOLUTIONS
             * IDEALLY SHOULD BE CHANGED IN FUTURE AT SOME POINT
             */
            if (LevelManager.instance.currentState == LevelManager.DayState.Night ||
                LevelManager.instance.currentState == LevelManager.DayState.Midnight)
            {
                minimapThing.SetActive(true);
                
                if (foundPortal)
                {
                    Vector3 diff = GameManager.current.bossPortalReference.transform.position - transform.position;
                    diff.Normalize();

                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    minimapThing.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                }
                else
                {
                    minimapThing.transform.RotateAround(minimapThing.transform.position, new Vector3(0, 0, 1), 10);
                }
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

            
            playerAnim.SetFloat("HMovement", mouseVector.x);
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

    private void MoveCharacter(Vector2 _playerInput)
    {
        if (canMove)
        {
            Vector2 moveVector = new Vector2(_playerInput.x, _playerInput.y);
            moveVector.Normalize();
            moveVector *= playerStats.characterSpeed.GetValue();
            
            playerRB.MovePosition((Vector2) transform.position +
                                  (moveVector * Time.deltaTime));
        }
    }

    virtual protected void PlayerAim()
    {
        armAngle                     = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArm.transform.rotation = Quaternion.AngleAxis(armAngle, Vector3.back);
    }

    #region External Calls
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
    
    public float AttackDelay  { get { return attackDelay; }     set { attackDelay = value; } }
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