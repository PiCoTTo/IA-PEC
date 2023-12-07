using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : Object
{
    public static Flock InstantiateFlock(Flock flockPrefab, FlockGroup flockGroup, Vector3 pos, Quaternion rotation)
    {
        Flock obj = Instantiate(flockPrefab, pos, rotation);
        return obj;
    }
}
