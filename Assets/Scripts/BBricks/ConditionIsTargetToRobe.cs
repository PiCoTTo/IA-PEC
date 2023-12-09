using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsThereTargetToRobe")]
public class ConditionIsTargetToRobe : BBUnity.Conditions.GOCondition
{
    //[InParam("Target Selected to Robe")]
    //public GameObject target;

    [InParam("MaximumDistanceToSearch")]
    public float MaxDistance;

    //[InParam("DelayBetweenSearches", DefaultValue = 360)]
    //public int delay;

    //[InParam("IsTargetSelected")]
    //public bool TargetSelected;

    //[InParam("IsTargetReleased")]
    //public bool TargetReleased;

    //[InParam("ResetTimer")]
    //public bool ResetTimer;

    public override bool Check()
    {
        //if (TargetSelected && !TargetReleased)
        //    return false;
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
