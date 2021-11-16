using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    bool _canMoveLeft;
    bool _canMoveRight;
    Rigidbody2D knight_rb;
    Animator knight_ani;
    GamePadsController gamePadsController;
    bool isDead;
    bool facingRight = true;

    private bool isGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpValue;
    bool canJump;

    public bool CanMoveLeft { get => _canMoveLeft; set => _canMoveLeft = value; }
    public bool CanMoveRight { get => _canMoveRight; set => _canMoveRight = value; }

    private void Awake(){
        knight_rb = transform.GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
    }

    private void FixedUpdate(){
        if(isGround){
            if(knight_ani){
                knight_ani.SetBool("jump", false);
            }
        } else {
            if(knight_ani){
                knight_ani.SetBool("jump", true);
            }
        }

        MoveHandle();
    }

    void MoveHandle(){
        if(Input.GetKey(KeyCode.LeftArrow)){
            knight_rb.velocity = new Vector2(-moveSpeed, knight_rb.velocity.y);
            if(facingRight){
                Flip();
            }
            if(knight_ani){
                knight_ani.SetBool("walk", true);
            }
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            knight_rb.velocity = new Vector2(+moveSpeed, knight_rb.velocity.y);
            if(!facingRight){
                Flip();
            }
            if(knight_ani){
                knight_ani.SetBool("walk", true);
            }
        }
        else{
            if(knight_rb){
                knight_rb.velocity = new Vector2(0f, knight_rb.velocity.y);
            }
            if(knight_ani){
                knight_ani.SetBool("walk", false);
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private bool IsGround(){
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        return isGround;
    }

    void Dead(){
        if(knight_ani){
            knight_ani.SetTrigger("die");
            isDead = true;
        }

        if(knight_rb){
            knight_rb.velocity = new Vector2(0f, knight_rb.velocity.y);
        }
    }

    private void OnCollision2D(Collision2D col){
        if(col.gameObject.CompareTag("enemy")){
            if(isDead) return;
        }
    }

    void Start()
    {
        extraJumps = extraJumpValue;
    }

    void Update()
    {
        _canMoveLeft = Input.GetAxis("Horizontal") < 0;
        _canMoveRight = Input.GetAxis("Horizontal") > 0;

        if( IsGround() && Input.GetKeyDown(KeyCode.Space)){
            if(isGround){
                knight_rb.velocity = Vector2.up * jumpForce;
            }
        }
    }
}
