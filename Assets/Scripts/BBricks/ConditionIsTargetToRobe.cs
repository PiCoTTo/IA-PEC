using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsThereTargetToRobe")]
public class ConditionIsTargetToRobe : BBUnity.Conditions.GOCondition
{
    [InParam("MaximumDistanceToSearch")]
    public float MaxDistance;

    public override bool Check()
    {
        Thief thiefParams = gameObject.GetComponent<Thief>();

        // Search for a potential target nearby
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("GoodCitizen");

        foreach (GameObject potentialTarget in potentialTargets)
        {
            if (Vector3.Distance(potentialTarget.transform.position, gameObject.transform.position) < MaxDistance && potentialTarget != thiefParams.TargetJustRobbed)
                return true;
        }

        return false;
    }
}
