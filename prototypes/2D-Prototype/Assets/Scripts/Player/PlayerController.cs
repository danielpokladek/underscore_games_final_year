using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 8.0f;

    [Header("Player Settings")]
    [SerializeField] protected int maxHealth = 8;

    protected Rigidbody2D playerRB;
    protected Vector2 playerInput;

    // Used by characters
    protected Vector2 mousePosition;
    protected Vector2 mouseVector;

    private int playerHealth;

    private void Start()
    {
        InitiatePlayer();
    }

    private void InitiatePlayer()
    {
        playerRB = GetComponent<Rigidbody2D>();

        playerHealth = maxHealth;
    }

    public virtual void Update()
    {
        PlayerMovement();
        GetMouseInput();

        //
        // Attacks, skills, etc.
        if (Input.GetButtonDown("LMB"))
            PrimAttack();

        if (Input.GetButtonDown("RMB"))
            SecAttack();

        if (Input.GetButtonDown("Dodge"))
            Dodge();
    }

    public virtual void FixedUpdate()
    {
        MoveCharacter(playerInput);
    }

    private void PlayerMovement()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    private void GetMouseInput()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseVector = (mousePosition - (Vector2)transform.position).normalized;
    }

    private void MoveCharacter(Vector2 input)
    {
        playerRB.MovePosition((Vector2)transform.position + (input * moveSpeed * Time.deltaTime));
    }

    // Primary Attack for the character
    public virtual void PrimAttack()
    {
        Debug.Log("Primary Attack");
    }

    // Secondary Attack for the character
    public virtual void SecAttack()
    {
        Debug.Log("Secondary Attack");
    }

    // Dodge Ability for the character
    public virtual void Dodge()
    {
        Debug.Log("Dodge");
    }
}
