using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grandfather : MonoBehaviour
{
    [HideInInspector] public List<Transform> Waypoints        = new List<Transform>();
    [HideInInspector] public List<Transform> currentWaypoints = new List<Transform>();
    List<Vector2>   newPoints        = new List<Vector2>();

    public float amountOfPoints = 3.0f;

    public float alpha = 0.5f;

    // Seconds
    public float RestTime = 3;

    int waypointCount    = 0;
    [HideInInspector] public int nextWaypoint     = 0;
    int currentNewPoint  = 0;

    [HideInInspector] public NavMeshAgent NavMeshAgent;

    [HideInInspector] public StateWander StateWander;
    [HideInInspector] public StateIdle StateIdle;
    [HideInInspector] public StateAproachingToRest StateAproachingToRest;
    [HideInInspector] public IState CurrentState;

    [HideInInspector] public Transform RestPosition;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        // Gather all secuential waypoints within Runner object
        var gatheredWaypoints = transform.parent.Find("Waypoints");
        foreach (Transform waypoint in gatheredWaypoints)
            Waypoints.Add(waypoint);

        // Random direction of the path
        if (Random.Range(0.0f, 1.0f) > 0.5f)
            Waypoints.Reverse();

        StateWander = new StateWander(this);
        StateIdle = new StateIdle(this);
        StateAproachingToRest = new StateAproachingToRest(this);

        ChangeToState(StateWander);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.UpdateState();
    }

    public void ChangeToState(IState state)
    {
        CurrentState = state;
        CurrentState.EnterState();
    }

    public void ResetMove()
    {
        waypointCount = Waypoints.Count;
        currentNewPoint = (int)amountOfPoints;
        newPoints.Clear();
    }

    public void Move()
    {
        if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance && waypointCount > 3)
        {
            if (currentNewPoint == (int)amountOfPoints)
            {
                newPoints.Clear();

                int wp;
                // If the first waypoint has just been reached
                if (currentWaypoints.Count < 4)
                {
                    wp = (nextWaypoint + 1) % waypointCount;
                    currentWaypoints.Add(Waypoints[wp].transform);
                    wp = (nextWaypoint + 2) % waypointCount;
                    currentWaypoints.Add(Waypoints[wp].transform);
                }
                else
                {
                    currentWaypoints.Clear();
                    // Go through circular buffer of waypoints
                    nextWaypoint = (nextWaypoint + 1) % Waypoints.Count;
                    wp = nextWaypoint == 0 ? waypointCount - 1 : nextWaypoint % waypointCount - 1;
                    currentWaypoints.Add(Waypoints[wp]);
                    wp = nextWaypoint % waypointCount;
                    currentWaypoints.Add(Waypoints[wp]);
                    wp = (nextWaypoint + 1) % waypointCount;
                    currentWaypoints.Add(Waypoints[wp]);
                    wp = (nextWaypoint + 2) % waypointCount;
                    currentWaypoints.Add(Waypoints[wp]);
                }

                Vector2 p0 = new Vector2(currentWaypoints[0].transform.position.x, currentWaypoints[0].transform.position.z);
                Vector2 p1 = new Vector2(currentWaypoints[1].transform.position.x, currentWaypoints[1].transform.position.z);
                Vector2 p2 = new Vector2(currentWaypoints[2].transform.position.x, currentWaypoints[2].transform.position.z);
                Vector2 p3 = new Vector2(currentWaypoints[3].transform.position.x, currentWaypoints[3].transform.position.z);
                float t0 = 0.0f;
                float t1 = GetT(t0, p0, p1);
                float t2 = GetT(t1, p1, p2);

                float t3 = GetT(t2, p2, p3);
                for (float t = t1; t < t2; t += ((t2 - t1) / amountOfPoints))
                {
                    Vector2 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
                    Vector2 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
                    Vector2 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;
                    Vector2 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
                    Vector2 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;
                    Vector2 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;
                    newPoints.Add(C);
                }

                currentNewPoint = 0;
            }

            // When there are configured waypoints
            if (newPoints.Count > 0)
            {
                //// Go through circular buffer of waypoints
                //nextWaypoint = (nextWaypoint + 1) % newPoints.Count;

                NavMeshAgent.destination = new Vector3(newPoints[currentNewPoint].x, transform.position.y, newPoints[currentNewPoint].y);
                currentNewPoint++;
            }
        }
    }

    float GetT(float t, Vector2 p0, Vector2 p1)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, alpha);
        return (c + t);
    }

    //Visualize the points
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 temp in newPoints)
        {
            Vector3 pos = new Vector3(temp.x, 0, temp.y);
            Gizmos.DrawSphere(pos, 0.3f);
        }

        if (currentWaypoints.Count == 4)
        {
            Gizmos.color = Color.blue;
            for (int i = 1; i <= 2; i++)
            {
                Vector3 pos = new Vector3(currentWaypoints[i].position.x, 0, currentWaypoints[i].position.z);
                Gizmos.DrawSphere(pos, 0.3f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
}
