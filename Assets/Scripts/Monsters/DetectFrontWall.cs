using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFrontWall : MonoBehaviour
{
    public bool frontWall = false;
    private void OnTriggerEnter2D(Collider2D collider2D) 
    {
        if(collider2D.CompareTag("Ground"))
        {
            frontWall = true;
        }   
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Ground"))
        {
            frontWall = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collider2D) 
    {
        if(collider2D.CompareTag("Ground"))
        {
            frontWall = false;
        }     
    }
}
