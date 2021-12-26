using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHead : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            Life.lifesCount += 1;
            Destroy(gameObject);
        }
    }
}
