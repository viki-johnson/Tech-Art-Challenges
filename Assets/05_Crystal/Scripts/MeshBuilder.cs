using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fiveCrystal {

    public class MeshBuilder {

        private List<Vector3> vertices = new List<Vector3>();
        private List<int> indices = new List<int>();

        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();

        private List<int>[] submeshIndices = new List<int>[] { };

        public MeshBuilder (int submeshCount)
        {
            submeshIndices = new List<int>[submeshCount];
            for (int i = 0; i < submeshCount; i++)
            {
                submeshIndices[i] = new List<int>();
            }
        }

        public void BuildTriangle (Vector3 p0, Vector3 p1, Vector3 p2, int submesh)
        {
            int p0Index = vertices.Count;
            int p1Index = vertices.Count + 1;
            int p2Index = vertices.Count + 2;

            indices.Add(p0Index);
            indices.Add(p1Index);
            indices.Add(p2Index);

            submeshIndices[submesh].Add(p0Index);
            submeshIndices[submesh].Add(p1Index);
            submeshIndices[submesh].Add(p2Index);

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);

            uvs.Add(new Vector2(0,1));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));
        }

        public void BuildTriangle (Vector3 p0, Vector3 p1, Vector3 p2, int submesh, int tri)
        {
            int p0Index = vertices.Count;
            int p1Index = vertices.Count + 1;
            int p2Index = vertices.Count + 2;

            indices.Add(p0Index);
            indices.Add(p1Index);
            indices.Add(p2Index);

            submeshIndices[submesh].Add(p0Index);
            submeshIndices[submesh].Add(p1Index);
            submeshIndices[submesh].Add(p2Index);

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);

            if(tri == 0)
            {
                uvs.Add(new Vector2(1,0));
                uvs.Add(new Vector2(0,0));
                uvs.Add(new Vector2(1,1));
            } else {
                uvs.Add(new Vector2(0,1));
                uvs.Add(new Vector2(1,1));
                uvs.Add(new Vector2(0,0));
            }
        }

        // public void BuildTriangle (Vector3 p0, Vector3 p1, Vector3 p2, int submesh, int x, int y, int xSize, int ySize, int tri = 0)
        // {
        //     int p0Index = vertices.Count;
        //     int p1Index = vertices.Count + 1;
        //     int p2Index = vertices.Count + 2;

        //     indices.Add(p0Index);
        //     indices.Add(p1Index);
        //     indices.Add(p2Index);

        //     submeshIndices[submesh].Add(p0Index);
        //     submeshIndices[submesh].Add(p1Index);
        //     submeshIndices[submesh].Add(p2Index);

        //     vertices.Add(p0);
        //     vertices.Add(p1);
        //     vertices.Add(p2);

        //     if(tri == 0){
        //         uvs.Add(new Vector2((float)x / xSize, (float)y / ySize));
        //         uvs.Add(new Vector2((float)x / xSize, (float)y / ySize));
        //         uvs.Add(new Vector2((float)x / xSize, (float)y / ySize));
        //     }else if(tri == 1){
        //         uvs.Add(new Vector2((float)x / xSize,       ((float)y-1) / ySize));
        //         uvs.Add(new Vector2(((float)x-1) / xSize,   (float)y / ySize));
        //         uvs.Add(new Vector2((float)x / xSize,       (float)y / ySize));
        //     }else if(tri ==2){
        //         uvs.Add(new Vector2((float)x / xSize,       ((float)y-1) / ySize));
        //         uvs.Add(new Vector2(((float)x-1) / xSize,   ((float)y-1) / ySize));
        //         uvs.Add(new Vector2(((float)x-1) / xSize,   (float)y / ySize));
        //     }

        // }

        public Mesh CreateMesh ()
        {
            Mesh mesh = new Mesh();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();

            mesh.RecalculateNormals();
            mesh.uv = uvs.ToArray();

            mesh.subMeshCount = submeshIndices.Length;

            //mesh.SetTriangles ()

            for (int i = 0; i < submeshIndices.Length; i++)
            {
                if (submeshIndices[i].Count < 3)
                    mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
                else
                    mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }

            return mesh;
        }
    }
}