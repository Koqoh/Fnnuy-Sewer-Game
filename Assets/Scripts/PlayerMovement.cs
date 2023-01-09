using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


//k  so any important logic from here should be moved to the player script and various parts of the statemachine  please and thank you

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool[] groundLayerTrue;

    [SerializeField] public float moveInput { get; private set; }
    [SerializeField] private float acceleration; //move speed
    [SerializeField] private float decceleration; //??
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float moveCap;

    [SerializeField] public static float lastJumpPressed;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float lastGrounded;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool touchingGround;
    [SerializeField] private bool isGrounded = false;
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
        Physics2D.Raycast(violin.transform.position, Mouse.current.position.ReadValue());
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
