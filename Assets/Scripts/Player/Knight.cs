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
    [SerializeField] private float knockBackDistance = 0.5f;
    [SerializeField] private float knockBackForce = 0.5f;

    [Header("Timer")]
    [SerializeField] private float wallJumpCoolDown;
    [SerializeField] private float stopHorizontalTime = 0.5f;
    [SerializeField] private float stopAttackTime = 0.3f;

    Rigidbody2D knight_rb;
    Animator knight_ani;
    private BoxCollider2D boxCollider2D;
    private KeyCode lastKeyCode;
    private Vector3 respawnPoint;

    private bool isGround;
    private bool isWall;
    private bool isAttack;
    private bool wallSliding;
    private bool wallJumping;
    private bool isDead;
    private bool facingRight = true;
    private bool canJump;
    private bool canMove;
    private bool falling;
    private bool isWallJumpOver;
    private bool isDashing;
    private bool isHurting = false;
    private bool stopHorizontal = false;
    private bool stopAttack = false;
    private bool jumpAttack;
    private bool knockBack = false;

    private float horizontalInput;
    private float doubleTapTime;
    private float undamageTime = 1f;
    private float undamageCoolDown;
    private float gravity;
    private LevelManager levelManager;

    private void Awake() {
        knight_rb = transform.GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        respawnPoint = transform.position;
        gravity = knight_rb.gravityScale;
        isDead = false;
    }

    private void Start() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        transform.position = levelManager.respawnPoint;
    }

    private void FixedUpdate() {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);

        if(knight_ani){
            knight_ani.SetBool("isGround", isGround);
            knight_ani.SetFloat("yVelocity", knight_rb.velocity.y);
            knight_ani.SetBool("walk", canMove);
            knight_ani.SetBool("attack", isAttack);
            knight_ani.SetBool("dash", isDashing);
            knight_ani.SetBool("jumpAttack", jumpAttack);
            knight_ani.SetBool("jump", canJump);
            knight_ani.SetBool("fall", falling);
        }

        if(isWall == true && isGround == false && (horizontalInput <= -1 || horizontalInput >= 1)) {
            wallSliding = true;
            isWallJumpOver = false;
        } else {
            wallSliding = false;
            isWallJumpOver = true;
        }

        if(isWallJumpOver == true && isDashing == false && isHurting == false) {
            MoveHandle();
        }
    }

    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");
        undamageCoolDown += Time.deltaTime;

        KeyHandle();
        HandleOnAir();

        if(horizontalInput != 0) {
            canMove = true;
        } else {
            canMove = false;
        }

        if(wallSliding == true) {
            knight_rb.velocity = new Vector2(knight_rb.velocity.x, Mathf.Clamp(knight_rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if(wallJumping) {
            if(stopHorizontal == false) {
                knight_rb.velocity = new Vector2(xWallJumpForce * -horizontalInput, jumpForce);
                StartCoroutine(StopHorizontal());
            }
        }

        if(isDashing == true && isWall == true) {
            knight_rb.gravityScale = gravity;
            isDashing = false;
        }
    }

    private void KeyHandle() {
        if(isDead == false && isHurting == false) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
            if(Input.GetKeyDown(KeyCode.X)) {
                if(stopAttack == false) {
                    Attack();
                    StartCoroutine(StopAttack());
                }
            }
            if(Input.GetKeyDown(KeyCode.Z)) {
                if(horizontalInput != 0) {
                    isDashing = true;
                    StartCoroutine(Dash(horizontalInput));
                }
            }
            if(Input.GetKeyDown(KeyCode.R)) {
                transform.position = respawnPoint;
            }
        }
    }

    void MoveHandle() {
        
        if(Input.GetKey(KeyCode.LeftArrow)) {
            if(!knight_ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                knight_rb.velocity = new Vector2(moveSpeed * horizontalInput, knight_rb.velocity.y);
            }
            if(facingRight) {
                Flip();
            }
            
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            if(!knight_ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
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

    public void SetIsDead(bool var) {
        isDead = var;
    }

    public bool GetIsDead() {
        return isDead;
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
        if (isGround == true && isDashing == false) {
            knight_rb.velocity = Vector2.up * jumpForce;
            canJump = true;
        } else if(wallSliding == true) {
            wallJumping = true;
            Invoke("setWallJumpToFalse", wallJumpCoolDown);
        }
    }

    private void HandleOnAir() {
        if(!isGround) {
            knight_ani.SetLayerWeight(1, 1);
        } else {
            knight_ani.SetLayerWeight(1, 0);
            falling = false;
        }

        if(knight_rb.velocity.y < 0) {
            falling = true;
            canJump = false;
        }
    }

    void Attack() {
        
        if(isWall == false && isGround == true && !this.knight_ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            isAttack = true;
        }

        if(isGround == false && !this.knight_ani.GetCurrentAnimatorStateInfo(1).IsName("jumpattack")) {
            jumpAttack = true;
        } else {
            jumpAttack = false;
        }
    }

    public bool GetIsWall() {
        return isWall;
    }

    void setWallJumpToFalse() {
        wallJumping = false;
    }

    void setAttackToFalse() {
        isAttack = false;
    }

    void setJumpAttackToFalse() {
        jumpAttack = false;
    }

    public void Dead() {
        if(knight_ani){
            knight_ani.SetTrigger("Die");
        }

        if(knight_rb){
            knight_rb.velocity = new Vector2(0f, 0f);
        }
    }

    public bool canShoot(){
        return isGround || isWall || (!isGround && !isWall);
    }

    public void DoKnockBack(float direction) {
        if(direction == transform.localScale.x) {
            Flip();
        }
        StartCoroutine(HurtingTime(knockBackDistance));
        knight_rb.velocity = new Vector2(-transform.localScale.x * knockBackForce, knockBackForce);
    }

    private IEnumerator Dash(float dir) {
        if(isDashing == true) {
            knight_rb.gravityScale = 0;
            knight_rb.velocity = new Vector2(knight_rb.velocity.x, 0f);
            knight_rb.AddForce(new Vector2(dashDistance * dir, 0f), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.4f);
        knight_rb.gravityScale = gravity;
        isDashing = false;
    }

    private IEnumerator StopHorizontal() {
        stopHorizontal = true;
        yield return new WaitForSeconds(stopHorizontalTime);
        stopHorizontal = false;
    }

    private IEnumerator StopAttack() {
        stopAttack = true;
        yield return new WaitForSeconds(stopAttackTime);
        stopAttack = false;
    }

    private IEnumerator HurtingTime(float time) {
        isHurting = true;
        yield return new WaitForSeconds(time);
        isHurting = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "CheckPoint") {
            levelManager.respawnPoint = transform.position;
        }
    }
}
