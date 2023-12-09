using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsThereTargetToRobeSelected")]
public class ConditionIsTargetToRobeSelected : BBUnity.Conditions.GOCondition
{
    //[InParam("Target Selected to Robe")]
    //public GameObject target;

    [InParam("IsTargetSelected")]
    public bool TargetSelected;

    [InParam("IsTargetRobbed")]
    public bool TargetRobbed;

    //[InParam("Maximum Distance to Search")]
    //public float MaxDistance;

    //[InParam("Delay between searches", DefaultValue = 120)]
    //public int delay;

    //// Game loop cycles since last target selection
    //int elapsed = 0;

    public override bool Check()
    {
        if (TargetSelected && !TargetRobbed)
            return true;

        return false;
    }
}
