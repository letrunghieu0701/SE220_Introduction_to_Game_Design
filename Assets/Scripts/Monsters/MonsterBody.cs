using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBody : MonoBehaviour
{
    public int bodyDamage = 1;
    public bool damaged = false;

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            collider2D.GetComponent<Health>().TakeDamage(bodyDamage, transform.position);
            damaged = true;
        }    
    }

    private void OnTriggerStay2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            collider2D.GetComponent<Health>().TakeDamage(bodyDamage, transform.position);
            damaged = true;
        }    
    }
}
