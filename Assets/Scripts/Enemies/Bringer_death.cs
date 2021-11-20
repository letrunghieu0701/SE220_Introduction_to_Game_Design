using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_death : MonoBehaviour
{
    private enum State{
        Idle,
        Attack,
        Hurt,
        Walk,
        Skill,
        Dead,
    }

    private State currentState;
    private bool groundDetected, wallDetected;
    [SerializeField] private Transform groundCheck, wallCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance, wallCheckDistance;

    private void Update(){
        switch(currentState){
            case State.Idle: UpdateIdleState();
                break;
            case State.Attack: UpdateAttackState();
                break;
            case State.Hurt: UpdateHurtState();
                break;
            case State.Walk: UpdateWalkState();
                break;
            case State.Skill: UpdateSkillState();
                break;
            case State.Dead: UpdateDeadState();
                break;
        }
    }

    //====================Idle state========================

    private void EnterIdleState(){

    }

    private void UpdateIdleState(){

    }

    private void ExitIdleState(){

    }

    //====================Attack state========================

    private void EnterAttackState(){

    }

    private void UpdateAttackState(){

    }

    private void ExitAttackState(){

    }

    //====================Hurt state========================

    private void EnterHurtState(){

    }

    private void UpdateHurtState(){

    }

    private void ExitHurtState(){

    }

    //====================Walking state========================

    private void EnterWalkState(){

    }

    private void UpdateWalkState(){
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void ExitWalkState(){

    }

    //====================Skill state========================

    private void EnterSkillState(){

    }

    private void UpdateSkillState(){

    }

    private void ExitSkillState(){

    }

    //====================Dead state========================

    private void EnterDeadState(){

    }

    private void UpdateDeadState(){

    }

    private void ExitDeadState(){

    }

    //====================Other functions========================

    private void SwitchState(State state){
        switch(currentState){
            case State.Idle: ExitIdleState();
                break;
            case State.Attack: ExitAttackState();
                break;
            case State.Hurt: ExitHurtState();
                break;
            case State.Walk: ExitWalkState();
                break;
            case State.Skill: ExitSkillState();
                break;
            case State.Dead: ExitDeadState();
                break;
        }

        switch(state){
            case State.Idle: EnterIdleState();
                break;
            case State.Attack: EnterAttackState();
                break;
            case State.Hurt: EnterHurtState();
                break;
            case State.Walk: EnterWalkState();
                break;
            case State.Skill: EnterSkillState();
                break;
            case State.Dead: EnterDeadState();
                break;
        }

        currentState = state;
    }
}
