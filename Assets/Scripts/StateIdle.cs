using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    Grandfather selfGrandfather;
    float remainingRestTime;

    public StateIdle(Grandfather grandfather)
    {
        selfGrandfather = grandfather;
    }

    public void EnterState()
    {
        remainingRestTime = selfGrandfather.RestTime;
    }

    public void UpdateState()
    {
        remainingRestTime -= Time.deltaTime;

        if(remainingRestTime <= 0)
        {
            selfGrandfather.NavMeshAgent.stoppingDistance = 1.0f;
            selfGrandfather.ChangeToState(selfGrandfather.StateWander);
        }
    }

    public void Impact()
    {
    }

    public void OnTriggerEnter(Collider col)
    {
    }

    public void OnTriggerStay(Collider col)
    {
    }

    public void OnTriggerExit(Collider col)
    {
    }
}
