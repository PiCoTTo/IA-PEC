using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Chase Game Object")]
public class ActionChaseGameObject : BBUnity.Actions.GOAction
{
    [InParam("Game Object to Chase")]
    public GameObject gameObjectToChase;

    [InParam("ChaserColor")]
    public int chaserColor;

    NavMeshAgent navMesh;

    public override void OnStart()
    {
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        navMesh.destination = gameObjectToChase.transform.position;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        switch (chaserColor)
        {
            case 0:
                renderer.material.color = Color.black;
                break;
            case 1:
                renderer.material.color = Color.blue;
                break;
            case 2:
                renderer.material.color = Color.red;
                break;
            default:
                renderer.material.color = Color.gray;
                break;
        }


        navMesh.speed = 7;

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (navMesh.pathPending)
            return TaskStatus.RUNNING;

        if (navMesh.remainingDistance > navMesh.stoppingDistance)
        {
            navMesh.destination = gameObjectToChase.transform.position;
            return TaskStatus.RUNNING;
        }

        return TaskStatus.COMPLETED;
    }
}
