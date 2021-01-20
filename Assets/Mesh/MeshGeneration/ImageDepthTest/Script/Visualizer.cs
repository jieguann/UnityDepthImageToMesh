using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Visualizer : MonoBehaviour
{
    Mesh mesh;
    int[] indices;
    // Start is called before the first frame update

    public void UpdateMeshInfo(Vector3[] vertices, Color[] colors)
    {
        //GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        
        if (mesh == null)
        {

            mesh = new Mesh();
            mesh.name = "Point Cloud";
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            //PointCloudの点の数はDepthのピクセル数から計算
            int num = vertices.Length;
            indices = new int[num];
            for (int i = 0; i < num; i++) { 
                indices[i] = i;
                //trangle
                
            }

            //meshを初期化
            mesh.vertices = vertices;
            mesh.colors = colors;
            //mesh.SetIndices(indices, MeshTopology.Points, 0);
            mesh.SetIndices(indices, MeshTopology.Points, 0);


            //trangle
            /*
            int[] triangles = new int[3];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            mesh.triangles = triangles;
            */


            //meshを登場させる
            gameObject.GetComponent<MeshFilter>().mesh = mesh;

            
        }
        else
        {
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.RecalculateBounds();
        }
    }
}