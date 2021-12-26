using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedGem : MonoBehaviour
{
    [SerializeField] private AudioSource collectSound;
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            Score.gemAmount += 1;
            collectSound.Play();
            Destroy(gameObject);
        }
    }
}
