using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsGameObjectReached")]
public class IsGameObjectReached : BBUnity.Conditions.GOCondition
{
    [InParam("ChasedTarget")]
    public GameObject ChasedTarget;

    public override bool Check()
    {
        if (ChasedTarget == null)
            return false;

        bool stopCondition = false;
        if (gameObject.tag == "Thief")
        {
            Thief thiefParams = gameObject.GetComponent<Thief>();
            stopCondition = thiefParams.IsRobbing;
        }

        if (Vector3.Distance(ChasedTarget.transform.position, gameObject.transform.position) < 2 || stopCondition)
            return true;

        return false;
    }
}
