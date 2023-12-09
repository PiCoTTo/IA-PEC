using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pada1.BBCore.Framework;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;


[Action("Get Terrain Random Point")]
public class ActionGetTerrainRandomPoint : BasePrimitiveAction
{
    //[InParam("terrain")]
    Terrain terrain;

    [OutParam("point")]
    Vector3 point;

    Vector3 size;
    Vector3 pos;

    public override void OnStart()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        size = terrain.terrainData.size;
        pos = terrain.GetPosition();

        Debug.Log("Get Terrain Random OnStart()");
        //base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        point = new Vector3(
            Random.Range(pos.x - size.x / 2, pos.x + size.x / 2),
            pos.y,
            Random.Range(pos.z - size.z / 2, pos.z + size.z / 2));

        Debug.Log("Get Terrain Random OnUpdate(): pos = " + pos);

        return TaskStatus.COMPLETED;

        //return base.OnUpdate();
    }
}
