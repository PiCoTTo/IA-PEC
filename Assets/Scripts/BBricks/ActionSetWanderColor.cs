using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Set Wander Color")]
public class ActionSetWanderColor : BBUnity.Actions.GOAction
{
    [InParam("WanderColor")]
    public int wanderColor;

    public override void OnStart()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        switch (wanderColor)
        {
            case 0:
                renderer.material.color = Color.gray;
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

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }
}
