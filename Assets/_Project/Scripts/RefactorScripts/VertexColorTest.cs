using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColorTest : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;

    public Color _GrassColor;
    public Color _DefaultGround;
    public Color _RockColor;

    public List<Vector3> TreePositions = new List<Vector3>();
    public List<Vector3> RockPositions = new List<Vector3>();
    public WorldGeneration _WorldGen;
    // Start is called before the first frame update
    void Start()
    {
       // TreePositions = _WorldGen.TreePositions;

        Invoke("LateStart", 0f);

    }

    void LateStart()
    {
        TreePositions = _WorldGen.TreePositions;

        PaintWorld(_WorldGen.TreePositions, _GrassColor);
       // PaintWorld(_WorldGen.RockPositions, _RockColor, true);


    }

    void PaintWorld(List<Vector3> positions, Color color)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) //lerp verts
        {
            
            
            colors[i] = _DefaultGround;//Color.Lerp(Color.blue, Color.red, i/vertices.Length);
            Vector3 vertPos = transform.TransformPoint(vertices[i]); //worldspace vert
            foreach (Vector3 pos in positions) //loop tree pos
            {
                // Vector3 pos2 = new Vector2(vertPos.x, vertPos.z);

                if (Vector3.Distance(pos, vertPos) < 5)
                    colors[i] = color;

            }
        }
        // assign the array of colors to the Mesh.
        mesh.colors = colors;
        //mesh.RecalculateNormals();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
