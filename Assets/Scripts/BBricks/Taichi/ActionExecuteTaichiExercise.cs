using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using UnityEngine.AI;


[Action("Taichi/Execute Taichi Exercise")]
public class ActionExecuteTaichiExercise : BBUnity.Actions.GOAction
{
    [InParam("ExerciseSelected")]
    public int ExerciseSelected;

    [OutParam("IsExerciseSelected")]
    public bool IsExerciseSelected;

    NavMeshAgent selfNavMesh;
    List<Vector3> exercise;
    int currentStep;
    Vector3 origin;

    public override void OnStart()
    {
        selfNavMesh = gameObject.GetComponent<NavMeshAgent>();
        selfNavMesh.updateRotation = false;
        selfNavMesh.angularSpeed = 0;
        currentStep = 0;

        TaichiMaster tMaster = gameObject.GetComponent<TaichiMaster>();
        exercise = tMaster.Exercises[ExerciseSelected];
        origin = tMaster.OriginPos;
        Debug.Log("Taichi Master begining exercise #" + ExerciseSelected);

        // First step
        //Debug.Log("Taichi Master step " + currentStep + " going to " + selfNavMesh.destination);
        selfNavMesh.destination = origin + exercise[currentStep++];

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        TaichiMaster tMaster = gameObject.GetComponent<TaichiMaster>();

        if (selfNavMesh.pathPending)
            return TaskStatus.RUNNING;

        if (selfNavMesh.remainingDistance > selfNavMesh.stoppingDistance)
        {
            return TaskStatus.RUNNING;
        }
        else
        {
            if(currentStep == exercise.Count-1)
            {
                IsExerciseSelected = false;
                return TaskStatus.COMPLETED;
            }

            // Next step
            //Debug.Log("Taichi Master step " + currentStep + " going to " + selfNavMesh.destination);
            selfNavMesh.destination = origin + exercise[currentStep++];
            return TaskStatus.RUNNING;
        }
    }
}
