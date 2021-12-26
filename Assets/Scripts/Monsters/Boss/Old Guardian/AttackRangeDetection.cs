using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDetection : MonoBehaviour
{
    public bool isInRange = false;

    private void OnTriggerStay2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    // private void OnTriggerExit2D(Collider2D collider2D) 
    // {
    //     if (collider2D.CompareTag("Player"))
    //     {
    //         isInRange = false;
    //     }
    // }
}
