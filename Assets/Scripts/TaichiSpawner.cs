using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaichiSpawner : MonoBehaviour
{
    public GameObject TaichiFollowerPrefab;

    GameObject taichiMaster;
    
    // Start is called before the first frame update
    void Start()
    {
        taichiMaster = GameObject.Find("TaichiMaster");

        for(int j = 1; j < 5; j++)
            for (int i = -2; i < 3; i++)
                GameObject.Instantiate(TaichiFollowerPrefab, taichiMaster.transform.position + new Vector3(i*2, 0, -j*2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
