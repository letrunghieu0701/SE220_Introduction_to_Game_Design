using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElectronicDoor : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator ani;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Player") {
            ani.SetBool("open", true);
            boxCollider.isTrigger = true;
        }
    }
}
