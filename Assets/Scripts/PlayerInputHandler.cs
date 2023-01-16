using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


//Brendan can you please comment this script my brain is small and I have forgotten how anything works

public class PlayerInputHandler : MonoBehaviour

{
    public Vector2 RawMoveInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; } //please don't fuck with y we need it if we want air strafing with the grapple
    public bool JumpInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.02f;

    private float jumpInputStartTime;

    private void Update() {
        CheckJumpInputHoldTime();
    }

    //afaik how these Callback contexts work is
    //Player inputs, moveinput is stored from that, and the movement script is also called after
    //perhaps

    public void OnMoveInput(InputAction.CallbackContext context) {
        //Debug.Log("move input recieved");
        if (context.phase is InputActionPhase.Performed or InputActionPhase.Canceled) {
            RawMoveInput = context.ReadValue<Vector2>();

            NormalizedInputX = (int)(RawMoveInput * Vector2.right).normalized.x;
            NormalizedInputY = (int)(RawMoveInput * Vector2.up).normalized.y;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            //Debug.Log("jump input recieved");
            
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }

    public void UseJumpInput() => JumpInput = false; 

    private void CheckJumpInputHoldTime() {
        if(Time.time >= jumpInputStartTime + inputHoldTime){
            JumpInput = false;
        }
    }

    public void OnFireViolinRay(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            //Debug.Log("Ray cast recieved");
        }
    }
}
