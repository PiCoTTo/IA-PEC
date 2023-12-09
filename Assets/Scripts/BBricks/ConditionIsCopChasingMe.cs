using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("ConditionIsCopChasingMe")]
public class ConditionIsCopChasingMe : BBUnity.Conditions.GOCondition
{
    [InParam("ChasingCop")]
    public GameObject ChasingCop;

    public override bool Check()
    {
        Thief thief = gameObject.GetComponent<Thief>();

        if (thief.ChasingCop != null)
            return true;

        return false;
    }
}
