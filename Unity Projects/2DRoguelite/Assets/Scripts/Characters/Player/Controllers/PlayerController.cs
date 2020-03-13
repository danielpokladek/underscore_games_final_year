using System.Collections.Generic;
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
    public Sprite skillOneBack, skillTwoBack, skillThreeBack;
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
    public GameObject minimapThing;

    // --- --- ---
    [HideInInspector] public float  projectileSizeMultiplier = 1; 
    [HideInInspector] public bool   foundPortal = false;
    [HideInInspector] public bool   playerDead  = false;

    // --- --- ---
    protected Camera    playerCamera;

    // --- --- ---
    protected float _attackDelay;
    protected float _abilityOneCooldown;
    protected float _abilityTwoCooldown;
    protected float _abilityThreeCooldown;
    [HideInInspector] public bool playerAlive { get; set; }

    //[SerializeField] protected GameObject portalIndicator;

    // --- Accessed by other classes ---
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerInteractions playerInteractions;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public Animator playerAnim;

    private List<GameObject> powerupBalls;

    // --- MANAGERS --- //
    protected GameUIManager gameUIManager;
    protected GameManager gameManager;
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

        gameManager   = GameManager.current;
        gameUIManager = GameUIManager.currentInstance;

        InitiatePlayer();
        UIManager.current.PlayerSpawned();
    }

    private void InitiatePlayer()
    {
        playerAlive   = true;

        gameUIManager         = GameUIManager.currentInstance;
        gameManager.playerRef = gameObject;
        
        _abilityOneCooldown   = playerStats.abilityOneCooldown.GetValue();
        _abilityTwoCooldown   = playerStats.abilityTwoCooldown.GetValue();
        _abilityThreeCooldown = playerStats.abilityThreeCooldown.GetValue();

        powerupBalls = new List<GameObject>();

        minimapThing.SetActive(false);

        if (UIManager.current.tutorialHud != null)
            Instantiate(UIManager.current.tutorialHud, transform.position, Quaternion.identity);
    }

    virtual protected void Update()
    {
        DebugInputs();

        if (!playerAlive)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onInteractCallback != null)
                onInteractCallback.Invoke();
        }
    }

    public void AddEnergyBall(GameObject energyBall)
    {
        powerupBalls.Add(energyBall);

        float angle = 0;
        float angleStep = (0 - 360) / powerupBalls.Count;

        for (int i = 0; i < powerupBalls.Count; i++)
        {
            powerupBalls[i].transform.position = Vector3.zero;

            float dirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float dirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 moveVector = new Vector3(dirX, dirY, 0.0f);
            Vector2 dir = (moveVector - transform.position).normalized;

            powerupBalls[i].transform.position += new Vector3(dirX, dirY, 0);

            angle += angleStep;
        }
    }

    #region Attack/Dodge/Skill Declarations
    virtual protected void PrimAttack()   { /* Only declaration for the function, needs to be defined per character. */
                                            onPrimAttackCallback.Invoke(); }

    virtual protected void SecAttack()    { /* Only declaration for the function, needs to be defined per character. */
                                            throw new System.NotImplementedException(); }
    #endregion

    public float GetSkillOneCooldown() { return _abilityOneCooldown; }
    public float GetSkillTwoCooldown() { return _abilityTwoCooldown; }
    public float GetSkillThreeCooldown() { return _abilityThreeCooldown; }

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

            if (Input.GetKeyDown(KeyCode.F5))
                LevelManager.instance.LoadBossBattle();

            if (Input.GetKeyDown(KeyCode.R))
                LevelManager.instance.Restart();
        }
    }
}