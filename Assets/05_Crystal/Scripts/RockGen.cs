using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fiveCrystal {

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]


public class RockGen : MonoBehaviour
{
    public float[] radius;
    public float r;
    public int sides = 8;

    public Vector3[] bottom, mid, top;

    public Vector3 topPoint;
    public float spike, rotate;

    MeshBuilder mb;
    private void Start() {
        MeshFilter meshfilter = this.GetComponent<MeshFilter>();
        mb = new MeshBuilder(1);

        // sides = Random.Range(5,7);

        GeneratePoints();
        MakeCircle();
        meshfilter.mesh = mb.CreateMesh();

        SetScale();
    }

    void GeneratePoints()
    {
        bottom = new Vector3[sides];
        mid = new Vector3[sides];
        top = new Vector3[sides];

        for(int d = 0; d<sides; d++) {
            float degrees = 360f/sides;

            float radiusMid = Random.Range(1f,1.2f);
            // float radiusMid = Random.Range(0.4f,0.6f);

            bottom[d] = Quaternion.AngleAxis(degrees * d, Vector3.up) * Vector3.forward * (radiusMid);
            mid[d] =    Quaternion.AngleAxis(degrees * d, Vector3.up) * Vector3.forward * (radiusMid-0.05f);
            mid[d] =    new Vector3(mid[d].x, Random.Range(0.2f, 0.8f), mid[d].z);

            top[d] =    Quaternion.AngleAxis(degrees * d, Vector3.up) * Vector3.forward * Random.Range(0.5f,1f);;
            top[d] =    new Vector3(top[d].x, 1f, top[d].z);
        }

        topPoint = new Vector3(0, Random.Range(1,1+spike),0);
    }

    void MakeCircle()
    {
        for(int d = 0; d<sides; d++){

            if(d == sides-1)
            {
                mb.BuildTriangle(bottom[d], bottom[0],  mid[d],     0,  0);
                mb.BuildTriangle(mid[0],    mid[d],     bottom[0],  0,  1);

                mb.BuildTriangle(mid[d],    mid[0],     top[d],     0,  0);
                mb.BuildTriangle(top[0],    top[d],     mid[0],     0,  1);

                mb.BuildTriangle(top[d],    top[0],   topPoint, 0);


            } else {
                mb.BuildTriangle(bottom[d], bottom[d+1],    mid[d],         0,  0);
                mb.BuildTriangle(mid[d+1],  mid[d],         bottom[d+1],    0,  1);

                mb.BuildTriangle(mid[d],    mid[d+1],       top[d],         0,  0);
                mb.BuildTriangle(top[d+1],  top[d],         mid[d+1],       0,  1);

                mb.BuildTriangle(top[d],    top[d+1],   topPoint, 0);
            }
        }
    }
    void SetScale(){

        //is it a little chonk or a tall boi?
        float scaleVar = Random.Range(0,5);
        float randomY = 1f;

        // if(scaleVar >= 4) {
        //     randomY = Random.Range(1.5f,2f);
        //     // Debug.Log("tall boi " + randomY);
        // }else{
        //     randomY = Random.Range(0.5f,1f);
        //     // Debug.Log("little chonk " + randomY);
        // }

        randomY = Random.Range(0.1f,2f);

        // float randomScale = Random.Range(0.25f,1f);
        this.transform.localScale = new Vector3(0.45f, randomY, 0.45f);

        this.transform.localEulerAngles = new Vector3(Random.Range(-rotate, rotate), 0, Random.Range(-rotate, rotate));
    }
}
}