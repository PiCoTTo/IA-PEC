using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour
{
    [HideInInspector]
    public bool IsRobbing = false;
    [HideInInspector]
    public bool IsfleeingFromCop = false;
    [HideInInspector]
    public GameObject ChasingCop = null;
    [HideInInspector]
    public GameObject TargetJustRobbed = null;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
