using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioSource collectSound;

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            col.GetComponent<Health>().Heal(healthValue);
            collectSound.Play();
            gameObject.SetActive(false);
        }
    }
}
