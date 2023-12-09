using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Run From Game Object")]
public class ActionRunFromGameObject : BBUnity.Actions.GOAction
{
    [InParam("terrain")]
    public Terrain terrain;

    [InParam("TargetToRobe")]
    GameObject targetToRobe;

    [OutParam("IsTargetSelected")]
    public bool TargetSelected;

    [OutParam("IsTargetReleased")]
    public bool TargetReleased;

    [OutParam("IsTargetRobbed")]
    public bool TargetRobbed;

    [OutParam("ReleasedGameObject")]
    public GameObject releasedGameObject;

    NavMeshAgent selfNavMesh;
    NavMeshAgent otherNavMesh;
    Thief thiefParams;

    public override void OnStart()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        thiefParams = gameObject.GetComponent<Thief>();

        selfNavMesh = gameObject.GetComponent<NavMeshAgent>();
        otherNavMesh = targetToRobe.GetComponent<NavMeshAgent>();
        selfNavMesh.isStopped = false;
        otherNavMesh.isStopped = false;
        if (gameObject.tag == "Thief")
        {
            thiefParams.TargetJustRobbed = targetToRobe;

            TargetReleased = false;
            TargetRobbed = true;
            if (thiefParams != null)
                thiefParams.IsRobbing = false;
        }

        // Free GameObject from being chased
        releasedGameObject = null;

        selfNavMesh.destination = getDestinationWithinTerrain();

        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = Color.yellow;

        selfNavMesh.speed = 5;

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        //if (navMesh.isStopped == false)
        //{
            if (selfNavMesh.pathPending)
                return TaskStatus.RUNNING;

            if (selfNavMesh.remainingDistance > selfNavMesh.stoppingDistance)
            {
                return TaskStatus.RUNNING;
            }
            else
            {
                selfNavMesh.destination = getDestinationWithinTerrain();
                return TaskStatus.RUNNING;
            }
        //}

        return TaskStatus.COMPLETED;
    }

    Vector3 getDestinationWithinTerrain()
    {
        Vector3 dir = (gameObject.transform.position - thiefParams.ChasingCop.transform.position);
        dir = Vector3.Normalize(dir);

        // Angle within which randomly modify the escape direction
        float fovAngle = 90;
        Vector3 newPos = gameObject.transform.position + Quaternion.Euler(0, Random.Range(-fovAngle/2, fovAngle/2), 0) * (dir * 5);

        return newPos;
    }
}
