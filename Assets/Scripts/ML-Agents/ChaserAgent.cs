using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ChaserAgent : Agent
{
    public float forceMultiplier = 10;
    Rigidbody rBody;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = initialPosition;
        }

        // Move the target to a new spot
        //Target.localPosition = new Vector3(Random.value * 50,
        //                                   0.5f,
        //                                   Random.value * 50);

        Vector3 navmeshPos;
        // Get a new random location on the NavMesh
        Utils.RandomPointOnNavMesh(new Vector3(
            Random.value * 50,
            0.0f,
            Random.value * 50), 2,
            out navmeshPos);
        Target.localPosition = navmeshPos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Target")
        {
            // Rewards
            //float distanceToTarget = Vector3.Distance(this.transform.localPosition, other.gameObject.transform.localPosition);

            SetReward(1.0f);
            EndEpisode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
}
