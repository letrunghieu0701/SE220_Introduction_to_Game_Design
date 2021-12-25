using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBoxScript : MonoBehaviour
{
    [SerializeField] private int damage;
    public void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Monster") {
            col.GetComponent<Monster>().TakeDamage(damage);
        }
        if(col.tag == "BringerOfDeath") {
            col.GetComponent<Bringer_death>().TakeDamage(damage);
        }
    }
}
