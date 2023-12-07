using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockGroup : MonoBehaviour
{
    public static FlockGroup Instance;
    public GameObject FlockPrefab;
    public List<GameObject> FlockPrefabs = new List<GameObject>();
    public int NumFlocks = 5;
    public Vector3 VolumeBoundaries = new Vector3(0.4f, 0.4f, 0.4f);
    public Vector3 Destination;

    [Range(0, 10)]
    public float MinSpeed;
    [Range(0, 10)]
    public float MaxSpeed;
    [Range(0.5f, 10)]
    public float NearbyFlockDistance;
    [Range(0.5f, 10)]
    public float RotationSpeed;

    private void Awake()
    {
        Instance = this;
        Destination = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create all flocks
        for (int i = 0; i < NumFlocks; ++i)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-VolumeBoundaries.x, VolumeBoundaries.x),
                Random.Range(-VolumeBoundaries.y, VolumeBoundaries.y),
                Random.Range(-VolumeBoundaries.z, VolumeBoundaries.z));
            FlockPrefabs.Add(Utils.Instantiate(FlockPrefab, pos, Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0))));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100) < 10)
        {
            Destination = transform.position + new Vector3(
                Random.Range(-VolumeBoundaries.x, VolumeBoundaries.x),
                Random.Range(-VolumeBoundaries.y, VolumeBoundaries.y),
                Random.Range(-VolumeBoundaries.z, VolumeBoundaries.z));
        }
    }
}
