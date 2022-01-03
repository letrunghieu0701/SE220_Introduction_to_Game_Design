using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLighterControl : Monster
{
    [Header("Self Skeleton: ")]
    [SerializeField] private float restAfterAttackDelay = 5f;
    [SerializeField] private GameObject blastRange;
    [SerializeField] private int blastDamage = 2;
    [SerializeField] private GameObject blastHitBox;
    [SerializeField] private GameObject ShovelRange;
    [SerializeField] private int shovelDamage = 1;
    [SerializeField] private GameObject ShovelHitBox;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject leftBound;

    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private Vector3 posToGo;

    private State restAfterAttackState;

    private void Awake() 
    {
        _isAnimationFinished = true;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        UpdateBounds();
        posToGo = leftBoundPos;
        restAfterAttackState = new State("rest after attack", restAfterAttackDelay);
        restAfterAttackState.ResetState();

        blastHitBox.GetComponent<MonsterBody>().bodyDamage = blastDamage;
        ShovelHitBox.GetComponent<MonsterBody>().bodyDamage = shovelDamage;
    }

    private void Update() {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(10);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDead)
        {
            if (playerDetected)
            {
                animator.SetBool("walk", false);
                restAfterAttackState.CountDown();
                // animator.ResetTrigger("blast");
                // animator.ResetTrigger("shovel");

                if (restAfterAttackState.timeSup)
                {
                    if (blastRange.GetComponent<AttackRangeDetection>().isInRange)
                    {
                        animator.SetTrigger("blast");
                        _StandStill();
                        blastRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        restAfterAttackState.ResetState();
                    }
                    else if (ShovelRange.GetComponent<AttackRangeDetection>().isInRange)
                    {
                        animator.SetTrigger("shovel");
                        _StandStill();
                        ShovelRange.GetComponent<AttackRangeDetection>().isInRange = false;
                        restAfterAttackState.ResetState();
                    }
                    else
                    {
                        _StandStill();
                        restAfterAttackState.timer = 0f;

                        if (_isAnimationFinished)
                        {
                            _CheckPositionAndMoveHorSmooth(player.transform.position, speed);
                            animator.SetBool("walk", true);
                        }
                    }
                }
                else
                {
                    blastRange.GetComponent<AttackRangeDetection>().isInRange = false;
                    ShovelRange.GetComponent<AttackRangeDetection>().isInRange = false;
                }
            }
            else
            {
                blastRange.GetComponent<AttackRangeDetection>().isInRange = false;
                ShovelRange.GetComponent<AttackRangeDetection>().isInRange = false;
                animator.ResetTrigger("blast");
                animator.ResetTrigger("shovel");
                _isAnimationFinished = true;
                restAfterAttackState.timer = 0f;

                if (transform.position.x <= leftBoundPos.x)
                {
                    posToGo = rightBoundPos;
                }
                else if (transform.position.x >= rightBoundPos.x)
                {
                    posToGo = leftBoundPos;
                }

                _CheckPositionAndMoveHorSmooth(posToGo, speed);
                animator.SetBool("walk", true);
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isDead)
        {
            if (!isImmune)
            {
                animator.SetTrigger("hurt");
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
                _isAnimationFinished = true;

                if (health <= 0)
                {
                    isDead = true;
                    isAllowToMove = false;
                    isAllowToUpdate = false;

                    rb.velocity = Vector2.zero;
                    //rb.isKinematic = true;

                    body.SetActive(false);
                    animator.SetTrigger("die");
                }
                else
                {
                    isImmune = true;
                }
            }
        }
    }

    private void UpdateBounds()
    {
        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
    }
}
