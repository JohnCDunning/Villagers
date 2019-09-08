using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTesting : MonoBehaviour
{
    Mesh mesh;
    MeshFilter mf;
    private Vector3[] verticies;
    private int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public float Amplitude;
    public float FreqX;
    public float FreqZ;
    public float flatHeight;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateShape();
    }

    void CreateShape()
    {
        verticies = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * FreqX, z * FreqZ) * Amplitude;
                if(y / Amplitude < flatHeight)
                {
                    
                    y = Random.Range(0,0.1f);
                }
                verticies[i] = new Vector3(x,y,z);
                i++;
            }
        }


        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

               
            }
            vert++;
        }
        
        
    }
    void UpdateShape()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateShape();

    }
    private void OnDrawGizmos()
    {
        if(verticies == null)
        {
            return;
        }
        for (int i = 0; i < verticies.Length; i++)
        {
            Gizmos.DrawSphere(verticies[i], 0.1f);
        }
    }
}
