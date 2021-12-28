using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private int clockwise = 1;

    private Animator ani;
    private Rigidbody2D rd;

    void Awake() {
        rd = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    void FixedUpdate() {
        ani.SetBool("goleft", clockwise == -1);
        ani.SetBool("goright", clockwise == 1);
        if(!rd) return;

        Vector2 position = rd.position;

        if(clockwise == 1) {
            rd.position += Vector2.left * speed * Time.fixedDeltaTime;
            if(transform.localScale.x == -1) {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        } else {
            rd.position += Vector2.right * speed * Time.fixedDeltaTime;
            if(transform.localScale.x == 1) {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
        rd.MovePosition(position);
    }
}
