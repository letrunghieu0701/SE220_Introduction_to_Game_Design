using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource collectGems;
    [SerializeField] private AudioSource collectHearts;
    [SerializeField] private AudioSource collectLifes;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Gem") {
            collectGems.Play();
        } else if(other.tag == "Heart") {
            collectHearts.Play();
        } else if(other.tag == "Life") {
            collectLifes.Play();
        }
    }
}
