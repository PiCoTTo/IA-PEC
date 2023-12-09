using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("IsChasingGameObject")]
public class IsChasingGameObject : BBUnity.Conditions.GOCondition
{
    [InParam("ChasedTarget")]
    public GameObject ChasedTarget;

    public override bool Check()
    {
        if (ChasedTarget == null)
            return false;

        return true;
    }
}
