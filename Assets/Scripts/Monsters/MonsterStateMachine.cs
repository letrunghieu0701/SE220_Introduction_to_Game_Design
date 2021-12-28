using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public int stateOrder = 0;
    public List<State> states;

    public MonsterStateMachine(List<State> _states)
    {
        states = _states;
    }

    public void Run()
    {
        states[stateOrder].CountDown();

        if (states[stateOrder].timeSup)
        {
            states[stateOrder].ResetState();
            stateOrder++;
            if (stateOrder >= states.Count)
            {
                stateOrder = 0;
            }
        }
    }

    public void Reset()
    {
        stateOrder = 0;

        for (int i = 0; i < states.Count; i++)
        {
            states[i].ResetState();
        }
    }
    
    public void SkipState()
    {
        states[stateOrder].ResetState();
        stateOrder++;
        if (stateOrder >= states.Count)
        {
            stateOrder = 0;
        }
    }
}
