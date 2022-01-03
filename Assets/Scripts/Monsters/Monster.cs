using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Self :")]
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField][Range(0f, 10f)] protected float _smoothSpeedTime = 0f;
    [SerializeField] protected float immuneTime = 1f;
    [SerializeField] protected int bodyDamage = 1;
    [SerializeField] protected GameObject detectionRange;
    [SerializeField] protected GameObject body;
    [SerializeField] public float destroyObjAfterDieDelay = 5f;
    [SerializeField] protected Material flashMat;
    [SerializeField] protected float flashingDelay = 0.3f;
    [SerializeField] protected bool enableHurtFlashing = true;
    [SerializeField] protected MonsterHealthBar monsterHealthBar;


    [HideInInspector] public bool battleStarted = false;
    protected bool isImmune = false;
    protected float immuneTimer;
    protected bool facingRight = true;
    protected bool isAllowToMove = true;
    protected bool isAllowToUpdate = true;
    protected bool playerDetected = false;
    [HideInInspector] public bool isDead = false;
    protected bool isHurt = false;
    protected bool _isAnimationFinished = false;
    protected State flashingState;
    protected Vector3 _positionMovingTo;
    protected Vector3 _currentVelocity = Vector3.zero;

    protected MonsterStateMachine stateMachine;
    protected Animator animator;
    protected GameObject player;
    protected Rigidbody2D rb;

    protected virtual void Start() 
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        this.GetComponent<Renderer>().material = new Material(flashMat);

        immuneTimer = immuneTime;
        body.GetComponent<MonsterBody>().bodyDamage = bodyDamage;

        flashingState = new State("flashing time", flashingDelay);
        flashingState.ResetState();

        if (monsterHealthBar)
        {
            monsterHealthBar.SetHealth(health, health);
        }
    }

    protected virtual void FixedUpdate() 
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

        playerDetected = detectionRange.GetComponent<DetectionRange>().playerDetected;
    }

    protected IEnumerator HurtFlashing()
    {
        this.GetComponent<Renderer>().material.SetInt("_isFlashing", 1);
        yield return new WaitForSeconds(flashingDelay);
        this.GetComponent<Renderer>().material.SetInt("_isFlashing", 0);
    }

    protected void MoveToPosition (Vector2 pos, float speed)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, pos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public virtual void TakeDamage (int damage)
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


    protected void _CheckAnimationStart()
    {
        _isAnimationFinished = false;
    }

    protected void _CheckAnimationFinish()
    {
        _isAnimationFinished = true;
    }
    
    protected void _StandStill()
    {
        rb.velocity = Vector2.zero;
    }

    protected virtual void _MoveHorSmooth(int _moveDirection, float speed)
    {
        _positionMovingTo = new Vector2(speed * Time.fixedDeltaTime * 10f * _moveDirection, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, _positionMovingTo, ref _currentVelocity, _smoothSpeedTime);
    }

    protected virtual void _CheckPositionAndMoveHorSmooth(Vector3 target, float _speed)
    {
        if (target.x < transform.position.x)
        {
            _MoveHorSmooth( -1, _speed);
            _LookAtTarget(target);
        }
        else if (target.x > transform.position.x)
        {
            _MoveHorSmooth( 1, _speed);
            _LookAtTarget(target);
        } 
    }

    protected virtual void _LookAtTarget(Vector3 target)
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

    protected void IsAllowToMove(bool allow)
    {
        if (!allow)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    protected void CheckFlipMovingToPosition(Vector2 pos)
    {
        if (pos.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (pos.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    protected virtual void Flip ()
    {
        facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    protected Vector2 ThrowProjectileVector (Vector2 target, Vector2 origin, float time)
    {
        Vector2 distance = target - origin;

        float Dx = Mathf.Abs(distance.x);
        float Dy = distance.y;

        float Vx = Dx / time;
        float Vy = Dy / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 result = distance.normalized;

        result *= Vx;
        result.y = Vy;

        return result;
    }

    protected virtual void DeleteGameObjDelay()
    {
        Invoke("DeleteGameObj", destroyObjAfterDieDelay);
    }

    protected virtual void DeleteGameObj()
    {
        Destroy(this.gameObject);
    }
}
