using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDroidControl : Monster
{
    [Header("Self: ")]
    [SerializeField] protected GameObject rightBound;
    [SerializeField] protected GameObject leftBound;
    [SerializeField] private float shieldTime = 4.0f;
    [SerializeField] private float restAfterShield = 1.0f;
    [SerializeField] private float restAfterShock = 1.0f;
    [SerializeField] private bool canClimb = false;
    [SerializeField] private bool flipped = false;

    private State state;
    private List<State> states; 
    private bool updated = false;
    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private Vector3 posToGo;

    private void Awake() {
        states = new List<State>();
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                state = new State("shield", shieldTime);
                state.ResetState();
                states.Add(state);
            }
            else if (i == 1)
            {
                state = new State("rest after shield", restAfterShield);
                state.ResetState();
                states.Add(state);
            }
            else if (i == 2)
            {
                state = new State("shock", 10.0f);
                state.ResetState();
                states.Add(state);
            }
            else if (i == 3)
            {
                state = new State("rest after shock", restAfterShock);
                state.ResetState();
                states.Add(state);
            }
        }

        stateMachine = new MonsterStateMachine(states);
        _isAnimationFinished = true;
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
        posToGo = leftBoundPos;

        if (canClimb)
        {
            rb.isKinematic = true;
        }
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
                    stateMachine.Run();

                    if (!updated)
                    {
                        _isAnimationFinished = false;
                        animator.SetTrigger("shield");
                        updated = true;
                    }
                    _StandStill();
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("shielding"))
                    {
                        isImmune = true;
                    }

                    if (stateMachine.stateOrder == 1)
                    {
                        animator.SetTrigger("stop shield");
                        isImmune = false;
                    }
                }
                else if (stateMachine.stateOrder == 1)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("shielding"))
                    {
                        animator.SetTrigger("stop shield");
                        isImmune = false;
                    }
                    else
                    {
                        animator.SetBool("walk", false);
                        animator.ResetTrigger("shield");
                        animator.ResetTrigger("stop shield");
                        updated = false;

                        stateMachine.Run();
                    }
                }
                else if (stateMachine.stateOrder == 2)
                {
                    animator.SetBool("walk", false);

                    if (!updated)
                    {
                        _isAnimationFinished = false;
                        animator.SetTrigger("shock");
                        updated = true;
                    }
                    _StandStill();

                    if (_isAnimationFinished)
                    {
                        stateMachine.SkipState();
                    }
                }
                else if (stateMachine.stateOrder == 3)
                {
                    animator.ResetTrigger("shock");
                    updated = false;
                    stateMachine.Run();
                }
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("shielding"))
                {
                    animator.SetTrigger("stop shield");
                    isImmune = false;

                    animator.SetBool("walk", false);
                    animator.ResetTrigger("shield");
                    animator.ResetTrigger("shock");
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                {
                    _isAnimationFinished = true;

                    animator.SetBool("walk", false);
                    animator.ResetTrigger("shield");
                    animator.ResetTrigger("stop shield");
                    animator.ResetTrigger("shock");
                }

                stateMachine.Reset();
                updated = false;

                if (canClimb)
                {
                    if (transform.position.y <= (flipped?rightBoundPos.y:leftBoundPos.y))
                    {
                        posToGo = flipped?leftBoundPos:rightBoundPos;
                    }
                    else if (transform.position.y >= (flipped?leftBoundPos.y:rightBoundPos.y))
                    {
                        posToGo = flipped?rightBoundPos:leftBoundPos;
                    }
                }
                else
                {
                    if (transform.position.x <= leftBoundPos.x)
                    {
                        posToGo = rightBoundPos;
                    }
                    else if (transform.position.x >= rightBoundPos.x)
                    {
                        posToGo = leftBoundPos;
                    }
                }

                if (_isAnimationFinished)
                {
                    _CheckPositionAndMoveHorSmooth(posToGo, speed);
                    animator.SetBool("walk", true);
                }
            }
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    protected override void _MoveHorSmooth(int _moveDirection, float speed)
    {
        if (canClimb)
        {
            _positionMovingTo = new Vector2(rb.velocity.x , speed * Time.fixedDeltaTime * 10f * _moveDirection);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, _positionMovingTo, ref _currentVelocity, _smoothSpeedTime);
        }
        else
        {
            base._MoveHorSmooth(_moveDirection, speed);
        }
    }

    protected override void _CheckPositionAndMoveHorSmooth(Vector3 target, float _speed)
    {
        if (canClimb)
        {
            if (target.y < transform.position.y)
            {
                _MoveHorSmooth( -1, _speed);
                _LookAtTarget(target);
            }
            else if (target.y > transform.position.y)
            {
                _MoveHorSmooth( 1, _speed);
                _LookAtTarget(target);
            } 
        }
        else
        {
            base._CheckPositionAndMoveHorSmooth(target, _speed);
        }
    }

    protected override void _LookAtTarget(Vector3 target)
    {
        if (canClimb)
        {
            if (target.y < transform.position.y && (flipped?(!facingRight):facingRight))
            {
                Flip();
            }
            else if (target.y > transform.position.y && (flipped?facingRight:(!facingRight)))
            {
                Flip();
            }
        }
        else
        {
            base._LookAtTarget(target);
        }
    }
}
