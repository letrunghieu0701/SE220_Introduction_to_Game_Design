using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultDroidControl : Monster
{
    [Header("Self: ")]
    [SerializeField] protected GameObject rightBound;
    [SerializeField] protected GameObject leftBound;
    [SerializeField] private float restAfterAttackDelay = 2.0f;
    [SerializeField] private float flipError = 2.0f;

    private State state;
    private List<State> states; 
    private bool updated = false;
    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private Vector3 posToGo;

    private void Awake() {
        states = new List<State>();
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                state = new State("attack", 5.0f);
                state.ResetState();
                states.Add(state);
            }
            else if (i == 1)
            {
                state = new State("rest after attack", restAfterAttackDelay);
                state.ResetState();
                states.Add(state);
            }
        }

        stateMachine = new MonsterStateMachine(states);
        _isAnimationFinished = true;
    }

    protected override void Start()
    {
        base.Start();

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
        posToGo = leftBoundPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDead)
        {
            if (playerDetected)
            {
                if (stateMachine.stateOrder == 0)
                {
                    animator.SetBool("walk", false);

                    if (!updated)
                    {
                        _isAnimationFinished = false;
                        _LookAtTarget(player.transform.position);
                        animator.SetTrigger("attack");
                        _StandStill();
                        updated = true;
                    }

                    if (_isAnimationFinished)
                    {
                        stateMachine.SkipState();
                        //_isAnimationFinished = false;
                    }
                }
                else if (stateMachine.stateOrder == 1)
                {
                    animator.SetBool("walk", false);
                    animator.ResetTrigger("attack");
                    updated = false;

                    stateMachine.Run();
                }
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                {
                    _isAnimationFinished = true;
                }
                
                //_isAnimationFinished = false;
                stateMachine.Reset();
                animator.ResetTrigger("attack");
                updated = false;

                if (transform.position.x <= leftBoundPos.x)
                {
                    posToGo = rightBoundPos;
                }
                else if (transform.position.x >= rightBoundPos.x)
                {
                    posToGo = leftBoundPos;
                }

                if (_isAnimationFinished)
                {
                    _CheckPositionAndMoveHorSmooth(posToGo, speed);
                    animator.SetBool("walk", true);
                }
                
            }
        }
    }

    protected override void Flip()
    {
        facingRight = !facingRight;

		Vector3 theScale = transform.localScale;

        if (theScale.x == -1)
        {
            transform.position = new Vector2(transform.position.x + flipError, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - flipError, transform.position.y);
        }

		theScale.x *= -1;
		transform.localScale = theScale;
    }

}
