using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWander : IState
{
    Grandfather selfGrandfather;
        
    List<Transform> currentWaypoints = new List<Transform>();

    int     waypointCount       = 0;
    int     firstWaypointIdx    = 0;
    int     nextWaypoint        = 0;
    bool    firstMovement       = true;
    
    public StateWander(Grandfather grandfather)
    {
        selfGrandfather = grandfather;
    }

    public void EnterState()
    {
        if (firstMovement)
        {
            waypointCount = selfGrandfather.Waypoints.Count;
            firstWaypointIdx = Random.Range(0, waypointCount);
            selfGrandfather.NavMeshAgent.speed = Random.Range(3.5f, 4.0f);

            // Go through circular buffer of waypoints
            nextWaypoint = (firstWaypointIdx + 1) % selfGrandfather.Waypoints.Count;
            selfGrandfather.NextWaypoint = nextWaypoint;
            firstMovement = false;
        }
        else
        {
            firstWaypointIdx = (nextWaypoint + 1) % selfGrandfather.Waypoints.Count;
            //nextWaypoint = (firstWaypointIdx + 1) % selfGrandfather.Waypoints.Count;
            selfGrandfather.NextWaypoint = firstWaypointIdx;
        }

        // Register current position
        selfGrandfather.currentWaypoints.Clear();
        selfGrandfather.currentWaypoints.Add(selfGrandfather.transform);
        selfGrandfather.currentWaypoints.Add(selfGrandfather.Waypoints[firstWaypointIdx].transform);

        selfGrandfather.ResetMove();
    }


    public void UpdateState()
    {
        selfGrandfather.Move();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactive")
            selfGrandfather.RestPosition = other.transform.GetChild(0);

        // Change state
        nextWaypoint = selfGrandfather.NextWaypoint;
        selfGrandfather.NavMeshAgent.stoppingDistance = 0.0f;
        selfGrandfather.ChangeToState(selfGrandfather.StateAproachingToRest);
    }
}
