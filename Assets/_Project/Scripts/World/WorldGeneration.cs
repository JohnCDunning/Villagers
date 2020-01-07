using System.Collections;
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


    private Vector2 _WorldX, _WorldZ;

    [HideInInspector]
    public List<Vector3> _ObjectPositions = new List<Vector3>();

    private NavMeshSurface _Navmesh;
    private Mesh groundMesh;

 
    public Color[] _GrassColors;
    public Color[] _DefaultGround;

    [Header("World Gen Variable Requirments")]
    public GameObject _Ground;
    public Material _Leaves;
    public GameObject[] _Villages;
    public float _MinDistanceAllowedNearVillage; //For objects to spawn nearbye

    // Start is called before the first frame update
    void Awake()
    {
        Collider groundCol = _Ground.GetComponent<Collider>();
        //Sets Object spawn range based on the world collider
        _WorldX = new Vector2(-groundCol.bounds.size.x / 2, groundCol.bounds.size.x / 2);
        _WorldZ = new Vector2(-groundCol.bounds.size.z / 2, groundCol.bounds.size.z / 2);

        _Navmesh = FindObjectOfType<NavMeshSurface>();
        groundMesh = _Ground.GetComponent<MeshFilter>().mesh;

        SpawnObjects();
        PerlinNoise();
        CarveWorld();
        Invoke("PaintWorld", 1.5f);
        
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
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + Mathf.Abs(Mathf.PerlinNoise(vertices[i].x * .1f, vertices[i].z * .1f)) * 0.5f, vertices[i].z);
        }

        SetMesh(vertices, groundMesh,null);
    }
    void CarveWorld()
    {
        
        Vector3[] vertices = groundMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertPos = _Ground.transform.TransformPoint(vertices[i]);
            foreach (Vector3 pos in _ObjectPositions)
            {
                if (Vector3.Distance(pos, vertPos) < 5)
                {
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y + 0.1f, vertices[i].z); //+ Random.Range(0.1f, 0.3f)
                }
            }
            //Debug.Log(vertPos);
            Vector3 testPos = _Ground.transform.TransformPoint(vertices[i]);
            for (int x = 0; x < _Villages.Length; x++)
            {
                if (Vector3.Distance(new Vector3(_Villages[x].transform.localPosition.x, testPos.y, _Villages[x].transform.localPosition.z), testPos) < 10)
                {
                    vertices[i] = new Vector3(vertices[i].x, 0, vertices[i].z);
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

            Vector3 vertPos = _Ground.transform.TransformPoint(vertices[i]);
            WorldResource[] allObs = FindObjectsOfType<WorldResource>();
            foreach (WorldResource worldObject in allObs)
            {
                
                if (Vector3.Distance(worldObject.transform.position, vertPos) < 5)
                {
                    if(worldObject._ResourceType == ResourceType.wood)
                    {
                        colors[i] = _GrassColors[grassColor];
                        Color color = _Leaves.color;
                        float b = 0;
                        Color leaves = new Color(_GrassColors[grassColor].r * 1.1f, _GrassColors[grassColor].g, _GrassColors[grassColor].b - 0.5f, _GrassColors[grassColor].a);
                        _Leaves.SetColor("_BaseColor", leaves);
                        
                    }
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

                for (int v = 0; v < _Villages.Length; v++)
                {
                    Vector3 pos = _Villages[v].transform.position;
                    if (Vector3.Distance(tempPos, new Vector3(pos.x, tempPos.y, pos.z)) > _MinDistanceAllowedNearVillage)
                    {
                        PlaceObject(obj, tempPos, Parent);
                    }
                    else
                    {
                        break;
                    }
                }
               
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
