using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAproachingToRest : IState
{
    Grandfather selfGrandfather;

    List<Transform> currentWaypoints = new List<Transform>();

    public StateAproachingToRest(Grandfather grandfather)
    {
        selfGrandfather = grandfather;
    }

    public void EnterState()
    {
        // Gather the 4 positions necessary to calculate the smoothinig curve

        // If grandfather has not yet reached the first destination
        if (selfGrandfather.currentWaypoints.Count == 2)
        {
            //currentWaypoints.Add(selfGrandfather.currentWaypoints[0]);
            //currentWaypoints.Add(selfGrandfather.currentWaypoints[0]);
            currentWaypoints.Add(selfGrandfather.transform);
            currentWaypoints.Add(selfGrandfather.transform);
        }
        else
        {
            currentWaypoints.Add(selfGrandfather.currentWaypoints[0]);
            currentWaypoints.Add(selfGrandfather.currentWaypoints[1]);
        }

        // Rest position as both destination and next postiion for the algorithm
        currentWaypoints.Add(selfGrandfather.RestPosition);
        currentWaypoints.Add(selfGrandfather.RestPosition);

        selfGrandfather.currentWaypoints = currentWaypoints;

        selfGrandfather.NavMeshAgent.destination = selfGrandfather.RestPosition.transform.position;

        selfGrandfather.ResetMove();
    }

    public void UpdateState()
    {
        // When rest point reached
        if (Vector3.Distance(selfGrandfather.transform.position, selfGrandfather.RestPosition.transform.position) < 1)
        {
            selfGrandfather.ChangeToState(selfGrandfather.StateIdle);
        }
        else
            selfGrandfather.Move();
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
