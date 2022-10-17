using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveInput;
    [SerializeField] private float moveSpeed;

    [SerializeField] private int airJumpsAvailable, maxAirJumps;

    [SerializeField] private bool onGround;

    [SerializeField] private float jumpForce;

    [SerializeField] private bool isGrounded;

    [Header("Components")]

    [SerializeField]
    private Rigidbody2D rb;

    private void FixedUpdate() {
        Move();
        isGrounded = GetComponent<GroundCheck>().IsGrounded();
    }
    private void Move() {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnMoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
    }

    public void OnJumpInput(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            if(GetComponent<GroundCheck>().IsGrounded()) {
                Jump();
            } else if(airJumpsAvailable > 0) {
                airJumpsAvailable--;
                Jump();
            }
        }
    }

    public void OnCollisionEnter2D() {
        if (GetComponent<GroundCheck>().IsGrounded()) {
                airJumpsAvailable = maxAirJumps;
        }
    }
}
