using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            Score.gemAmount += 1;
            Destroy(gameObject);
        }
    }
}
