using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDrippingControl : MonoBehaviour
{
    [SerializeField] private GameObject lavaDrop;
    [SerializeField] private GameObject dripPos;
    [SerializeField] private float drippingDelay = 7.0f;

    [SerializeField] private float startDrippingDelay = 1.0f;

    private State dripDelayState;

    // Start is called before the first frame update
    void Start()
    {
        dripDelayState = new State("Drip delay", drippingDelay);
        dripDelayState.ResetState();
        dripDelayState.timer = startDrippingDelay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        dripDelayState.CountDown();
        if (dripDelayState.timeSup)
        {
            Instantiate(lavaDrop, dripPos.transform.position, Quaternion.identity);
            dripDelayState.ResetState();
        }
    }
}
