using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Utils : Object
{
    public static Flock InstantiateFlock(Flock flockPrefab, FlockGroup flockGroup, Vector3 pos, Quaternion rotation)
    {
        Flock obj = Instantiate(flockPrefab, pos, rotation);
        return obj;
    }

    public static bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for(int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center
                + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                result.y = center.y;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
