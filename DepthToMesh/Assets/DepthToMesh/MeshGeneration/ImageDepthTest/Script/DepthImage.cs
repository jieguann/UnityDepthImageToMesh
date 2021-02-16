/*
 * Tutorial for mesh generation for code in unity
 * https://catlikecoding.com/unity/tutorials/procedural-grid/
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DepthImage : MonoBehaviour
{
    public Texture2D m_DepthTexture_Float;

    Mesh mesh;
    public RawImage raw;
    Vector3[] vertices = null;
    Color[] colors = null;
    private Vector2[] uvs;
    private int[] indices;
    private int[] triangles;

    //Value Control
    public float depthRange;

    // Start is called before the first frame update
    void Start()
    {
        //m_DepthTexture_Float = new Texture2D(12,12);
        //GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        depthRange = 0.9f; //The value to show the reange of depth

    }

    // Update is called once per frame
    void Update()
    {
        ReprojectPointCloud();
        raw.texture = m_DepthTexture_Float;
    }

    void ReprojectPointCloud()
    {
        mesh = new Mesh();
        mesh.name = "Point Cloud";
        //print("Depth:" + m_DepthTexture_Float.width + "," + m_DepthTexture_Float.height);
        //print("Color:" + m_CameraTexture.width + "," + m_CameraTexture.height);
        int width_depth = m_DepthTexture_Float.width;
        int height_depth = m_DepthTexture_Float.height;
        //int width_camera = m_CameraTexture.width;
        int width_camera = m_DepthTexture_Float.width;

        if (vertices == null || colors == null)
        {
            //vertices = new Vector3[(width_depth +1) * (height_depth + 1)];
            vertices = new Vector3[width_depth * height_depth];
            //For Texture UV
            uvs = new Vector2[vertices.Length];

            //colors = new Color[(width_depth + 1) * (height_depth + 1)];
            colors = new Color[vertices.Length];
            indices = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i] = i;
                //trangle

            }
            //XRCameraIntrinsics intrinsic;
            //m_CameraManager.TryGetIntrinsics(out intrinsic);
            //print("intrinsics:" + intrinsic);

            //float ratio = (float)width_depth / (float)width_camera;
            //fx = intrins/ic.focalLength.x * ratio;
            //fy = intrinsic.focalLength.y * ratio;

            //cx = intrinsic.principalPoint.x * ratio;
            //cy = intrinsic.principalPoint.y * ratio;

        }

        Color[] depthPixels = m_DepthTexture_Float.GetPixels();


        //int index_dst;
        float depth;

        for (int index_dst =0, depth_y = 0; depth_y < height_depth; depth_y++)
        {
            //index_dst = depth_y * width_depth;
            for (int depth_x = 0; depth_x < width_depth; depth_x++, index_dst++)
            {
                
                //colors[index_dst] = m_DepthTexture_Float.GetPixelBilinear((float)depth_x / (width_depth), (float)depth_y / (height_depth));
                colors[index_dst] = m_DepthTexture_Float.GetPixelBilinear((float)width_depth, (float)height_depth);
                //print(depthPixels[20].r);
                //depth = depthPixels[index_dst].r;


                //to showing the vertices since the r value is too small
                float pointDepth = -100;
                depth = depthPixels[index_dst].grayscale *pointDepth;
                //depth = depthPixels[index_dst].r * pointDepth;
                //print(depthPixels[5]);

                vertices[index_dst].z = -depth;
                //vertices[index_dst].z = 100;
                vertices[index_dst].x = depth_x;
                vertices[index_dst].y = depth_y;

                //index_dst++;

                //Set UV
                uvs[index_dst] = new Vector2((float)depth_x / (width_depth-1), (float)depth_y / (height_depth-1));


            }
        }

        
        //mesh vertices 
        mesh.vertices = vertices;
        //apply uv
        mesh.uv = uvs;
        //showing the point of the vertices(for testing, when generate the mesh can be commented)
        //mesh.SetIndices(indices, MeshTopology.Points, 0); 

        GetComponent<MeshFilter>().mesh = mesh;
        



        //mesh
        /*
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = width_depth;
        triangles[2] = 1;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        */
        
        int[] triangles = new int[(width_depth -1) * (height_depth -1) * 6]; //-1 since the mesh is one less then the width and heigh size of the pixel image 
        //int[] triangles = new int[width_depth * height_depth * 6];
        
        for (int ti = 0, vi = 0, y = 0; y < height_depth -1; y++, vi++)
        {
            for (int x = 0; x < width_depth -1 ; x++, ti += 6, vi++)
            {
                //IF statement
                //Attempt to show a part of the mesh
                
                if (depthPixels[vi].r < depthRange)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + width_depth;
                    triangles[ti + 5] = vi + width_depth + 1;
                }



                //showing the Whole mesh of the depth map
                /*
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width_depth;
                triangles[ti + 5] = vi + width_depth + 1;
                */

            }
        }
        mesh.triangles = triangles;
        //mesh.SetIndices(triangles, MeshTopology.Triangles, 0, false);
        mesh.RecalculateNormals();
        
    }


    //for showing the point
    /*
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    */
}
