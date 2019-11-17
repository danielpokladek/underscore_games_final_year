using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Settings")]
    [Tooltip("Speed at which the player will move")]
    [SerializeField] protected float moveSpeed = 8.0f;

    [Tooltip("Player's maximum health points.")]
    [SerializeField] protected float playerHealth = 20;
    
    [Tooltip("Damage that the player will deal to the enemies, later this will be determined by the weapon.")]
    [SerializeField] protected float damageAmount;

    [Tooltip("This is player's 'arm' which will be used to aiming, shooting, etc. It rotates towards the mouse.")]
    [SerializeField] protected GameObject playerArm;
    
    // ---------------------------
    protected Rigidbody2D playerRB;
    protected Vector2     playerInput;
    protected Camera      playerCamera;

    // -----------------------------
    protected Vector2 mousePosition = new Vector2(0, 0);
    protected Vector2 mouseVector   = new Vector2(0, 0);
    protected float   armAngle;

    // ---
    private bool      canMove;
    private int       playerDirection;
    
    // -------------------------
    protected float currentHealth;
    protected bool  playerAlive = true;

    // --- MANAGERS --- //
    protected GameUIManager gameUIManager;

    virtual public void Start()
    {
        InitiatePlayer();
    }

    private void InitiatePlayer()
    {
        playerRB      = GetComponent<Rigidbody2D>();
        playerCamera  = Camera.main;
        
        currentHealth = playerHealth;
        playerAlive   = true;
        CanMove       = true;

        gameUIManager = GameUIManager.currentInstance;
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

    public void TakeDamage(float damageAmount)
    {
        currentHealth = currentHealth - damageAmount;

        // Check player health
        if (currentHealth <= 0)
        {
            Debug.Log("Player is det. Try again?");
            playerAlive = false;
            this.gameObject.SetActive(false);
        }
    }

    public void HealPlayer(float healAmount)
    {
        // If player's health will be higher than max amount, set health to max.
        if ((currentHealth + healAmount) > playerHealth)
        {
            currentHealth = playerHealth;
            return;
        }

        currentHealth += healAmount;
    }

    private void PlayerMovement()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
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
            playerRB.MovePosition((Vector2) transform.position + (input * moveSpeed * Time.deltaTime));
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

    #endregion

    #region PlayerAttacks
    // Only the declarations for the attacks, need to be declared per characters.
    virtual protected void PrimAttack()
    {
        
    }

    virtual protected void SecAttack()
    {
        /* Secondary Attack for the character. */
    }

    virtual protected void Dodge()
    {
        /* Dodge Ability for the character. */
    }
    #endregion

    private void DebugInputs()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.M))
                TakeDamage(10);

            if (Input.GetKeyDown(KeyCode.N))
                HealPlayer(10);

            if (Input.GetKeyDown(KeyCode.Backspace))
                LevelManager.instance.Restart();
        }
    }
}
    
