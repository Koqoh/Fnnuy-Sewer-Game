using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool[] groundLayerTrue;

    [SerializeField] private float moveInput;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float lastJumpPressed;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float lastGrounded;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool touchingGround;
    [SerializeField] private bool isGrounded;

    private bool HasBufferedJump => lastJumpPressed + jumpBuffer > Time.time;
    private bool HasCoyoteTime => lastGrounded + coyoteTime > Time.time;

    [Header("Components")]

    [SerializeField]
    private Rigidbody2D rb;

    private void FixedUpdate() {
        Move();
        if (touchingGround) isGrounded = GetComponent<GroundCheck>().IsGrounded();
        if (isGrounded) {
            lastGrounded = Time.time;
        }
        if (HasBufferedJump && HasCoyoteTime) Jump();
    }
    private void Move() {
        rb.velocity = new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnMoveInput(InputAction.CallbackContext context) {
        Debug.Log("move input recieved");
        if (context.phase is InputActionPhase.Performed or InputActionPhase.Canceled) {
            moveInput = context.ReadValue<float>();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            Debug.Log("jump input recieved");
            lastJumpPressed = Time.time;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (groundLayerTrue[collision.gameObject.layer]) {
            touchingGround = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision) {
        if (groundLayerTrue[collision.gameObject.layer]) {
            touchingGround = false;
            isGrounded = false;
        }
    }

    private void OnValidate() {
        groundLayerTrue = new BitArray(new int[]{groundLayer.value}).Cast<bool>().ToArray();
    }
}
