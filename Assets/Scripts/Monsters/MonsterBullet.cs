using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public int damage = 1;
    public bool rotateBaseOnVector = true;

    protected Animator animator;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() 
    {
        if (rotateBaseOnVector)
        {
            if (!animator.GetBool("isHitted"))
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, rb.velocity));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            if (!animator.GetBool("isHitted"))
            {
                collider2D.GetComponent<Health>().TakeDamage(damage, transform.position);
                animator.SetBool("isHitted", true);
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }
        else if (collider2D.CompareTag("Ground"))
        {
            animator.SetBool("isHitted", true);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }

    protected virtual void DeleteGameObjDelay(float destroyObjAfterDieDelay)
    {
        Invoke("Destroy", destroyObjAfterDieDelay);
    }

    protected virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}
