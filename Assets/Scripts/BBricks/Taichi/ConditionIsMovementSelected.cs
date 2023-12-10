using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;


[Condition("Taichi/IsExerciseNotSelected")]
public class IsNotExerciseNotSelected : ConditionBase
{
    [InParam("IsExerciseSelected")]
    public bool IsExerciseSelected;

    public override bool Check()
    {
        if (IsExerciseSelected)
            return false;

        return true;
    }
}
