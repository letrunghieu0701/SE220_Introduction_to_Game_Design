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
    private Vector3 respawnPoint;

    private bool isGround;
    private bool isWall;
    private bool isAttack;
    private bool wallSliding;
    private bool wallJumping;
    private bool isDead;
    private bool facingRight = true;
    private bool canJump;
    private bool falling;
    private bool isWallJumpOver;
    private bool isDashing;
    private bool isHurting = false;
    private bool stopHorizontal;
    private bool jumpAttack;

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
            knight_ani.SetBool("walk", horizontalInput != 0);
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

        if(isWallJumpOver == true && isDashing == false) {
            MoveHandle();
        }
    }

    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");
        undamageCoolDown += Time.deltaTime;

        KeyHandle();
        HandleOnAir();

        if(undamageCoolDown > undamageTime) {
            isHurting = false;
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
        if(isDead == false) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
            if(Input.GetKeyDown(KeyCode.X)) {
                Attack();
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

    public void SetIsHurting(bool var) {
        undamageCoolDown = 0;
        isHurting = var;
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

    void setWallJumpToFalse() {
        wallJumping = false;
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
        yield return new WaitForSeconds(0.5f);
        stopHorizontal = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "CheckPoint") {
            levelManager.respawnPoint = transform.position;
        }
    }
}
