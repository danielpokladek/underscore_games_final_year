using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteractions))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Character UI")]
    public Sprite characterPortrait;
    public Sprite skillOne, skillTwo, skillThree;
    [Tooltip("This is player's 'arm' which will be used to aiming, shooting, etc. It rotates towards the mouse.")]
    [SerializeField] protected GameObject playerArm;

    [Header("Character Settings")]
    [Tooltip("This is container for the powerups, this should be a empty gameobject placed on the player." +
        "This ensures a clearner hierarchy, and makes it so the objects follow player." +
        "This is only requried for powerups that spawn in world, for example energy balls.")]
    public Transform powerUpContainer;
    [Tooltip("This is the animator that controls the attack animations." +
        "This animator is most likely placed on the object used as a weapon/fists.")]
    public Animator attackAnim;
    public SpriteRenderer playerSprite;

    // --- --- ---
    [HideInInspector] public float  projectileSizeMultiplier = 1; 
    [HideInInspector] public bool   foundPortal = false;

    // --- --- ---
    protected Camera    playerCamera;

    // --- --- ---
    protected float _attackDelay;
    protected float _abilityOneCooldown;
    protected float _abilityTwoCooldown;
    protected float _abilityThreeCooldown;
    protected bool playerAlive { get; set; }

    //[SerializeField] protected GameObject portalIndicator;

    // --- Accessed by other classes ---
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerInteractions playerInteractions;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public Animator playerAnim;

    // --- MANAGERS --- //
    protected GameUIManager gameUIManager;
    private LineRenderer lineRenderer;

    public delegate void OnGUIUpdate();
    public OnGUIUpdate onUIUpdateCallback;

    public delegate void OnPrimAttack();
    public OnPrimAttack onPrimAttackCallback;

    public delegate void OnInteract();
    public OnInteract onInteractCallback;

    public delegate void OnItemInteract();
    public OnItemInteract onItemInteractCallback;

    virtual protected void Start()
    {
        playerAnim          = GetComponent<Animator>();
        playerStats         = GetComponent<PlayerStats>();
        playerInteractions  = GetComponent<PlayerInteractions>();
        playerMovement      = GetComponent<PlayerMovement>();
        playerCamera        = Camera.main;

        InitiatePlayer();
        UIManager.current.PlayerSpawned();
    }

    private void InitiatePlayer()
    {
        playerAlive   = true;

        gameUIManager = GameUIManager.currentInstance;

        GameManager.current.playerRef = gameObject;
        
       // minimapThing.SetActive(false);
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

            DebugInputs();
            
            //if (LevelManager.instance.currentState == LevelManager.DayState.Night ||
            //    LevelManager.instance.currentState == LevelManager.DayState.Midnight)
            //{
            //    portalIndicator.SetActive(true);

            //    if (foundPortal)
            //    {
            //        Vector3 diff = GameManager.current.bossPortalRef.transform.position - transform.position;
            //        diff.Normalize();

            //        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            //        portalIndicator.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            //    }
            //    else
            //    {
            //        minimapThing.transform.RotateAround(portalIndicator.transform.position, new Vector3(0, 0, 1), 10);
            //    }
            //}
            //else
            //{
            //    minimapThing.SetActive(false);
            //}
        }
    }

    #region Attack/Dodge/Skill Declarations
    virtual protected void PrimAttack()   { /* Only declaration for the function, needs to be defined per character. */
                                            onPrimAttackCallback.Invoke(); }

    virtual protected void SecAttack()    { /* Only declaration for the function, needs to be defined per character. */
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

            if (Input.GetKeyDown(KeyCode.L))
                playerStats.godMode = !playerStats.godMode;

            if (Input.GetKeyDown(KeyCode.I))
                LevelManager.instance.AddSoul();
        }
    }
}