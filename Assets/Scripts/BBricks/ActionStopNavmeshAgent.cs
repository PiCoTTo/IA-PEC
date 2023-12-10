using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Stop Game Object")]
public class ActionStopNavmeshAgent : BBUnity.Actions.GOAction
{
    [InParam("GameObjectToStop")]
    public GameObject gameObjectToStop;

    [InParam("StopDuration", DefaultValue = 360)]
    public int StopDuration;

    [OutParam("IsTargetReleased")]
    public bool TargetReleased;

    [OutParam("IsTargetRobbed")]
    public bool TargetRobbed;

    [OutParam("ReleasedGameObject")]
    public GameObject releasedGameObject;

    NavMeshAgent selfNavMesh;
    NavMeshAgent otherNavMesh;
    Thief thiefParams = null;

    // Game loop cycles since last target selection
    int elapsed = 0;

    public override void OnStart()
    {
        selfNavMesh = gameObject.GetComponent<NavMeshAgent>();
        otherNavMesh = gameObjectToStop.GetComponent<NavMeshAgent>();
        elapsed = 0;

        if (gameObject.tag == "Thief")
        {
            thiefParams = gameObject.GetComponent<Thief>();
            thiefParams.IsRobbing = true;
        }
        else if (gameObject.tag == "Cop")
            thiefParams = gameObjectToStop.GetComponent<Thief>();

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        selfNavMesh.isStopped = true;
        otherNavMesh.isStopped = true;

        if (StopDuration > 0)
        {
            elapsed++;
            elapsed %= StopDuration;
            if (elapsed != 0)
                return TaskStatus.RUNNING;
        }

        selfNavMesh.isStopped = false;
        otherNavMesh.isStopped = false;

        if (gameObject.tag == "Thief")
        {
            thiefParams.TargetJustRobbed = gameObjectToStop;
            thiefParams.IsRobbing = false;
            TargetReleased = false;
            TargetRobbed = true;
        }
        if (gameObject.tag == "Cop")
        {
            thiefParams.TargetJustRobbed = gameObjectToStop;
            thiefParams.ChasingCop = null;
            thiefParams.IsRobbing = false;
            gameObjectToStop = null;
        }

        // Free GameObject from being chased
        releasedGameObject = null;

        return TaskStatus.COMPLETED;
    }
}
