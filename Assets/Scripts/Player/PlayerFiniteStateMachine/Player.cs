using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player state objects are created here
public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine{ get; private set;}//can be got anywhere, only set here

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    [SerializeField] 
    private PlayerData playerData; //???


    #endregion

    #region Components
    public Animator Anim { get; private set; }

    [SerializeField] Transform meshTransform;

    public PlayerInputHandler InputHandler { get; private set; }

    public Rigidbody2D rb { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField] private Transform groundCheck;
    
    #endregion
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; } //stores movement input
    public int facingDirection { get; private set; }
    
    private Vector2 workspace; //future velocity
    #endregion


    #region Unity Callback Functions
    private void Awake(){
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
    }

    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();

        facingDirection = 1;

        StateMachine.Initialize(IdleState);
    }
    private void Update() { //call logic update for our state
        CurrentVelocity = rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate() { //call physics update for our state
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions

    //for movement
    public void SetVelocityX(float velocity){
        workspace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity){
        workspace.Set(CurrentVelocity.x, velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region Check Functions

    public bool CheckIfGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.WhatIsGround);
    }
    public void CheckIfShouldFlip(int xInput){
        if (xInput != 0 && xInput != facingDirection){
            Flip();
        }
    }



    #endregion

    #region Other Functions
    private void Flip(){
        facingDirection *= -1;
        meshTransform.rotation = Quaternion.Euler(0, 120 * facingDirection, 0);
    }
    #endregion
}
