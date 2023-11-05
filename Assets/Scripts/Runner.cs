using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runner : MonoBehaviour
{
    List<Transform> waypoints        = new List<Transform>();
    List<Transform> currentWaypoints = new List<Transform>();
    List<Vector2>   newPoints        = new List<Vector2>();

    public float AmountOfPoints = 3.0f;
    public float Alpha          = 0.5f;

    int waypointCount    = 0;
    int firstWaypointIdx = 0;
    int nextWaypoint     = 0;
    int currentNewPoint  = 0;

    [HideInInspector] public NavMeshAgent NavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        // Gather all secuential waypoints within Runner object
        var gatheredWaypoints = transform.parent.Find("Waypoints");
        foreach (Transform waypoint in gatheredWaypoints)
            waypoints.Add(waypoint);

        // Parameters currently not adjusted for fluent circulation with runners circulating with opposite directions
        if (Random.Range(0.0f, 1.0f) > 0.5f)
            waypoints.Reverse();

        waypointCount = waypoints.Count;
        firstWaypointIdx = Random.Range(0, waypointCount);
        NavMeshAgent.speed = Random.Range(3.5f, 6.0f);

        // Go through circular buffer of waypoints
        nextWaypoint = (firstWaypointIdx + 1) % waypoints.Count;

        // Register current position
        currentWaypoints.Add(transform);
        currentWaypoints.Add(waypoints[firstWaypointIdx].transform);

        NavMeshAgent.destination = new Vector3(waypoints[nextWaypoint].transform.position.x, waypoints[nextWaypoint].transform.position.y, transform.position.z);
        currentNewPoint = (int)AmountOfPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance && waypointCount > 3)
        {
            if (currentNewPoint == (int)AmountOfPoints)
            {
                newPoints.Clear();

                // Choose the 4 waypoints needed for Catmull-Rom curve
                int wp;
                // If the first waypoint has just been reached
                if (currentWaypoints.Count < 4)
                {
                    wp = (nextWaypoint + 1) % waypointCount;
                    currentWaypoints.Add(waypoints[wp].transform);
                    wp = (nextWaypoint + 2) % waypointCount;
                    currentWaypoints.Add(waypoints[wp].transform);
                }
                else
                {
                    currentWaypoints.Clear();
                    // Go through circular buffer of waypoints
                    nextWaypoint = (nextWaypoint + 1) % waypoints.Count;
                    wp = nextWaypoint == 0 ? waypointCount - 1 : nextWaypoint % waypointCount - 1;
                    currentWaypoints.Add(waypoints[wp]);
                    wp = nextWaypoint % waypointCount;
                    currentWaypoints.Add(waypoints[wp]);
                    wp = (nextWaypoint + 1) % waypointCount;
                    currentWaypoints.Add(waypoints[wp]);
                    wp = (nextWaypoint + 2) % waypointCount;
                    currentWaypoints.Add(waypoints[wp]);
                }

                // Calculate Catmull-Rom curve
                Vector2 p0 = new Vector2(currentWaypoints[0].transform.position.x, currentWaypoints[0].transform.position.z);
                Vector2 p1 = new Vector2(currentWaypoints[1].transform.position.x, currentWaypoints[1].transform.position.z);
                Vector2 p2 = new Vector2(currentWaypoints[2].transform.position.x, currentWaypoints[2].transform.position.z);
                Vector2 p3 = new Vector2(currentWaypoints[3].transform.position.x, currentWaypoints[3].transform.position.z);
                float t0 = 0.0f;
                float t1 = GetT(t0, p0, p1);
                float t2 = GetT(t1, p1, p2);

                float t3 = GetT(t2, p2, p3);
                for (float t = t1; t < t2; t += ((t2 - t1) / AmountOfPoints))
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
                // Go to the next generated point for smoothing
                NavMeshAgent.destination = new Vector3(newPoints[currentNewPoint].x, transform.position.y, newPoints[currentNewPoint].y);
                currentNewPoint++;
            }
        }
    }

    float GetT(float t, Vector2 p0, Vector2 p1)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, Alpha);
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
}
