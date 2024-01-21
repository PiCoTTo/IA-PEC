using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ChaserAgent : Agent
{
    public float forceMultiplier = 10;
    Rigidbody rBody;
    Vector3 initialPosition;
    int inTrigger = 0;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        // Saves the initial position
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
        sensor.AddObservation(Target.position);
        sensor.AddObservation(this.transform.position);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        SetReward(-1f / MaxStep);

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var direction = Vector3.zero;
        var rotation = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 1:
                direction = transform.forward * 1f;
                break;
            case 2:
                direction = transform.forward * -1f;
                break;
            case 3:
                rotation = transform.up * 1f;
                break;
            case 4:
                rotation = transform.up * -1f;
                break;
            case 5:
                direction = transform.right * -0.75f;
                break;
            case 6:
                direction = transform.right * 0.75f;
                break;
        }

        transform.Rotate(rotation, Time.fixedDeltaTime * 200f);
        rBody.AddForce(direction * 1, ForceMode.VelocityChange);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;

        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prevents triggers from being permanent called
        inTrigger++;
        if (other.gameObject.name == "Target")
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if(other.transform.parent.tag == "Obstacle")
        {
            SetReward(-1.0f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Only for the event that the target spawns over the agent
        if (inTrigger > 0)
            return;
        OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger--;
    }
}
