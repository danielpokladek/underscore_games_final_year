using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public PlayerHearts playerHearts;

    [Header("Character Settings")]
    [Tooltip("This is container for the powerups, this should be a empty gameobject placed on the player." +
        "This ensures a clearner hierarchy, and makes it so the objects follow player." +
        "This is only requried for powerups that spawn in world, for example energy balls.")]
    public Transform powerUpContainer;
    public Transform powerupIcons;
    [Tooltip("This is the animator that controls the attack animations." +
        "This animator is most likely placed on the object used as a weapon/fists.")]
    public Animator attackAnim;
    public SpriteRenderer playerSprite;
    public GameObject minimapThing;
    public ParticleSystem abilityOne, abilityTwo, abilityThree;
    public Transform itemPickup;

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

    /* ITEM CALLBACKS */
    public delegate void OnTakeDamage();
    public OnTakeDamage onTakeDamageCallback;

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
        GameManager.current.playerRef = gameObject;

        playerAnim          = GetComponent<Animator>();
        playerStats         = GetComponent<PlayerStats>();
        playerInteractions  = GetComponent<PlayerInteractions>();
        playerMovement      = GetComponent<PlayerMovement>();
        playerCamera        = Camera.main;

        gameManager   = GameManager.current;
        gameUIManager = GameUIManager.currentInstance;

        InitiatePlayer();
        UIManager.current.PlayerSpawned(this);
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

        if (gameManager.loadStats)
        {
            Debug.Log("Load playerStats");

            SaveManager.current.Load();
            playerStats.Init();
        }
        else if (!gameManager.loadStats)
        {
            Debug.Log("Don't load player stats");

            playerStats.Init();
            gameManager.loadStats = true;
        }
    }

    protected bool a1 = false;
    protected bool a2 = false;
    protected bool a3 = false;

    virtual protected void Update()
    {
        DebugInputs();

        if (!playerAlive)
            return;

        #region Cooldowns (only look here if you dare)
        if (_abilityOneCooldown <= playerStats.abilityOneCooldown.GetValue())
        {
            _abilityOneCooldown += Time.deltaTime;
            UIManager.current.updateUICallback.Invoke();

            a1 = false;
        }
        else if (_abilityOneCooldown >= playerStats.abilityOneCooldown.GetValue() && !a1)
        {
            //abilityOne.Stop();
            //abilityOne.Play();

            a1 = true;
        }

        if (_abilityTwoCooldown <= playerStats.abilityTwoCooldown.GetValue())
        {
            _abilityTwoCooldown += Time.deltaTime;
            UIManager.current.updateUICallback.Invoke();

            a2 = false;
        }
        else if (_abilityTwoCooldown >= playerStats.abilityTwoCooldown.GetValue() && !a2)
        {
            abilityTwo.Stop();
            abilityTwo.Play();

            a2 = true;
        }

        if (_abilityThreeCooldown <= playerStats.abilityThreeCooldown.GetValue())
        {
            _abilityThreeCooldown += Time.deltaTime;
            UIManager.current.updateUICallback.Invoke();

            a3 = false;
        }
        else if (_abilityThreeCooldown >= playerStats.abilityThreeCooldown.GetValue() && !a3)
        {
            abilityThree.Stop();
            abilityThree.Play();

            a3 = true;
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onInteractCallback != null)
                onInteractCallback.Invoke();
        }
    }

    public void ItemPickedUp(ShopPlayerModifier item)
    {
        StartCoroutine(ItemCoroutine(item));
    }

    private IEnumerator ItemCoroutine(ShopPlayerModifier item)
    {
        playerMovement.EnableMovement = false;
        playerAnim.SetBool("itemPose", true);

        item.transform.SetParent(itemPickup);
        item.transform.localPosition = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(2.0f);

        item.AddEffect(this);

        Destroy(item.gameObject);
        playerAnim.SetBool("itemPose", false);
        playerMovement.EnableMovement = true;
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

            //dirX += 5.0f;
            //dirY += 5.0f;

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
            if (Input.GetKeyDown(KeyCode.G))
                SaveManager.current.Save();

            if (Input.GetKeyDown(KeyCode.H))
                SaveManager.current.Load();

            if (Input.GetKeyDown(KeyCode.P))
                playerStats.HealCharacter(10);

            if (Input.GetKeyDown(KeyCode.O))
                playerStats.TakeDamage(10);

            if (Input.GetKeyDown(KeyCode.L))
                playerStats.godMode = !playerStats.godMode;

            if (Input.GetKeyDown(KeyCode.I))
                LevelManager.instance.AddSoul();

            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (SceneManager.GetActiveScene().buildIndex + 2 < SceneManager.sceneCountInBuildSettings)
                    gameManager.LoadScene(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex + 1);
                else
                    gameManager.LoadScene(SceneManager.GetActiveScene().buildIndex, (int)SceneIndexes.MAIN);
            }

            if (Input.GetKeyDown(KeyCode.R) && !playerAlive)
                gameManager.LoadScene(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex);

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}