using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
basically a variable that holds a reference to our current state, 
a function to initialise that state, 
and a function to change what our current state is
*/
public class PlayerStateMachine
{
    public PlayerState CurrentState {get; private set; } //any other script can get this, but it can only be set from this script // it is capitalised cuz that's the standard syntax fite me
    
    public void Initialize(PlayerState startingState){
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(PlayerState newState){
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
