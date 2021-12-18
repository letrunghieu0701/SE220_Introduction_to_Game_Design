using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeControl : Monster
{
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float attackingSpeed = 7f;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private float stayStillTime0 = 0.5f;
    [SerializeField] private float flyUpTime1 = 2f;
    [SerializeField] private float flyUpHeight = 15f;
    [SerializeField] private float flyUpFar = 20f;
    [SerializeField] private float restAfterHurt = 1f;
    [SerializeField] protected GameObject frontWall;

    private Vector2 rightBoundPos;
    private Vector2 leftBoundPos;
    private bool goRight = true;
    private bool resetedBound = true;
    private bool attacked = false;
    private bool isOnGround = false;
    private State state;
    private List<State> states;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
        states = new List<State>();

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                state = new State("stay still about to attack", stayStillTime0);
                state.ResetState();
                states.Add(state);
            }
            else if (i == 1)
            {
                state = new State("fly up after attack", flyUpTime1);
                state.ResetState();
                states.Add(state);
            }
        }

        stateMachine = new MonsterStateMachine(states);

        state = new State("rest after hurt", restAfterHurt);
        state.ResetState();

        enableHurtFlashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
        }
    }

    protected override void FixedUpdate()
    {
        if (isHurt)
        {
            if (!isDead)
            {
                isAllowToMove = false;
                IsAllowToMove(false);

                animator.SetBool("attack", false);
                animator.SetBool("about to attack", false);
                body.GetComponent<MonsterBody>().damaged = false;
                stateMachine.Reset();
                state.CountDown();

                if (state.timeSup)
                {
                    isHurt = false;

                    isAllowToMove = true;
                    IsAllowToMove(true);
                    state.ResetState();
                }
            }
            else
            {
                rb.gravityScale = 1;
                rb.isKinematic = false;

                if (isOnGround)
                {
                    animator.SetTrigger("die on ground");
                }
            }
        }

        if (isAllowToUpdate)
        {
            base.FixedUpdate();

            if (isAllowToMove)
            {
                if (playerDetected)
                {
                    resetedBound = false;

                    if (player.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                    else if (player.transform.position.x > transform.position.x && !facingRight)
                    {
                        Flip();
                    }

                    if (stateMachine.stateOrder == 0)
                    {
                        attacked = false;
                        animator.SetBool("attack", false);
                        body.GetComponent<MonsterBody>().damaged = false;

                        stateMachine.Run();
                        IsAllowToMove(false);

                        animator.SetBool("about to attack", true);

                    }
                    else if (stateMachine.stateOrder == 1)
                    {
                        IsAllowToMove(true);

                        if (!attacked)
                        {
                            if (!body.GetComponent<MonsterBody>().damaged)
                            {
                                animator.SetBool("about to attack", false);
                                animator.SetBool("attack", true);
                                MoveToPosition(player.transform.position, attackingSpeed);
                            }
                            else
                            {
                                attacked = true;
                                body.GetComponent<MonsterBody>().damaged = false;
                            }
                        }
                        else
                        {
                            stateMachine.Run();
                            animator.SetBool("attack", false);
                            MoveToPosition(new Vector2(player.transform.position.x + flyUpFar, player.transform.position.y + flyUpHeight), normalSpeed);
                        }
                    }
                }
                else
                {
                    stateMachine.Reset();
                    animator.SetBool("about to attack", false);
                    animator.SetBool("attack", false);

                    IsAllowToMove(true);
                    if (!resetedBound)
                    {
                        ResetBound();
                        resetedBound = true;
                    }

                    if (new Vector2 (rb.transform.position.x, rb.transform.position.y) == rightBoundPos)
                    {
                        goRight = false;
                    }
                    else if (new Vector2 (rb.transform.position.x, rb.transform.position.y) == leftBoundPos)
                    {
                        goRight = true;
                    }

                    CheckFrontWall();

                    if (goRight)
                    {
                        MoveToPosition(rightBoundPos, normalSpeed);
                        CheckFlipMovingToPosition(rightBoundPos);
                    }
                    else
                    {
                        MoveToPosition(leftBoundPos, normalSpeed);
                        CheckFlipMovingToPosition(leftBoundPos);
                    }
                }
            }
        }
    }

    private void ResetBound()
    {
        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
    }

    protected void CheckFrontWall()
    {
        if (frontWall.GetComponent<DetectFrontWall>().frontWall)
        {
            goRight = !goRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) 
    {
        if (isDead)
        {
            if (collision2D.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
            } 
        } 
    }
}
