using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkRadius;

    [Header("Physic variable")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float xWallJumpForce;
    [SerializeField] private float dashDistance = 5f;

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
    private bool isHurting = false;

    private float horizontalInput;
    private float doubleTapTime;
    private float undamageTime = 2f;
    private float undamageCoolDown;

    private void Awake() {
        knight_rb = transform.GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);

        if(knight_ani){
            knight_ani.SetBool("isGround", isGround);
            knight_ani.SetFloat("yVelocity", knight_rb.velocity.y);
            knight_ani.SetBool("walk", horizontalInput != 0);
            knight_ani.SetBool("attack", isAttack);
            knight_ani.SetBool("dash", isDashing);
        }

        if(isWall == true && isGround == false && (horizontalInput <= -1 || horizontalInput >= 1)) {
            wallSliding = true;
            isWallJumpOver = false;
        } else {
            wallSliding = false;
            isWallJumpOver = true;
        }

        if(isWallJumpOver == true && isDashing == false) {
            MoveHandle();
        }

        if(undamageCoolDown > undamageTime) {
            isHurting = false;
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

    public void StartUndamageTime() {
        undamageCoolDown += Time.deltaTime;
    }

    public void SetIsHurting(bool var) {
        isHurting = var;
    }

    public bool GetIsHurting() {
        return isHurting;
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

    public bool GetIsWall() {
        return isWall;
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

        if(isHurting = true) {
            StartUndamageTime();
        }

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
        }
    }
}
