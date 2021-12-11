using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float checkRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallJumpCoolDown;
    [SerializeField] private float xWallJumpForce;

    Rigidbody2D knight_rb;
    Animator knight_ani;
    private BoxCollider2D boxCollider2D;

    private bool isGround;
    private bool isWall;
    private bool isAttack;
    private bool wallSliding;
    private bool wallJumping;
    private bool isDead;
    private bool facingRight = true;
    private bool canJump;
    private bool isWallJumpOver;
    private float horizontalInput;

    private void Awake() {
        knight_rb = transform.GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        if(knight_ani){
            knight_ani.SetBool("isGround", isGround);
            knight_ani.SetFloat("yVelocity", knight_rb.velocity.y);
            knight_ani.SetBool("walk", horizontalInput != 0);
            knight_ani.SetBool("attack", isAttack);
        }

        if(isWallJumpOver == true) {
            MoveHandle();
        }
    }

    void MoveHandle() {
        
        if(Input.GetKey(KeyCode.LeftArrow)) {
            if (Input.GetKey(KeyCode.A)) // Chạy
            {
                knight_rb.velocity = new Vector2(-moveSpeed * 2, knight_rb.velocity.y);
            }
            else // Đi bộ
            {
                knight_rb.velocity = new Vector2(moveSpeed * horizontalInput, knight_rb.velocity.y);
            }
            
            if(facingRight){
                Flip();
            }
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            if (Input.GetKey(KeyCode.A)) { // Chạy
                knight_rb.velocity = new Vector2(moveSpeed * 2, knight_rb.velocity.y);
            } else  {// Đi bộ
                knight_rb.velocity = new Vector2(moveSpeed * horizontalInput, knight_rb.velocity.y);
            }       

            if(!facingRight) {
                Flip();
            }
        } else {
            if(knight_rb) {
                knight_rb.velocity = new Vector2(0f, knight_rb.velocity.y);
            }
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Jump() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if (isGround == true) {
                knight_rb.velocity = Vector2.up * jumpForce;
            } else if(wallSliding == true) {
                wallJumping = true;
                Invoke("setWallJumpToFalse", wallJumpCoolDown);
            }
        }
    }

    void setWallJumpToFalse() {
        wallJumping = false;
    }

    void Attack() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            isAttack = true;
        }
    }

    void setAttackToFalse() {
        isAttack = false;
    }

    void Dead() {
        if(knight_ani){
            knight_ani.SetTrigger("die");
            isDead = true;
        }

        if(knight_rb){
            knight_rb.velocity = new Vector2(0f, knight_rb.velocity.y);
        }
    }

    public bool canShoot(){
        return isGround || isWall || (!isGround && !isWall);
    }

    public bool GetIsWall() {
        return isWall;
    }

    private void OnCollision2D(Collision2D col) {
        if(col.gameObject.CompareTag("enemy")) {
            if(isDead) return;
        }
    }

    void Update() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);
        horizontalInput = Input.GetAxis("Horizontal");

        Jump();

        Attack();

        if(isWall == true && isGround == false && horizontalInput != 0) {
            wallSliding = true;
            isWallJumpOver = false;
        } else {
            wallSliding = false;
            isWallJumpOver = true;
        }

        if(wallSliding == true) {
            knight_rb.velocity = new Vector2(knight_rb.velocity.x, Mathf.Clamp(knight_rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if(wallJumping) {
            knight_rb.velocity = new Vector2(xWallJumpForce * -horizontalInput, jumpForce);
        }
    }
}
