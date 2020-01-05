﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WorldGeneration : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        [Header("Object To Spawn")]
        public string _Name;
        public GameObject _ObjectPrefab;
        public int _ObjectTotalGroups;
        public int _ObjectsPerPoint;
        public float _ObjectRangeFromPoint;
        public Transform _ObjectParent;

        [HideInInspector] public Vector3[] ObjectPos;
    }
    public SpawnableObject[] _SpawnableObjects;


    public Vector2 _WorldX;
    public Vector2 _WorldZ;


    [HideInInspector]
    public List<Vector3> TreePositions = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> RockPositions = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> BerryPositions = new List<Vector3>();
    [HideInInspector]
    public List<GameObject> _AllObjects = new List<GameObject>();
    public List<Vector3> _ObjectPositions = new List<Vector3>();




    public NavMeshSurface _Navmesh;
    Mesh groundMesh;

 
    public Color[] _GrassColors;
    public Color[] _DefaultGround;
    public Color _RockColor;
    public Color _BerryBushColor;
    public GameObject _Ground;

    // Start is called before the first frame update
    void Awake()
    {
        groundMesh = _Ground.GetComponent<MeshFilter>().mesh;

        SpawnObjects();
        PerlinNoise();
        CarveWorld();
        Invoke("PaintWorld", 2f);
        
    }
    void SpawnObjects()
    {
        for (int i = 0; i < _SpawnableObjects.Length; i++)
        {
            SpawnableObject _Object = _SpawnableObjects[i];
            SetUpObject(_Object._ObjectTotalGroups, _Object.ObjectPos, _Object._ObjectPrefab, _Object._ObjectsPerPoint, _Object._ObjectRangeFromPoint, _Object._ObjectParent);
        }

    }
    void PerlinNoise()
    {
        Vector3[] vertices = groundMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Mathf.Abs(Mathf.PerlinNoise(vertices[i].x * .05f, vertices[i].z * .05f)) * 1f, vertices[i].z);
        }

        SetMesh(vertices, groundMesh,null);
    }
    void CarveWorld()
    {
        Vector3[] vertices = groundMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertPos = transform.TransformPoint(vertices[i]);
            foreach (Vector3 pos in _ObjectPositions)
            {
                if (Vector3.Distance(pos, vertPos) < 5)
                {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y + 0.3f, vertices[i].z); //+ Random.Range(0.1f, 0.3f)
                }
            }
        }
        SetMesh(vertices, groundMesh, null);
    }
    void PaintWorld()
    {
        int defaultGrassColor = Random.Range(0, _DefaultGround.Length);
        int grassColor = Random.Range(0, _GrassColors.Length);

        Vector3[] vertices = groundMesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = _DefaultGround[defaultGrassColor];

            Vector3 vertPos = transform.TransformPoint(vertices[i]);
            foreach (Vector3 pos in _ObjectPositions)
            {

                if (Vector3.Distance(pos, vertPos) < 5)
                {
                    colors[i] = _GrassColors[grassColor];
                }
            }
        }
        SetMesh(vertices, groundMesh, colors);
    }
    void SetMesh(Vector3[] vertices, Mesh mesh, Color[] meshColors)
    {
        MeshFilter mf = _Ground.GetComponent<MeshFilter>();
        MeshCollider mc = _Ground.GetComponent<MeshCollider>();

        mf.mesh.vertices = vertices;
        mf.mesh = mesh;
        mc.sharedMesh = mesh;

        if (meshColors != null)
            mf.mesh.colors = meshColors;

        mf.mesh.RecalculateNormals();
        mf.mesh.RecalculateBounds();
        _Navmesh.BuildNavMesh();
        groundMesh = _Ground.GetComponent<MeshFilter>().mesh;
    }

   

    void SetUpObject(int objectGroups, Vector3[] objectPos, GameObject obj, int objectsPerPoint, float objectRangeFromPoint,Transform Parent)
    {
        objectPos = new Vector3[objectGroups];

        //Generating the origin treepoints;
        for (int i = 0; i < objectGroups; i++)
        {
            objectPos[i] = CheckSpot();
            PlaceObject(obj, objectPos[i], Parent);
            for (int x = 0; x < objectsPerPoint; x++)
            {
                Vector3 tempPos = CheckClosePosition(objectPos[i], objectRangeFromPoint);
                
                PlaceObject(obj, tempPos,Parent);
            }
        }
    }

    void PlaceObject(GameObject obj, Vector3 pos, Transform Parent)
    {
        if (pos == Vector3.zero)
            return;

        GameObject placedObject = Instantiate(obj, new Vector3(pos.x,pos.y,pos.z), Quaternion.identity, Parent);
        _ObjectPositions.Add(placedObject.transform.position);
        placedObject.transform.position = new Vector3(pos.x, pos.y + 60, pos.z);

        _AllObjects.Add(placedObject);
        

    }
    Vector3 CheckClosePosition(Vector3 pos, float objectRangeFromPoint)
    {
        Vector3 Spot = new Vector3(Random.Range(-objectRangeFromPoint, objectRangeFromPoint), 100,Random.Range(-objectRangeFromPoint, objectRangeFromPoint));
        //Below creates kinda circle gaps
        //Vector3 Spot = new Vector3(Random.insideUnitCircle.x * _TreeRangeFromPoint, 0,Random.insideUnitCircle.y * _TreeRangeFromPoint);
        
        RaycastHit hit;
        if (Physics.Raycast(pos + Spot, Vector3.down, out hit, 100))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
    Vector3 CheckSpot()
    {
        
        Vector3 Spot = new Vector3(Random.Range(_WorldX.x, _WorldX.y), 15, Random.Range(_WorldZ.x, _WorldZ.y));
        RaycastHit hit;
        if (Physics.Raycast(Spot, Vector3.down,out hit, 100))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
}
