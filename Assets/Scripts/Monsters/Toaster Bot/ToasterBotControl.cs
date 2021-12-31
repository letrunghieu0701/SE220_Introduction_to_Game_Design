using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterBotControl : Monster
{
    [Header("Self:")]
    [SerializeField] private float nextBeamDelay = 1.0f;
    [SerializeField] private bool DestroyDelay = false;

    private State nextBeamDelayState;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        nextBeamDelayState = new State("next beam delay", nextBeamDelay);
        nextBeamDelayState.ResetState();
        nextBeamDelayState.timer = 0f;

        if (DestroyDelay)
        {
            DeleteGameObjDelay();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDead)
        {
            if (playerDetected)
            {
                nextBeamDelayState.CountDown();
                if (nextBeamDelayState.timeSup)
                {
                    animator.SetTrigger("attack");
                    nextBeamDelayState.ResetState();
                }
            }
            else
            {
                nextBeamDelayState.ResetState();
                nextBeamDelayState.timer = 0f;
            }
        }
    }
}
