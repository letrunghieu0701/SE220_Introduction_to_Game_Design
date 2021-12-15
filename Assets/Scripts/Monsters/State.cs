using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public bool timeSup = false;
    public string stateName;
    public float time;
    public float timer;

    public State (string _stateName, float _time)
    {
        stateName = _stateName;
        time = _time;
    }

    private void Awake() {
        timer = time;
    }

    public void CountDown()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            timeSup = true;
        }
    }

    public void ResetState()
    {
        timer = time;
        timeSup = false;
    }

}
