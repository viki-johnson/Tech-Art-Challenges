using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]

public class LandscapeMaker : MonoBehaviour
{
    public float cellSize = 1f;
    public int width = 24;
    public int height = 24;
    public float bumpyness = 5f;
    public float bumpHeight = 5f;

    public float r;
    private void Start() {
        MakeLandscape();
    }
    

    public void MakeLandscape()
    {
            
        // r = Random.Range(0,100000);

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();
        MeshBuilder mb = new MeshBuilder(6);

        //points for our plane
        Vector3[,]points = new Vector3[width,height];

        for(int x =0; x<width; x++){
            for(int y=0; y<height; y++){
                float randomX = Random.Range(-r,r);
                float randomY = Random.Range(-r,r);

                points[x,y] = new Vector3(
                    cellSize * x + randomX,
                    Mathf.PerlinNoise(x * bumpyness * 0.1f ,y * bumpyness* 0.1f ) * bumpHeight,
                    cellSize * y + randomY);
            }
        }

        for (int x=0; x<width-1; x++){
            for(int y=0; y<height-1; y++){
                Vector3 br = points [x,     y];
                Vector3 bl = points [x+1,   y];
                Vector3 tr = points [x,     y+1];
                Vector3 tl = points [x+1,   y+1];


                int mat = 0;

                    mb.BuildTriangle(bl, tr, tl, mat, x, y, width, height, 1);
                    mb.BuildTriangle(bl, br, tr, mat, x, y, width, height, 2);           
            }
        }

        meshFilter.mesh = mb.CreateMesh();
        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
