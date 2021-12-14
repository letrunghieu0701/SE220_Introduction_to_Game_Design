using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Self :")]
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float immuneTime = 1f;
    [SerializeField] protected int bodyDamage = 1;
    [SerializeField] protected GameObject detectionRange;
    [SerializeField] protected GameObject body;
    [SerializeField] protected float destroyObjAfterDieDelay = 5f;

    protected bool isImmune = false;
    protected float immuneTimer;
    protected bool facingRight = true;
    protected bool isAllowToMove = true;
    protected bool isAllowToUpdate = true;
    protected bool playerDetected = false;
    protected bool isDead = false;

    protected Animator animator;
    protected GameObject player;
    protected Rigidbody2D rb;

    protected virtual void Start() 
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        immuneTimer = immuneTime;
        body.GetComponent<MonsterBody>().bodyDamage = bodyDamage;
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

    public void TakeDamage (int damage)
    {
        if (!isDead)
        {
            if (!isImmune)
            {
                animator.SetTrigger("hurt");

                health -= damage;

                if (health <= 0)
                {
                    isDead = true;
                    isAllowToMove = false;
                    isAllowToUpdate = false;

                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;

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

    protected void Flip ()
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
