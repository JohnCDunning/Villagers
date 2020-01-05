using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VertexColorTest : MonoBehaviour
{
    public NavMeshSurface _Navmesh;
    Mesh mesh;
    MeshFilter meshFilter;

    public Color _GrassColor;
    public Color _DefaultGround;
    public Color _RockColor;
    public Color _BerryBushColor;


    private List<GameObject> _ObjectPositions = new List<GameObject>();

    public WorldGeneration _WorldGen;

    private void Awake()
    {

        mesh = GetComponent<MeshFilter>().mesh;

        meshFilter = GetComponent<MeshFilter>();
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Mathf.Abs(Mathf.PerlinNoise(vertices[i].x * .05f, vertices[i].z * .05f)) * 1.5f, vertices[i].z);
        }
        

    }
    // Start is called before the first frame update
    void Start()
    {
       // TreePositions = _WorldGen.TreePositions;

        Invoke("LateStart", 0f);

    }

    void LateStart()
    {
        TreePositions = _WorldGen.TreePositions;
        RockPositions = _WorldGen.RockPositions;
        BerryPositions = _WorldGen.BerryPositions;
        // PaintWorld(_WorldGen.RockPositions, _RockColor, true);

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) //lerp verts
        {


            colors[i] = _DefaultGround;//Color.Lerp(Color.blue, Color.red, i/vertices.Length);
            Vector3 vertPos = transform.TransformPoint(vertices[i]); //worldspace vert
            foreach (GameObject ob in _ObjectPositions) //loop tree pos
            {
                // Vector3 pos2 = new Vector2(vertPos.x, vertPos.z);

                if (Vector3.Distance(ob.transform.position, vertPos) < 5)
                {
                    colors[i] = _GrassColor;
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Random.Range(0.5f,1), vertices[i].z);
                }

               // foreach(Vector3 rockPos in RockPositions)
               // {
                  //  if (Vector3.Distance(rockPos, vertPos) < 2.5f)
                   // {
                      //  colors[i] = _RockColor;
                    //}
                   // foreach (Vector3 berryPos in BerryPositions)
                   // {
                       // if (Vector3.Distance(berryPos, vertPos) < 2)
                       // {
                        //    colors[i] = _BerryBushColor;
                       // }
                   // }
               // }

            }
        }

        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateBounds();
        // assign the array of colors to the Mesh.
        mesh.colors = colors;
        //mesh.RecalculateNormals();
        _Navmesh.BuildNavMesh();
    }
}
