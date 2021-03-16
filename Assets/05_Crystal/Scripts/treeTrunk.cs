using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fiveCrystal {

    [RequireComponent (typeof(MeshRenderer))]
    [RequireComponent (typeof(MeshFilter))]
    public class treeTrunk : MonoBehaviour
    {
        public float radius;
        public int sides = 8;
        public float height, spike;

        public float var0, var1, var2, radiusVar;

        public float[] heights;
        public float[] innerRadius;

        private void Start() {
            MakeCircle();
        }

        public void MakeCircle()
        {
            MeshFilter meshfilter = this.GetComponent<MeshFilter>();
            MeshBuilder mb = new MeshBuilder(1);

            heights = new float[sides+1];
            innerRadius = new float[sides+1];

            float h = height+Random.Range(-var0,var0);

            for(int w=0; w<sides; w++)
            {
                heights[w] = h+Random.Range(-var1,var1);
                // innerRadius[w] = radius-Random.Range(radiusVar, radiusVar*3);
                innerRadius[w] = radius;
            }

            heights[sides] = heights[0];
            innerRadius[sides] = innerRadius[0];

            float s = spike+h+Random.Range(-var2, var2);

            Debug.Log(s + "  " + h);
            
            Vector3 centre = new Vector3(0, s, 0);


            for(int d = 0; d<sides; d++){
                float degrees = 360f/sides;


                Vector3 b0 = Quaternion.AngleAxis(degrees * d, Vector3.up) * Vector3.forward * radius;
                Vector3 t0 = Quaternion.AngleAxis(degrees * d, Vector3.up*heights[d]) * Vector3.forward * innerRadius[d+1];
                t0 = new Vector3(t0.x, t0.y + heights[d], t0.z);
                // Vector3 t0 = new Vector3(b0.x, b0.y+heights[d], b0.z);

                Vector3 b1 = Quaternion.AngleAxis(degrees * (d+1), Vector3.up) * Vector3.forward * radius;
                Vector3 t1 = Quaternion.AngleAxis(degrees * (d+1), Vector3.up*heights[d+1]) * Vector3.forward * innerRadius[d+1];
                t1 = new Vector3(t1.x, t1.y + heights[d+1], t1.z);
                // Vector3 t1 = new Vector3(b1.x, b1.y+heights[d+1], b1.z);


                mb.BuildTriangle(t0, b1, t1, 0, 0);
                mb.BuildTriangle(b0, b1, t0, 0, 1);
                

                mb.BuildTriangle(t0, t1, centre, 0);
            }

            meshfilter.mesh = mb.CreateMesh();
        }
    }
}