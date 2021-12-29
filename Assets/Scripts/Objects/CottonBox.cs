using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonBox : MonoBehaviour
{
    private Animator ani;
    private int health;

    private void Awake() {
        ani = GetComponent<Animator>();
        health = 2;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Sword") {
            health--;
            if(health == 1) {
                ani.SetBool("break", true);
            } else if(health == 0) {
                ani.SetBool("nouse", true);
            }
        }
    }
}
