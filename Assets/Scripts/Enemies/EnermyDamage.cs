using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            col.GetComponent<Health>().TakeDamage(damage, transform.position);
        }
    }
}
