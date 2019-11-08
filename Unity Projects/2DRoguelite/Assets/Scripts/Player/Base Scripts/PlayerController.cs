using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Settings")] [SerializeField]
    protected float moveSpeed = 8.0f;

    [SerializeField] protected float playerHealth = 20;
    
    [Tooltip("Damage that the player will deal to the enemies, later this will be determined by the weapon.")]
    [SerializeField] protected float playerDamage;
    
    // ---------------------------
    protected Rigidbody2D playerRB;
    protected Vector2     playerInput;
    protected Camera      playerCamera;

    // -----------------------------
    protected Vector2 mousePosition = new Vector2(0, 0);
    protected Vector2 mouseVector   = new Vector2(0, 0);
    protected bool    canMove;
    
    // -------------------------
    protected float currentHealth;
    protected bool  playerAlive = true;

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
        canMove       = true;
    }

    virtual protected void Update()
    {
        if (playerAlive)
        {
            GetMouseInput();
        }
    }

    virtual public void FixedUpdate()
    {
        if (playerAlive)
        {
            PlayerMovement();
            MoveCharacter(playerInput);
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

    #region Getters/Setters
    public bool CanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    public float GetHealth
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
}
    
