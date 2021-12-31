using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator ani;
    private float workingTime = 1f;
    private float stopWorkingTime = 3f;
    private float delayTime = 3.5f;
    private float timeCoolDown;

    private void Awake() {
        ani = GetComponent<Animator>();
    }

    private void Update() {
        timeCoolDown += Time.deltaTime;

        if(timeCoolDown > workingTime) {
            ani.SetBool("work", true);
        } 
        if(timeCoolDown > stopWorkingTime) {
            ani.SetBool("stopwork", true);
            ani.SetBool("work", false);
        } 
        if(timeCoolDown > delayTime) {
            ani.SetBool("stopwork", false);
            timeCoolDown = 0;
        }
    }
}
