using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnTriggerStay2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Ground"))
        {
            isGrounded = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D collider2D) 
    {
        if (collider2D.CompareTag("Ground"))
        {
            isGrounded = false;
        }    
    }
}
