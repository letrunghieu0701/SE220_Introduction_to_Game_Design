using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGuardianControl : Monster
{
    [Header("Self: ")]
    [SerializeField][Range(0f, 10f)] private float smoothSpeedTime = 0f;
    [SerializeField][Range(0f, 10f)] private float smoothSpeedAfterDeadTime = 0.5f;
    [SerializeField] private GameObject chimneySmoke;
    [SerializeField] private float delayBetweenEachSmoke = 0.07f;
    [SerializeField] private GameObject bossGate;
    [SerializeField] private GameObject healthBar;

    [Header("Each State: ")]
    [SerializeField] private float restTime = 5f;
    [SerializeField] private string restStateName = "Rest before combat"; // Use once, at start only

    [Header("State 0: ")]
    [SerializeField] private float stateTime0 = 7f;
    [SerializeField] private string stateName0 = "come to player and attack melee";
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private GameObject SlamRange;
    [SerializeField] private GameObject SlamHitBox;
    [SerializeField] private int SlamDamage = 5;
    [SerializeField] private GameObject SlashRange;
    [SerializeField] private GameObject SlashHitBox;
    [SerializeField] private int SlashDamage = 3;

    [Header("Rest after state 0: ")]
    [SerializeField] private float restTime0 = 3f;
    [SerializeField] private string restStateName0 = "rest after attack melee";

    [Header("State 1: ")]
    [SerializeField] private float stateTime1 = 4f;
    [SerializeField] private string stateName1 = "jump and after jump";
    [SerializeField] private float offGroundJumpToBoundTime = 1f;

    [Header("Rest after state 1: ")]
    [SerializeField] private float restTime1 = 3f;
    [SerializeField] private string restStateName1 = "rest after jump afar from player";

    [Header("State 2: ")]
    [SerializeField] private float stateTime2 = 3f;
    [SerializeField] private string stateName2 = "prepare to shoot";

    [Header("State 3: ")]
    [SerializeField] private float stateTime3 = 10f; // Symbolic
    [SerializeField] private string stateName3 = "shoot";
    [SerializeField] private int numberOfBullet = 10;
    [SerializeField] private int bulletDamage = 2;
    [SerializeField] private float timeBulletReachTarget = 2.5f;
    [SerializeField] private float delayBetweenEachBullet = 1f;
    [SerializeField] private GameObject boom;
    [SerializeField] private GameObject shootingPos;
    
    [Header("Rest after state 3: ")]
    [SerializeField] private float restTime2 = 3f;
    [SerializeField] private string restStateName2 = "rest after shooting";

    [Header("State 4: ")]
    [SerializeField] private float stateTime4 = 10f;
    [SerializeField] private string stateName4 = "run crazy to player's nearest bound and back";
    [SerializeField] private float crazySpeed = 25f;
    [SerializeField] private float boundDistanceError = 0.2f;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject leftBound;

    [Header("State 5: ")]
    [SerializeField] private float stateTime5 = 3f;
    [SerializeField] private string stateName5 = "prepare to jump to player";

    [Header("State 6: ")]
    [SerializeField] private float stateTime6 = 10f; // Symbolic
    [SerializeField] private string stateName6 = "jump to player";
    [SerializeField] private int jumpDamage = 3; // Set body damage to this, only at this animation
    [SerializeField] private float offGroundJumpToPlayerTime = 1f;
    [SerializeField] private float yAxisOffsetForJumpToPlayer = 1f;
    [SerializeField] private GameObject groundCheck;


    [Header("Rest after state 6: ")]
    [SerializeField] private float restTime3 = 3f;
    [SerializeField] private string restStateName3 = "rest after jump to player";
    
    
    private bool startBattle = false;
    private bool isAnimationFinished = false;
    private bool isPreparingToJump = false;
    private bool isJumping = false;
    private bool isLanded = false;
    private int bulletCount = 0;
    private bool shooted = false;
    private bool ranToFirstBound = false;
    private bool ranToSecondBound = false;
    private bool isAboutToDie = false;

    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private Vector3 positionMovingTo;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 firstBoundRanTo;
    private Vector3 secondBoundRanTo;


    private MonsterStateMachine behavioralStateMachine;
    private List<State> behavioralStates;
    private MonsterStateMachine restStateMachine;
    private List<State> restStates;
    private State state;
    private State immuneState;


   private void Awake() 
   {
        behavioralStates = new List<State>();
        restStates = new List<State>();

        for (int i = 0; i < 7; i++)
        {
            if (i == 0)
            {
                state = new State(stateName0, stateTime0);
                state.ResetState();
                behavioralStates.Add(state);
                state = new State(restStateName0, restTime0);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 1)
            {
                state = new State(stateName1, stateTime1);
                state.ResetState();
                behavioralStates.Add(state);
                state = new State(restStateName1, restTime1);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 2)
            {
                state = new State(stateName2, stateTime2);
                state.ResetState();
                behavioralStates.Add(state);
                state = new State(restStateName2, restTime2);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 3)
            {
                state = new State(stateName3, stateTime3);
                state.ResetState();
                behavioralStates.Add(state);
                state = new State(restStateName3, restTime3);
                state.ResetState();
                restStates.Add(state);
            }
            else if (i == 4)
            {
                state = new State(stateName4, stateTime4);
                state.ResetState();
                behavioralStates.Add(state);
            }
            else if (i == 5)
            {
                state = new State(stateName5, stateTime5);
                state.ResetState();
                behavioralStates.Add(state);
            }
            else if (i == 6)
            {
                state = new State(stateName6, stateTime6);
                state.ResetState();
                behavioralStates.Add(state);
            }
        }

        behavioralStateMachine = new MonsterStateMachine(behavioralStates);
        restStateMachine = new MonsterStateMachine(restStates);

        state = new State(restStateName, restTime);
        state.ResetState();

        immuneState = new State("immune time", immuneTime);
        immuneState.ResetState();

        chimneySmoke.GetComponent<ChimneyControl>().delayBetweenEachSmoke = delayBetweenEachSmoke;
   }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;

        SlamHitBox.GetComponent<MonsterBody>().bodyDamage = SlamDamage;
        SlashHitBox.GetComponent<MonsterBody>().bodyDamage = SlashDamage;
        boom.GetComponent<MonsterBullet>().damage = bulletDamage;

        startBattle = false;
        isAnimationFinished = false;
        animator.enabled = false;
        body.SetActive(false);
        isImmune = true;

        // TEST
        // behavioralStateMachine.stateOrder = 4;
        // restStateMachine.stateOrder = 2;
    }

    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(35);
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (isHurt)
        {
            if (!isDead)
            {
                isAboutToDie = false;

                immuneState.CountDown();
                if (immuneState.timeSup)
                {
                    immuneState.ResetState();
                    isHurt = false;
                }
            }
            else
            {
                smoothSpeedTime = smoothSpeedAfterDeadTime;
                MoveHorSmooth(Mathf.RoundToInt(transform.localScale.x), 0f);

                if (isAboutToDie)
                {
                    chimneySmoke.GetComponent<ChimneyControl>().isSmoking = false;
                    animator.SetTrigger("die");
                }
            }
        }

        if (bossGate.GetComponent<BossGateControl>().areBossGatesUpdated)
        {
            if (!isDead)
            {
                if (!startBattle)
                {
                    isImmune = true;

                    state.CountDown();

                    if (state.timer / state.time <= 0.3f)
                    {
                        LookAtTarget(player.transform.position);
                    }

                    if (state.timer / state.time <= 0.5f)
                    {
                        animator.enabled = true;
                    }

                    if (state.timer / state.time <= 0.8f)
                    {
                        chimneySmoke.GetComponent<ChimneyControl>().isSmoking = true;
                    }

                    if (state.timeSup)
                    {
                        startBattle = true;
                        healthBar.SetActive(true);
                        isImmune = false;
                        body.SetActive(true);
                        SlamRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        SlashRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        
                        state = new State("delay between each bullet", delayBetweenEachBullet);
                        state.ResetState();
                    }
                }
            }
        }

        if (startBattle)
        {
            ImmuneCountDown();

            if (behavioralStateMachine.stateOrder == 0)
            {
                behavioralStateMachine.Run();

                if (!SlamRange.GetComponent<AttackRangeDetection>().isInRange && !SlashRange.GetComponent<AttackRangeDetection>().isInRange)
                {
                    isAnimationFinished = false;

                    CheckPositionAndMoveHorSmooth(player.transform.position, normalSpeed);
                    animator.SetBool("walk", true);
                }
                else
                {
                    animator.SetBool("walk", false);
                    StandStill();

                    if (SlamRange.GetComponent<AttackRangeDetection>().isInRange)
                    {
                        animator.SetTrigger("slam");
                    }
                    else if (SlashRange.GetComponent<AttackRangeDetection>().isInRange)
                    {
                        animator.SetTrigger("slash");
                    }

                    if (isAnimationFinished)
                    {
                        SlamRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        SlashRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        isAnimationFinished = false;
                    }
                }
            }
            else if (restStateMachine.stateOrder == 0)
            {
                animator.ResetTrigger("slam");
                animator.ResetTrigger("slash");
                animator.SetBool("walk", false);
                SlamRange.GetComponent<AttackRangeDetection>().isInRange = false;
                SlashRange.GetComponent<AttackRangeDetection>().isInRange = false;

                restStateMachine.Run();
                StandStill();
            }
            else if (behavioralStateMachine.stateOrder == 1)
            {
                behavioralStateMachine.Run();

                if (!isLanded)
                {
                    if (!isJumping)
                    {
                        if (!isPreparingToJump)
                        {
                            LookAtTarget(OppositeBoundOfPlayer(true));
                            StandStill();
                            animator.SetTrigger("about to jump");
                            isPreparingToJump = true;

                            isAnimationFinished = false;
                        }
                        else
                        {
                            if (isAnimationFinished)
                            {
                                LookAtTarget(OppositeBoundOfPlayer(true));
                                JumpToTarget(OppositeBoundOfPlayer(true), offGroundJumpToBoundTime);
                                animator.SetTrigger("jump");
                                isJumping = true;

                                isAnimationFinished = false;
                            }
                        }
                    }
                    else
                    {
                        LookAtTarget(OppositeBoundOfPlayer(true));
                        isAnimationFinished = false;

                        if (groundCheck.GetComponent<GroundCheck>().isGrounded)
                        {
                            isLanded = true;
                        }
                    }
                }
                else
                {
                    StandStill();
                    animator.SetTrigger("land");

                    if (isAnimationFinished)
                    {
                        behavioralStateMachine.SkipState();
                        isAnimationFinished = false;
                    }
                }
            }
            else if (restStateMachine.stateOrder == 1)
            {
                animator.ResetTrigger("land");
                animator.ResetTrigger("jump");
                animator.ResetTrigger("about to jump");
                isPreparingToJump = false;
                isJumping = false;
                isLanded = false;

                restStateMachine.Run();
                if (restStateMachine.states[restStateMachine.stateOrder].timer / restStateMachine.states[restStateMachine.stateOrder].time >= 0.5)
                {
                    LookAtTarget(player.transform.position);
                }
            }
            else if (behavioralStateMachine.stateOrder == 2)
            {
                behavioralStateMachine.Run();

                LookAtTarget(player.transform.position);
                StandStill();
                animator.SetTrigger("about to shoot");
            }
            else if (behavioralStateMachine.stateOrder == 3)
            {
                animator.ResetTrigger("about to shoot");

                behavioralStateMachine.Run();

                StandStill();
                animator.SetBool("shoot", true);

                if (shooted)
                {
                    if (isAnimationFinished)
                    {
                        animator.SetBool("shoot", false);
                        behavioralStateMachine.SkipState();
                        isAnimationFinished = false;
                    }
                }
                else if (isAnimationFinished)
                {
                    state.CountDown();

                    if (state.timeSup)
                    {
                        state.ResetState();
                        animator.Play("shooting", -1, 0f);
                    }
                }
            }
            else if (restStateMachine.stateOrder == 2)
            {
                shooted = false;
                state.ResetState();

                restStateMachine.Run();

                StandStill();
            }
            else if (behavioralStateMachine.stateOrder == 4)
            {
                behavioralStateMachine.Run();

                if (!ranToSecondBound)
                {
                    if (!ranToFirstBound)
                    {
                        if (!animator.GetBool("walk"))
                        {
                            if (player.transform.position.x < transform.position.x)
                            {
                                firstBoundRanTo = leftBoundPos;
                            }
                            else
                            {
                                firstBoundRanTo = rightBoundPos;
                            }

                            animator.SetBool("walk", true);
                        }

                        CheckPositionAndMoveHorSmooth (firstBoundRanTo, crazySpeed);
                        
                        if (Vector3.Distance(firstBoundRanTo, transform.position) <= boundDistanceError)
                        {
                            ranToFirstBound = true;

                            if (firstBoundRanTo == rightBoundPos)
                            {
                                secondBoundRanTo = leftBoundPos;
                            }
                            else
                            {
                                secondBoundRanTo = rightBoundPos;
                            }
                            StandStill();
                        }
                    }
                    else
                    {
                        CheckPositionAndMoveHorSmooth (secondBoundRanTo, crazySpeed);
                        if (Vector3.Distance(secondBoundRanTo, transform.position) <= boundDistanceError)
                        {
                            ranToSecondBound = true;
                        }
                    }
                }
                else
                {
                    animator.SetBool("walk", false);
                    StandStill();
                    LookAtTarget(player.transform.position);
                    behavioralStateMachine.SkipState();
                }
            }
            else if (behavioralStateMachine.stateOrder == 5)
            {
                ranToFirstBound = false;
                ranToSecondBound = false;

                behavioralStateMachine.Run();

                if (!isPreparingToJump)
                {
                    LookAtTarget(player.transform.position);
                    StandStill();
                    animator.SetTrigger("about to jump");
                    isPreparingToJump = true;
                }
            }
            else if (behavioralStateMachine.stateOrder == 6)
            {
                isPreparingToJump = false;

                behavioralStateMachine.Run();

                if (!isLanded)
                {
                    if (!isJumping)
                    {
                        LookAtTarget(player.transform.position);
                        JumpToTarget(player.transform.position, offGroundJumpToPlayerTime);
                        animator.SetTrigger("jump");
                        isJumping = true;

                        body.GetComponent<MonsterBody>().bodyDamage = jumpDamage;
                    }
                    else
                    {
                        if (groundCheck.GetComponent<GroundCheck>().isGrounded)
                        {
                            animator.SetTrigger("land");
                            isLanded = true;

                            isAnimationFinished = false;
                        }
                    }
                }
                else
                {
                    StandStill();

                    if (isAnimationFinished)
                    {
                        isAnimationFinished = false;

                        isPreparingToJump = false;
                        isJumping = false;
                        isLanded = false;

                        behavioralStateMachine.stateOrder = behavioralStateMachine.states.Count + 2;
                        // To make the stateorder out of scope
                    }
                }
            }
            else if (restStateMachine.stateOrder == 3)
            {
                restStateMachine.Run();
                if (restStateMachine.stateOrder == 0)
                {
                    animator.ResetTrigger("land");
                    animator.ResetTrigger("jump");
                    animator.ResetTrigger("about to jump");
                    animator.ResetTrigger("slam");
                    animator.ResetTrigger("slash");
                    SlamRange.GetComponent<AttackRangeDetection>().isInRange = false;
                    SlashRange.GetComponent<AttackRangeDetection>().isInRange = false;

                    behavioralStateMachine.Reset();
                } 
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isDead)
        {
            if (!isImmune)
            {
                isHurt = true;
                if (enableHurtFlashing)
                {
                    StartCoroutine(HurtFlashing());
                }

                health -= damage;
                if (monsterHealthBar)
                {
                    monsterHealthBar.UpdateHealth(health);
                }

                if (health <= 0)
                {
                    isDead = true;
                    startBattle = false;

                    body.SetActive(false);
                    animator.SetTrigger("hurt");
                }
                else
                {
                    isImmune = true;
                }
            }
        }
    }

    private void ImmuneCountDown()
    {
        if (isImmune)
        {
            immuneTimer -= Time.fixedDeltaTime;
            if (immuneTimer <= 0)
            {
                isImmune = false;
                immuneTimer = immuneTime;
            }
        }
    }

    private void ShootPlayer()
    {
        GameObject bullet = Instantiate(boom, shootingPos.transform.position, Quaternion.identity);
        bullet.GetComponent<MonsterBullet>().rotateBaseOnVector = false;
        bullet.GetComponent<Rigidbody2D>().velocity = ThrowProjectileVector(player.transform.position, shootingPos.transform.position, timeBulletReachTarget);
    }

    private void JumpToTarget(Vector3 target, float offGroundTime)
    {
        if (target == player.transform.position)
        {
            if (target.y < transform.position.y)
            {
                target.y += yAxisOffsetForJumpToPlayer;
            }
        }

        rb.velocity = ThrowProjectileVector(target, transform.position, offGroundTime);
    }

    private Vector3 OppositeBoundOfPlayer(bool isOpposite)
    {
        if (Vector3.Distance(player.transform.position, rightBoundPos) > Vector3.Distance(player.transform.position, leftBoundPos))
        {
            if (isOpposite)
            {
                return rightBoundPos;
            }
            else
            {
                return leftBoundPos;
            }
            
        }
        else
        {
            if (isOpposite)
            {
                return leftBoundPos;
            }
            else
            {
                return rightBoundPos;
            }
        }
    }

    private void StandStill()
    {
        rb.velocity = Vector2.zero;
    }

    private void MoveHorSmooth(int _moveDirection, float speed)
    {
        positionMovingTo = new Vector2(speed * Time.fixedDeltaTime * 10f * _moveDirection, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, positionMovingTo, ref currentVelocity, smoothSpeedTime);
    }

    private void CheckPositionAndMoveHorSmooth(Vector3 target, float _speed)
    {
        if (target.x < transform.position.x)
        {
            MoveHorSmooth( -1, _speed);
            LookAtTarget(target);
        }
        else if (target.x > transform.position.x)
        {
            MoveHorSmooth( 1, _speed);
            LookAtTarget(target);
        } 
    }

    private void LookAtTarget(Vector3 target)
    {
        if (target.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (target.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    // Set body damade back down when finished jumping
    private void OnCollisionEnter2D(Collision2D collision2D) 
    {
        if (isJumping)
        {
            if (collision2D.gameObject.CompareTag("Ground"))
            {
                body.GetComponent<MonsterBody>().bodyDamage = bodyDamage;
            }
        }    
    }

    // Count bullet each time shoot - Add in animation clip
    private void CountBullet()
    {
        bulletCount++;
        if (bulletCount >= numberOfBullet)
        {
            shooted = true;
            bulletCount = 0;
        }
    }

    // Check is about to die
    private void CheckIsAboutToDie()
    {
        isAboutToDie = true;
    }

    // Check animation finish - Add in animation clip
    private void CheckAnimationFinish()
    {
        isAnimationFinished = true;
    }

    // Check animation start - Add in animation clip
    private void CheckAnimationStart()
    {
        isAnimationFinished = false;
    }

}
