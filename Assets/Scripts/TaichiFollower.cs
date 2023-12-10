using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaichiFollower : MonoBehaviour
{
    
    public GameObject target;
    public Vector3 pos;
    public Quaternion rot;

    [HideInInspector] public NavMeshAgent NavMeshAgent;

    void Start()
    {
        target = GameObject.Find("TaichiMaster");
        pos = transform.position - target.transform.position;
        NavMeshAgent = GetComponent<NavMeshAgent>();

        //transform.rotation = target.transform.rotation;
        transform.position = target.transform.TransformPoint(pos);
    }

    void Update()
    {
        NavMeshAgent.destination = target.transform.TransformPoint(pos);
        // Not valid to obstacle avoidance
        //transform.position = target.transform.TransformPoint(pos);
    }
}
