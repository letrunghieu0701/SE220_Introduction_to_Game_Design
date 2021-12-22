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
    [SerializeField] private float damage;
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    private Animator ani;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake() {
        ani = GetComponent<Animator>();
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void FixedUpdate() {
        if(ani) {
            ani.SetBool("walk", true);
        }
    }

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

        if(movingLeft) {
            if(transform.position.x > leftEdge) {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else Flip();
        } else {
            if(transform.position.x < rightEdge) {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            } else Flip();
        }
    }

    private void Flip() {
        movingLeft = !movingLeft;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            col.GetComponent<Health>().TakeDamage(damage, transform.localScale.x);
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
