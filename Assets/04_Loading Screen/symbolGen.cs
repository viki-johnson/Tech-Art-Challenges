using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class symbolGen : genBase
{
    void Start()
    {

        sides = Random.Range(5,10);
        radius = Random.Range(1f, 2f);

        if(sides % 2 == 0){
            sides++;
        }

        spike = (sides-1)/2;



        line.positionCount = sides+1;


        for(int d = 0; d<sides; d++)
        {

            int pos = d*spike % sides;

            float degrees = 360f/sides;
            Vector3 v0 = Quaternion.AngleAxis(degrees * pos, Vector3.up) * Vector3.forward * radius;
            GameObject go = Instantiate(sphere, v0, Quaternion.identity);
            go.name = "point " + d;
            points.Add(go);

            go.transform.parent = pointList.transform;

            order += " " + pos + " ";

            line.SetPosition(d, go.transform.position);
        }
        line.SetPosition(sides, points[0].transform.position);
    }
}
