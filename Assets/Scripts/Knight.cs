using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D knight_rb;
    Animator knight_ani;
    GamePadsController gamePadsController;
    
    bool isDead;

    private void Awake(){
        knight_rb = GetComponent<Rigidbody2D>();
        knight_ani = GetComponent<Animator>();
    }

    private void FixedUpdate(){
        MoveHandle();
    }

    void MoveHandle(){
        if(gamePadsController != null){
            if(gamePadsController.CanMoveLeft){
                if(knight_rb){
                    knight_rb.velocity = new Vector2(-1, knight_rb.velocity.y) * moveSpeed;
                }

                if(knight_ani){
                    knight_ani.SetBool("walk", true);
                }
            }
            else if(gamePadsController.CanMoveRight){
                if(knight_rb){
                    knight_rb.velocity = new Vector2(1, knight_rb.velocity.y) * moveSpeed;
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
        
    }

    void Start()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<GamePadsController>();
        gamePadsController = obj.GetComponent<GamePadsController>();
    }

    void Update()
    {
    }
}
