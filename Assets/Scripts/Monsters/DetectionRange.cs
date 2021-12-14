using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public bool playerDetected = false;
    
    private void OnTriggerStay2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            playerDetected = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Player"))
        {
            playerDetected = false;
        }   
    }
}
