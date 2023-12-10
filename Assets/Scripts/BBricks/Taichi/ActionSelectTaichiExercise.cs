using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Taichi/Select Taichi Exercise")]
public class ActionSelectTaichiExercise : BBUnity.Actions.GOAction
{
    [OutParam("ExerciseSelected")]
    public int ExerciseSelected;

    [OutParam("IsExerciseSelected")]
    public bool IsExerciseSelected;

    public override TaskStatus OnUpdate()
    {
        TaichiMaster tMaster = gameObject.GetComponent<TaichiMaster>();

        if (tMaster.Exercises.Count > 0)
        {
            //Random.seed = System.DateTime.Now.Millisecond;
            ExerciseSelected = Random.Range(0, tMaster.Exercises.Count);
            //ExerciseSelected = 1;
            IsExerciseSelected = true;
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.FAILED;
    }
}
