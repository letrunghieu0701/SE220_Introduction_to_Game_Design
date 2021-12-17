using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleCheck : MonoBehaviour
{
    public bool bossBattleCheck = false;

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            bossBattleCheck = true;
        }    
    }
}
