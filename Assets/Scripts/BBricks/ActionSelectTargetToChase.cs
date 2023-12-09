using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Framework;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;


[Action("Select Target to Catch")]
public class ActionSelectTargetToChase : BBUnity.Actions.GOAction
{
    [InParam("MaximumDistanceToSearch")]
    public float MaxDistance;

    [OutParam("TargetToCatch")]
    GameObject TargetToChase;

    //[OutParam("ChasingCop")]
    //public GameObject ChasingCop;

    public override TaskStatus OnUpdate()
    {
        // Search for a potential target nearby
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Thief");

        if (potentialTargets.Length > 0)
        {
            //int targetIndex = Random.Range(0, potentialTargets.Length - 1);

            foreach (GameObject obj in potentialTargets)
                if (Vector3.Distance(obj.transform.position, gameObject.transform.position) < MaxDistance)
                {
                    Thief thiefParams = obj.GetComponent<Thief>();

                    bool TargetSelected = false;
                    if (thiefParams.IsRobbing)
                        // Probability to catch the thief robbing
                        TargetSelected = Random.Range(0, 100) < 50;

                    if (TargetSelected)
                    {
                        TargetToChase = obj;
                        thiefParams.ChasingCop = gameObject;
                    }
                    return TaskStatus.COMPLETED;
                }
        }

        return TaskStatus.FAILED;
    }
}
