using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBoxScript : MonoBehaviour
{
    [SerializeField] private int damage;
    public static int SwordDamage = 0;
    private int HitBoxDamage;
    public void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Monster") {
            col.GetComponent<Monster>().TakeDamage(HitBoxDamage);
        }
        if(col.tag == "BringerOfDeath") {
            col.GetComponent<Bringer_death>().TakeDamage(HitBoxDamage);
        }
    }

    private void Update() {
        if(SwordDamage != 0) {
            HitBoxDamage = SwordDamage;
        } else {
            HitBoxDamage = damage;
        }
    }
}
