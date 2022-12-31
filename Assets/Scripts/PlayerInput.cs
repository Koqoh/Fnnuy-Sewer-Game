using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


//Brendan can you please comment this script my brain is small and I have forgotten how anything works

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool[] groundLayerTrue;

    [SerializeField] private float moveInput;
    [SerializeField] private float acceleration; //move speed
    [SerializeField] private float decceleration; //??
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float moveCap;

    [SerializeField] private float lastJumpPressed;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float lastGrounded;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool touchingGround;
    [SerializeField] private bool isGrounded;
    [SerializeField] Animator animator;
    [SerializeField] GameObject violin;

    private bool HasBufferedJump => lastJumpPressed + jumpBuffer > Time.time;
    private bool HasCoyoteTime => lastGrounded + coyoteTime > Time.time;

    [Header("Components")]

    [SerializeField]
    private Rigidbody2D rb;

    private void FixedUpdate() {
        FireViolinRay();
        Move();
        if (touchingGround) isGrounded = GetComponent<GroundCheck>().IsGrounded();
        if (isGrounded) {
            lastGrounded = Time.time;
        }
        if (HasBufferedJump && HasCoyoteTime) Jump();
    }
    private void Move() {
        velocity.x = moveInput * acceleration * Time.fixedDeltaTime;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        animator.SetFloat("Speed",rb.velocity.x);
    }

    private void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    private void FireViolinRay() {
        Debug.Log("ray casted");
        Physics2D.Raycast(violin.transform.position, Mouse.current.position.ReadValue());
    }


    //afaik how these Callback contexts work is
    //Player inputs, moveinput is stored from that, and the movement script is also called after
    //perhaps

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
    public void OnFireViolinRay(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            Debug.Log("Ray cast recieved");
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

    //cordon for animation methods
    void Face(int direction) {
        transform.localRotation = new Quaternion(0,1,0,direction*90);
    }
    //cordon over
}
