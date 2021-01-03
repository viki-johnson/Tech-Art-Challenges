using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleGen : genBase
{
    void Start()
    {
        radius = Random.Range(0.5f, 1.5f);

        line.positionCount = sides+1;


        for(int d = 0; d<sides; d++)
        {

            float degrees = 360f/sides;
            Vector3 v0 = Quaternion.AngleAxis(degrees * d, Vector3.up) * Vector3.forward * radius;
            GameObject go = Instantiate(sphere, v0, Quaternion.identity);
            go.name = "point " + d;
            points.Add(go);

            go.transform.parent = pointList.transform;

            line.SetPosition(d, go.transform.position);
        }


        line.SetPosition(sides, points[0].transform.position);
    }
}