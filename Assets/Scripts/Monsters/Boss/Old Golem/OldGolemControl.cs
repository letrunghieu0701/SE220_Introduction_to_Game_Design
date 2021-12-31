using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGolemControl : Monster
{
    [Header("Self:")]
    [SerializeField] protected GameObject rightBound;
    [SerializeField] protected GameObject leftBound;
    [SerializeField] protected GameObject meleeRange;
    [SerializeField] protected GameObject attackMeleeRange;
    [SerializeField] protected GameObject lavaBullet;
    [SerializeField] protected GameObject shootingPosition;
    [SerializeField] private float timeBulletReachTarget = 2.0f;
    [SerializeField] private float nextBulletDelay = 3.0f;
    [SerializeField] private float restAfterMeleeDelay = 1.5f;


    //private bool updated = false;
    private Vector3 rightBoundPos;
    private Vector3 leftBoundPos;
    private Vector3 posToGo;
    private int attackRandom;
    private State shootingState;
    private State restAfterMeleeState;


    private void Awake() {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rightBoundPos = rightBound.transform.position;
        leftBoundPos = leftBound.transform.position;
        posToGo = leftBoundPos;

        shootingState = new State("Next bullet delay", nextBulletDelay);
        shootingState.ResetState();
        shootingState.timer = 0f;

        restAfterMeleeState = new State("Rest after melee delay", restAfterMeleeDelay);
        restAfterMeleeState.ResetState();
        restAfterMeleeState.timer = 0f;

        facingRight = false;
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

        if (!isDead)
        {
            if (playerDetected)
            {
                _isAnimationFinished = false;
                animator.SetBool("walk", false);

                if (attackMeleeRange.GetComponent<DetectionRange>().playerDetected)
                {
                    animator.SetBool("walk", false);
                    animator.ResetTrigger("spit");
                    shootingState.timer = 0f;

                    restAfterMeleeState.CountDown();
                    if (restAfterMeleeState.timeSup)
                    {
                        attackRandom = Random.Range(1,3);
                        if (attackRandom == 1)
                        {
                            animator.SetTrigger("hit");
                        }
                        else
                        {
                            animator.SetTrigger("slash");
                        }

                        restAfterMeleeState.ResetState();
                    }
                }
                else if (meleeRange.GetComponent<DetectionRange>().playerDetected)
                {
                    //restAfterMeleeState.timer = 0f;
                    //shootingState.timer = 0f;

                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("slashing") && !animator.GetCurrentAnimatorStateInfo(0).IsName("hitting") && !animator.GetCurrentAnimatorStateInfo(0).IsName("spitting"))
                    {
                        _CheckPositionAndMoveHorSmooth(player.transform.position, speed);
                        animator.SetBool("walk", true);
                    }
                }
                else
                {
                    //restAfterMeleeState.timer = 0f;

                    shootingState.CountDown();
                    if (shootingState.timeSup)
                    {
                        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                        _LookAtTarget(player.transform.position);
                        animator.SetTrigger("spit");
                        //}

                        shootingState.ResetState();
                    }

                    _StandStill();
                }
            }
            else
            {
                restAfterMeleeState.timer = 0f;
                shootingState.timer = 0f;

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("idling"))
                {
                    _isAnimationFinished = true;

                    animator.ResetTrigger("hit");
                    animator.ResetTrigger("slash");
                    animator.ResetTrigger("spit");
                }

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

    private void ShootPlayer()
    {
        GameObject bullet = Instantiate(lavaBullet, shootingPosition.transform.position, Quaternion.identity);
        bullet.GetComponent<MonsterBullet>().rotateBaseOnVector = false;
        bullet.GetComponent<Rigidbody2D>().velocity = ThrowProjectileVector(player.transform.position, shootingPosition.transform.position, timeBulletReachTarget);
    }
}
