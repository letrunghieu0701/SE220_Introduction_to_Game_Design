using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private float startHitDelay = 1.0f;
    private Animator ani;
    private State hammerDelayState;
    private float hitTimeCoolDown;

    private void Awake() {
        ani = GetComponent<Animator>();
        hammerDelayState = new State("Hammer delay", delayTime);
        hammerDelayState.ResetState();
        hammerDelayState.timer = startHitDelay;
    }

    private void FixedUpdate() {
        hammerDelayState.CountDown();
        if (hammerDelayState.timeSup)
        {
            ani.SetTrigger("Hit");
            hammerDelayState.ResetState();
        }
    }
}
