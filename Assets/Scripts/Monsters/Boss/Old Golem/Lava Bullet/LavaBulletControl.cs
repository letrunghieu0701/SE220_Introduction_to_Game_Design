using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBulletControl : MonsterBullet
{

    private CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        circleCollider2D = GetComponent<CircleCollider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision2D) 
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            if (!animator.GetBool("isHitted"))
            {
                collision2D.gameObject.GetComponent<Health>().TakeDamage(damage, transform.position);
                animator.SetBool("isHitted", true);
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                Destroy();
            }
        }
        else if (collision2D.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isHitted", true);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            circleCollider2D.enabled = false;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            if (!animator.GetBool("isHitted"))
            {
                collider2D.GetComponent<Health>().TakeDamage(damage, transform.position);
                animator.SetBool("isHitted", true);
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                Destroy();
            }
        }
        else if (collider2D.CompareTag("Ground"))
        {
            animator.SetBool("isHitted", true);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            circleCollider2D.enabled = false;
        }
    }
}
