using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaichiMaster : MonoBehaviour
{
    [HideInInspector]
    public Vector3 OriginPos;

    public List<List<Vector3>> Exercises
    {
        get
        {
            return exercises;
        }
    }
    List<List<Vector3>> exercises = new List<List<Vector3>>();

    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.position;

        // Generate exercises
        List<Vector3> exercise1 = new List<Vector3>();

        // Circular exercise
        float radius = 3;
        int step = 10;
        for (int a = 270; a < 270 + 361; a += step)
        {
            exercise1.Add(new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * a),
                0, radius * Mathf.Sin(Mathf.Deg2Rad * a) + radius));
        }
        Exercises.Add(exercise1);

        // Back and forth exercise
        List<Vector3> exercise2 = new List<Vector3>();
        exercise2.Clear();
        step = 1;
        for (int i = 1 * step; i < 7 * step; i += step)
            exercise2.Add(new Vector3(0, 0, i));
        for (int i = 5 * step; i >= 0; i -= step)
            exercise2.Add(new Vector3(0, 0, i));
        Exercises.Add(exercise2);

        // Cross exercise
        List<Vector3> exercise3 = new List<Vector3>();
        exercise3.Clear();
        step = 3;
        exercise3.AddRange(new Vector3[] {
            new Vector3(1 * step,   0, 0),           new Vector3(0,0,0),
            new Vector3(0,          0, 1 * step),    new Vector3(0,0,0),
            new Vector3(-1 * step,  0, 0),           new Vector3(0,0,0),
            new Vector3(0,          0, -1 * step),   new Vector3(0,0,0)
        });
        Exercises.Add(exercise3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
