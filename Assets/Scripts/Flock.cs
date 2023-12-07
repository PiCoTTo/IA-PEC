using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;
    int yAxisSign = 1;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(FlockGroup.Instance.MinSpeed, FlockGroup.Instance.MaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // Make flocks move to the center of the group if they move too far away
        Bounds bounds = new Bounds(FlockGroup.Instance.transform.position, FlockGroup.Instance.VolumeBoundaries * 2);
        turning = !bounds.Contains(transform.position) ? true : false;

        if (turning)
        {
            // Bees to the center of the group
            Vector3 dir = FlockGroup.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), FlockGroup.Instance.RotationSpeed * Time.deltaTime);
        }
        else
        {
            // Update rules of flocks motion

            // Change speed not too often
            if (Random.Range(0, 100) < 1)
            {
                speed = Random.Range(FlockGroup.Instance.MinSpeed, FlockGroup.Instance.MaxSpeed);
            }

            float nfDistance;
            Vector3 averageCentre = Vector3.zero;
            Vector3 notConsider = Vector3.zero;
            int groupSize = 0;
            float groupSpeed = 0.01f;

            foreach (GameObject flockObj in FlockGroup.Instance.FlockPrefabs)
            {
                if (flockObj != this.gameObject)
                {
                    nfDistance = Vector3.Distance(flockObj.transform.position, transform.position);
                    if (nfDistance <= FlockGroup.Instance.NearbyFlockDistance)
                    {
                        averageCentre += flockObj.transform.position;
                        groupSize++;

                        if (nfDistance < FlockGroup.Instance.NotConsiderDistance)
                        {
                            notConsider = notConsider + (transform.position - flockObj.transform.position);
                        }
                    }

                    Flock flock = flockObj.GetComponent<Flock>();
                    groupSpeed += flock.speed;
                }
            }

            if (groupSize > 0)
            {
                averageCentre = averageCentre / groupSize + (FlockGroup.Instance.Destination - transform.position);
                speed = groupSpeed / groupSize;
                Mathf.Clamp(speed, FlockGroup.Instance.MinSpeed, FlockGroup.Instance.MaxSpeed);

                Vector3 dir = (averageCentre + notConsider) - transform.position;
                if (dir != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(dir),
                        FlockGroup.Instance.RotationSpeed * Time.deltaTime);
            }

            // Change direction of movement on Y axis not too often
            if (Random.Range(0, 100) < 1)
            {
                if (Random.Range(0, 100) < 50)
                    yAxisSign = -1;
                else
                    yAxisSign = 1;
            }
        }

        this.transform.Translate(0, yAxisSign * speed * Time.deltaTime, speed * Time.deltaTime);
    }
}
