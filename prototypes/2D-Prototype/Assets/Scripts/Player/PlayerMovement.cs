using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8.0f;

    // Player Movement
    private Rigidbody2D rb;
    private Vector2 playerInput;

    // Camera Rotation
    private Camera cam;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //rb.velocity = playerInput.normalized * moveSpeed; // looks laggy
        rb.MovePosition(rb.position + playerInput * moveSpeed * Time.deltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float playerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        rb.rotation = playerAngle;
    }
}
