using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBringer : MonoBehaviour
{
    [SerializeField] private int damage;
    public void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            col.GetComponent<Health>().TakeDamage(damage, transform.position);
        }
    }
}
