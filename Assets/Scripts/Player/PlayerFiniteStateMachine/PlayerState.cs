using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for all of our states
public class PlayerState
{
    protected Player player; //reference to the player
    protected PlayerStateMachine stateMachine; //reference to the player state machine
    protected PlayerData playerData; //reference to player data, scriptable object that controls all the variable the player has
    
    protected float startTime; //gets set every time we enter a state, used to provide a reference for how long we've been in a specific state
    
    private string animBoolName; //each state has a string assigned on creation, those are used to tell the animator what animations should be playing

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    //every state needs to have an enter and exit function, and also an update(LogicUpdate) and fixedUpdate(PhysicsUpdate) function

    //called when entering a specific state
    public virtual void Enter() {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
    }

    //called when leaving the state
    public virtual void Exit() {
        player.Anim.SetBool(animBoolName, false);
    }

    //called every frame
    public virtual void LogicUpdate() {
        
    }

    //called every Fixed update
    public virtual void PhysicsUpdate() {
        DoChecks();
    }

    //called from PhysicsUpdate and from Enter, put stuff here like looking for ground or walls
    public virtual void DoChecks(){
        
    }
 }
