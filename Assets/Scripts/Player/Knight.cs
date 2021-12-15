using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkRadius;

    [Header("Force")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float xWallJumpForce;
    [SerializeField] private float dashDistance = 10f;

    [Header("Timer")]
    [SerializeField] private float wallJumpCoolDown;

    Rigidbody2D knight_rb;
    Animator knight_ani;
    private BoxCollider2D boxCollider2D;
    private KeyCode lastKeyCode;

    private bool isGround;
    private bool isWall;
    private bool isAttack;
    private bool wallSliding;
    private bool wallJumping;
    private bool isDead;
    private bool facingRight = true;
    private bool canJump;
    private bool isWallJumpOver;
    private bool isDashing;

    private float horizontalInput;
    private float doubleTapTime;
    private float delayJumpTime = 1f;
    private float delayJumpCoolDown;

    private void Awake() {
        knight_rb = transform.GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);

        if(isWall == true && isGround == false && (horizontalInput <= -1 || horizontalInput >= 1)) {
            wallSliding = true;
            isWallJumpOver = false;
        } else {
            wallSliding = false;
            isWallJumpOver = true;
        }

        if(knight_ani){
            knight_ani.SetBool("isGround", isGround);
            knight_ani.SetFloat("yVelocity", knight_rb.velocity.y);
            knight_ani.SetBool("walk", horizontalInput != 0);
            knight_ani.SetBool("attack", isAttack);
            knight_ani.SetBool("dash", isDashing);
        }

        if(isWallJumpOver == true && isDashing == false) {
            MoveHandle();
        }
    }

    void MoveHandle() {
        
        if(Input.GetKey(KeyCode.LeftArrow)) {
            knight_rb.velocity = new Vector2(moveSpeed * horizontalInput, knight_rb.velocity.y);
            
            if(facingRight){
                Flip();
            }
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            knight_rb.velocity = new Vector2(moveSpeed * horizontalInput, knight_rb.velocity.y);

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
        if (isGround == true) {
            knight_rb.velocity = Vector2.up * jumpForce;
        } else if(wallSliding == true) {
            wallJumping = true;
            Invoke("setWallJumpToFalse", wallJumpCoolDown);
        }
    }

    void setWallJumpToFalse() {
        wallJumping = false;
    }

    void Attack() {
        if(isWall == false && isGround == true) {
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

    public bool GetIsDashing() {
        return isDashing;
    }

    private IEnumerator Dash(float dir) {
        float gravity = knight_rb.gravityScale;
        if(isDashing == true) {
            knight_rb.gravityScale = 0;
            knight_rb.velocity = new Vector2(knight_rb.velocity.x, 0f);
            knight_rb.AddForce(new Vector2(dashDistance * dir, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.4f);
        knight_rb.gravityScale = gravity;
        isDashing = false;
    }

    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Z)) {
            Attack();
        }

        if(wallSliding == true) {
            knight_rb.velocity = new Vector2(knight_rb.velocity.x, Mathf.Clamp(knight_rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if(wallJumping) {
            knight_rb.velocity = new Vector2(xWallJumpForce * -horizontalInput, jumpForce);
        }

        if(Input.GetKeyDown(KeyCode.C)) {
            if(horizontalInput != 0) {
                isDashing = true;
                StartCoroutine(Dash(horizontalInput));
            }

            lastKeyCode = KeyCode.C;
        }
    }
}
