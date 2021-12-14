using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFrontGround : MonoBehaviour
{
    public bool frontGround = true;
    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if(collider2D.CompareTag("Ground"))
        {
            frontGround = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collider2D) 
    {
        if(collider2D.CompareTag("Ground"))
        {
            frontGround = false;
        }     
    }
}
