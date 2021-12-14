using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSlimeControl : Monster
{
    [Header("Self :")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float crazySpeed = 15f;
    [SerializeField] private float normalSpeedTime = 3f;
    [SerializeField] private float crazySpeedTime = 2f;
    [SerializeField] [Range(0,10)] private float smoothSpeedTime = 1f;
    [SerializeField] protected GameObject frontWall;
    [SerializeField] protected GameObject frontGround;
 
     private int moveDirection = 1;
    private Vector3 movingToPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private float normalSpeedTimer;
    private float crazySpeedTimer;
    private int stageOrder;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        speed = normalSpeed;
        normalSpeedTimer = normalSpeedTime;
        crazySpeedTimer = crazySpeedTime;
        stageOrder = 1;
    }

    private void Update() 
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
        }
    }

    protected override void FixedUpdate()
    {
        if (isAllowToUpdate)
        {
            base.FixedUpdate();

            if (isAllowToMove)
            {
                CheckFrontWall();
                CheckFrontGround();

                if (playerDetected)
                {
                    if (stageOrder == 1)
                    {
                        normalSpeedTimer -= Time.fixedDeltaTime;
                        speed = normalSpeed;

                        if (normalSpeedTimer <= 0)
                        {
                            stageOrder = 2;
                            crazySpeedTimer = crazySpeedTime;
                        }
                    }
                    else
                    {
                        crazySpeedTimer -= Time.fixedDeltaTime;
                        speed = crazySpeed;

                        if (crazySpeedTimer <= 0)
                        {
                            stageOrder = 1;
                            normalSpeedTimer = normalSpeedTime;
                        }
                    }
                }
                else
                {
                    speed = normalSpeed;
                }

                movingToPosition = new Vector2(speed * Time.fixedDeltaTime * 10f * moveDirection, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, movingToPosition, ref currentVelocity, smoothSpeedTime);
            }
        }
    }

    protected void CheckFrontWall()
    {
        if (frontWall.GetComponent<DetectFrontWall>().frontWall)
        {
            moveDirection *= -1;
            Flip();
        }
    }

    protected void CheckFrontGround()
    {
        if (!frontGround.GetComponent<DetectFrontGround>().frontGround)
        {
            moveDirection *= -1;
            Flip();
        }
    }
}
