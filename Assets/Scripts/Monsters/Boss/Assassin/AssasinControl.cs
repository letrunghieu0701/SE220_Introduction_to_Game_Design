using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinControl : Monster
{
    [Header("Assassin self :")]
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject leftBound;

    [Header("Rest states:")]
    [SerializeField] private float restB4BattleTime = 5.0f;
    [SerializeField] private float restBtweenStatesTime = 1.0f;
    [SerializeField] private float restB4Appearing = 1.5f;
    [SerializeField] private float restB4Slamming = 1.0f;

    [Header("State 0: run and attack")]
    [SerializeField] private float runAndAttackTime = 10.0f;
    [SerializeField] private GameObject meleeAttackRange;

    [Header("State 1: tele attack")]
    [SerializeField] private int teleAttackTimes = 6;
    [SerializeField] private float appearingDistance = 1.0f;

    [Header("State 2: slam")]
    [SerializeField] private int slamAttackTimes = 5;
    [SerializeField] private float slammingDistance = 1f;


    private bool beginBattle = false;
    private bool teleAttacked = false;
    private int teleAttackCount = 0;
    private bool slamAttacked = false;
    private int slamAttackCount = 0;
    private bool immuneToTakeDamage = false;

    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private State state;
    private List<State> states;
    private List<State> restStates;
    private MonsterStateMachine restStateMachine;
    private State restBtweenWavesState;
    private State restB4AppearingState;
    private State restB4SlammingState;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        body.SetActive(false);

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;

        states = new List<State>();
        restStates = new List<State>();
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                state = new State("run and attack", runAndAttackTime);
                state.ResetState();
                states.Add(state);

                state = new State("rest 0", restBtweenStatesTime);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 1)
            {
                state = new State("tele attack", 50.0f);
                state.ResetState();
                states.Add(state);

                state = new State("rest 1", restBtweenStatesTime);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 2)
            {
                state = new State("slam attack", 50.0f);
                state.ResetState();
                states.Add(state);

                state = new State("rest 2", restBtweenStatesTime);
                state.ResetState();
                restStates.Add(state);
            }
        }

        stateMachine = new MonsterStateMachine(states);
        stateMachine.Reset();

        restStateMachine = new MonsterStateMachine(restStates);
        restStateMachine.Reset();

        restBtweenWavesState = new State("rest between states", restBtweenStatesTime);
        restBtweenWavesState.ResetState();

        state = new State("rest before battle", restB4BattleTime);
        state.ResetState();

        restB4AppearingState = new State("rest before apprearing", restB4Appearing);
        restB4AppearingState.ResetState();

        restB4SlammingState = new State("rest before slamming", restB4Slamming);
        restB4SlammingState.ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!battleStarted)
        {
            immuneToTakeDamage = true;
        }

        if (battleStarted && !beginBattle)
        {
            state.CountDown();
            if (state.timeSup)
            {
                beginBattle = true;
                immuneToTakeDamage = false;
                body.SetActive(true);
            }
        }

        if (beginBattle && !isDead)
        {
            if (stateMachine.stateOrder == 0)
            {
                stateMachine.Run();

                if (meleeAttackRange.GetComponent<DetectionRange>().playerDetected)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("running"))
                    {
                        animator.SetTrigger("approached");
                    }
                    
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        animator.SetTrigger("attack twice");
                    }
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        animator.SetTrigger("run");
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("running"))
                    {
                        _CheckPositionAndMoveHorSmooth(player.transform.position, speed);
                    }
                }
            }
            else if (restStateMachine.stateOrder == 0)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("running"))
                {
                    animator.SetTrigger("approached");
                    _StandStill();
                }
                else
                {
                    animator.ResetTrigger("approached");
                    animator.ResetTrigger("attack twice");
                    animator.ResetTrigger("run");
                }

                restBtweenWavesState.CountDown();
                if (restBtweenWavesState.timeSup)
                {
                    restBtweenWavesState.ResetState();
                    restStateMachine.SkipState();
                }
            }
            else if (stateMachine.stateOrder == 1)
            {
                if (teleAttacked)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        stateMachine.SkipState();
                    }
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        animator.SetTrigger("tele attack");
                    }

                    if (!animator.enabled)
                    {
                        restB4AppearingState.CountDown();
                        if (restB4AppearingState.timeSup)
                        {
                            TeleNearPlayer(leftBoundPos, rightBoundPos, appearingDistance, true);
                            
                            restB4AppearingState.ResetState();
                        }
                    }
                }
            }
            else if (restStateMachine.stateOrder == 1)
            {
                animator.ResetTrigger("tele attack");

                restBtweenWavesState.CountDown();
                if (restBtweenWavesState.timeSup)
                {
                    restBtweenWavesState.ResetState();
                    restStateMachine.SkipState();
                }
            }
            else if (stateMachine.stateOrder == 2)
            {
                if (slamAttacked)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        stateMachine.stateOrder = 5;
                        // Make stateOrder out of scope
                    }
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                    {
                        animator.SetTrigger("slam attack");
                    }

                    if (!animator.enabled)
                    {
                        restB4SlammingState.CountDown();
                        if (restB4SlammingState.timeSup)
                        {
                            TeleNearPlayer(leftBoundPos, rightBoundPos, slammingDistance, false);
                            
                            restB4SlammingState.ResetState();
                        }
                    }
                }
            }
            else if (restStateMachine.stateOrder == 2)
            {
                animator.ResetTrigger("slam attack");

                restBtweenWavesState.CountDown();
                if (restBtweenWavesState.timeSup)
                {
                    restBtweenWavesState.ResetState();
                    restStateMachine.Reset();
                    teleAttacked = false;
                    slamAttacked = false;

                    stateMachine.Reset();
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!immuneToTakeDamage)
        {
            base.TakeDamage(damage);
        }
    }

    private void TeleNearPlayer(Vector3 leftBoundPosition, Vector3 rightBoundPosition, float appearDistance, bool teleBehind)
    {
        if (player.transform.localScale.x == (teleBehind ? 1f : -1f))
        {
            if (player.transform.position.x - appearDistance <= leftBoundPosition.x)
            {
                transform.position = new Vector2(player.transform.position.x + appearDistance, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(player.transform.position.x - appearDistance, transform.position.y);
            }
        }
        else
        {
            if (player.transform.position.x + appearDistance >= rightBoundPosition.x)
            {
                transform.position = new Vector2(player.transform.position.x - appearDistance, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(player.transform.position.x + appearDistance, transform.position.y);
            }
        }

        _LookAtTarget(player.transform.position);
        Appear();
    }

    private void CountTeleAttack()
    {
        teleAttackCount++;
        if (teleAttackCount >= teleAttackTimes)
        {
            teleAttacked = true;
            teleAttackCount = 0;
        }
    }

    private void CountSlamAttack()
    {
        slamAttackCount++;
        if (slamAttackCount >= slamAttackTimes)
        {
            slamAttacked = true;
            slamAttackCount = 0;
        }
    }

    private void Appear()
    {
        animator.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void Disappear()
    {
        body.SetActive(false);
        animator.enabled = false;
        spriteRenderer.enabled = false;
        immuneToTakeDamage = true;
    }

    private void SetBodyBack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slamming"))
        {
            if (slamAttacked)
            {
                body.SetActive(true);
                immuneToTakeDamage = false;
            }
        }
        else
        {
            body.SetActive(true);
            immuneToTakeDamage = false;
        }
    }
}
