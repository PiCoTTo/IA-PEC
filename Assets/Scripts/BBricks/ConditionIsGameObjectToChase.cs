using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsGameObjectToChase")]
public class IsGameObjectToChase : BBUnity.Conditions.GOCondition
{
    [InParam("MaximumDistanceToSearch")]
    public float MaxDistance;

    public override bool Check()
    {
        // Search for a potential target nearby
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Thief");

        foreach (GameObject obj in potentialTargets)
            if (Vector3.Distance(obj.transform.position, gameObject.transform.position) < MaxDistance)
            {
                Thief thiefParams = obj.GetComponent<Thief>();

                if (thiefParams.IsRobbing)
                    return true;
            }

        return false;
    }
}
