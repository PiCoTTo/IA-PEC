using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Framework;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Move to XZ Position")]
public class ActionMoveToXZPosition : BBUnity.Actions.GOAction
{
    [InParam("point")]
    Vector3 point;

    NavMeshAgent navMesh;

    public override void OnStart()
    {
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        navMesh.destination = point;

        navMesh.speed = 3.5f;

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (navMesh.pathPending)
        {
            //Debug.Log("Move to XY OnUpdate(): waiting for the path to become available");
            return TaskStatus.RUNNING;
        }

        //Debug.Log("Move to XY OnUpdate(): remainingDistance = " + navMesh.remainingDistance);

        if (navMesh.remainingDistance > navMesh.stoppingDistance)
        {
            //Debug.Log("Move to XY OnUpdate(): destination not reached");
            return TaskStatus.RUNNING;
        }

        //Debug.Log("Move to XY OnUpdate(): destination reached !!!");
        return TaskStatus.COMPLETED;
    }
}
