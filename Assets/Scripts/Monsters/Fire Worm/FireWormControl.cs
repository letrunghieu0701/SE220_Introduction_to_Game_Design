using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWormControl : Monster
{ 
    [Header("Bullet :")]  
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject shootPos;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float timeBetweenEachBullet;
    [SerializeField] private float delayBeforeFirstBullet;
    [SerializeField] private float timeBulletReachTarget = 1f;

    private float timer = 0f; 

    protected override void Start()
    {
        base.Start();

        isAllowToMove = false;
        timer = delayBeforeFirstBullet;
    }

    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
        }

        if (isAllowToUpdate)
        {
            if (!isAllowToMove)
            {
                if (playerDetected)
                {
                    if (player.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                    else if (player.transform.position.x > transform.position.x && !facingRight)
                    {
                        Flip();
                    }
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (isAllowToUpdate)
        {
            base.FixedUpdate();

            if (playerDetected)
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    animator.SetTrigger("shoot");
                    timer = timeBetweenEachBullet;
                }
            }
            else
            {
                timer = delayBeforeFirstBullet;
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(fireBall, shootPos.transform.position, Quaternion.identity);
        bullet.GetComponent<MonsterBullet>().damage = bulletDamage;
        bullet.GetComponent<MonsterBullet>().rotateBaseOnVector = true;
        bullet.GetComponent<Rigidbody2D>().velocity = ThrowProjectileVector(player.transform.position, shootPos.transform.position, timeBulletReachTarget);
    }
}
