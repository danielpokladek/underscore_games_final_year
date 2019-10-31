using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 8.0f;
    [SerializeField] protected float maxHealth = 20;
    [Tooltip("Delay between player's attacks, for example this is the delay between ranger's projectiles." +
             "Increasing this value, will mean, that players will wait longer before next attack is performed." +
             "Leave this value to zero, to have no delay.")]
    [SerializeField] protected float delayLength;
    
    protected Rigidbody2D playerRB;
    protected Vector2 playerInput;
    protected float _delayAttack;

    // Used by characters
    protected Vector2 mousePosition    = new Vector2(0, 0);
    protected Vector2 mouseVector      = new Vector2(0,0);

    private float playerHealth;
    private bool playerAlive = true;

    virtual public void Start()
    {
        InitiatePlayer();
    }

    private void InitiatePlayer()
    {
        playerRB = GetComponent<Rigidbody2D>();

        playerHealth = maxHealth;
        playerAlive = true;

        _delayAttack = delayLength;
    }

    virtual public void Update()
    {
        if (playerAlive)
        {
            PlayerMovement();
            GetMouseInput();

            if (Input.GetButton("LMB"))
            {
                if (_delayAttack > delayLength)
                    PrimAttack();
            }
            
            if (Input.GetButton("RMB"))
                SecAttack();
            
            if (Input.GetButton("Dodge"))
                Dodge();
        }
        
        
        if (delayLength == 0)
            _delayAttack = delayLength;
        
        if (_delayAttack <= delayLength)
            _delayAttack += Time.deltaTime;
    }

    virtual public void FixedUpdate()
    {
        MoveCharacter(playerInput);
    }

    public void TakeDamage(float _dmgAmnt)
    {
        playerHealth = playerHealth - _dmgAmnt;

        // Check player health
        if (playerHealth <= 0)
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
        if (Camera.main)
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        mouseVector = (mousePosition - (Vector2)transform.position).normalized;
    }

    private void MoveCharacter(Vector2 input)
    {
        playerRB.MovePosition((Vector2)transform.position + (input * moveSpeed * Time.deltaTime));
    }
    
    // Only the definitions for the attacks, need to be updated per character (in their own scripts).
    // Also reset the delay when shooting., might as well be done here to keep inherits clean.
    virtual protected void PrimAttack() { _delayAttack = 0; }

    virtual protected void SecAttack() { /* Secondary Attack for the character. */ }

    virtual protected void Dodge() { /* Dodge Ability for the character. */ }
}
