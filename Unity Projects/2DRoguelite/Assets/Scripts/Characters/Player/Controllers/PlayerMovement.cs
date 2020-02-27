using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Movement & Aim Settings")]
    [Tooltip("Disable this toggle to stop the player from moving.")]
    [SerializeField] private bool           enableMovement  = true;
    [SerializeField] private float          dashDuration    = 0.2f;
    [SerializeField] private Transform      playerArmPivot;
    [SerializeField] private SpriteRenderer armSprite;

    // --- --- ---
    private Rigidbody2D playerRB;
    private Vector2 playerInput;
    private Vector2 mousePosition;
    private Vector2 mouseVector;
    private Vector2 moveVector;
    private Vector2 attackDirection;
    private float armAngle;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetMouseInput();
        ArmDrawLayer();
    }

    void FixedUpdate()
    {
        GetPlayerInput();
        MoveCharacter(playerInput);
        PlayerAim();

        playerController.playerAnim.SetFloat("HMovement", mouseVector.x);
        playerController.playerAnim.SetFloat("VMovement", mouseVector.y);
        playerController.playerAnim.SetFloat("moveMagnitude", playerInput.magnitude);
    }

    private void GetPlayerInput()
    {
        if (enableMovement)
        {
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.y = Input.GetAxisRaw("Vertical");
        }
    }

    private void MoveCharacter(Vector2 _playerInput)
    {
        moveVector = new Vector2(_playerInput.x, _playerInput.y);
        moveVector.Normalize();
        moveVector *= playerController.playerStats.characterSpeed.GetValue();

        playerRB.MovePosition((Vector2)transform.position + (moveVector * Time.deltaTime));
    }

    private void GetMouseInput()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseVector = (mousePosition - (Vector2)transform.position).normalized;
    }

    private void PlayerAim()
    {
        armAngle = -1 * Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        playerArmPivot.rotation = Quaternion.AngleAxis(armAngle, Vector3.back);

        attackDirection = mousePosition - playerRB.position;
    }

    private void ArmDrawLayer()
    {
        armSprite.sortingOrder = 5 - 1;

        if (armAngle > 0)
            armSprite.sortingOrder = 5 + 1;
    }

    #region External
    public bool EnableMovement { get; set; }
    public Vector2 GetAttackDirection { get; }
    public Vector2 GetMousePosition() { return mousePosition; }

    public IEnumerator PlayerDash()
    {
        enableMovement = false;
        playerController.playerStats.characterSpeed.AddModifier(15);
        playerController.playerStats.canTakeDamage = false;

        yield return new WaitForSeconds(dashDuration);

        playerController.playerStats.canTakeDamage = true;
        playerController.playerStats.characterSpeed.RemoveModifier(15);
        enableMovement = true;
    }

    public IEnumerator AddForce(Vector2 forceDirection, float forceStrength, float moveDelay)
    {
        enableMovement = false;

        playerRB.AddForce(forceDirection * forceStrength);
        yield return new WaitForSeconds(moveDelay);

        enableMovement = true;
    }
    #endregion
}
