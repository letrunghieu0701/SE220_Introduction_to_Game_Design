using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneyControl : MonoBehaviour
{
    [SerializeField] private GameObject smoke;
    [SerializeField] private List<GameObject> smokePosition;
    public float delayBetweenEachSmoke = 0.3f;
    
    private State nextSmokeCountDown;
    public bool isSmoking = false;

    
    // Start is called before the first frame update
    void Start()
    {
        nextSmokeCountDown = new State("delay between each smoke", delayBetweenEachSmoke);
    }

    private void FixedUpdate() 
    {
        if (isSmoking)
        {
            nextSmokeCountDown.CountDown();
            if (nextSmokeCountDown.timeSup)
            {
                Instantiate(smoke, smokePosition[Random.Range(0, smokePosition.Count)].transform.position, Quaternion.identity);
                nextSmokeCountDown.ResetState();
            }
        }
    }
}
