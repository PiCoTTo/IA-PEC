using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Framework;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;


[Action("Select Target to Robe")]
public class ActionSelectTargetToRobe : BBUnity.Actions.GOAction
{
    [InParam("MaximumDistanceToSearch")]
    public float MaxDistance;

    [InParam("Delay between searches", DefaultValue = 120)]
    public int delay;

    [OutParam("Target to be robbed")]
    GameObject target;

    [OutParam("IsTargetSelected")]
    public bool TargetSelected;

    [OutParam("IsTargetReleased")]
    public bool TargetReleased;

    [OutParam("ResetTimer")]
    public bool ResetTimer;

    public override TaskStatus OnUpdate()
    {
        Thief thiefParams = gameObject.GetComponent<Thief>();
        thiefParams.TargetJustRobbed = null;

        TargetReleased = false;

        // Search for a potential target nearby
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("GoodCitizen");

        if (potentialTargets.Length > 0)
        {
            int targetIndex = Random.Range(0, potentialTargets.Length - 1);

            foreach(GameObject obj in potentialTargets)
                if (Vector3.Distance(obj.transform.position, gameObject.transform.position) < MaxDistance)
                {
                    target = potentialTargets[targetIndex];
                    TargetSelected = true;
                    ResetTimer = true;
                }

            return TaskStatus.COMPLETED;
        }

        return TaskStatus.FAILED;
    }
}
