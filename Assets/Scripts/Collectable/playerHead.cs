using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHead : MonoBehaviour
{
    [SerializeField] private AudioSource collectSound;
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            //Life.lifesCount += 1;
            LevelManager.instance.lifeCount ++;
            collectSound.Play();
            Destroy(gameObject);
        }
    }
}
