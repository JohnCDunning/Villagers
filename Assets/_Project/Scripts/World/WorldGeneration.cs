using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  
  
    // Start is called before the first frame update
    void Start()
    {
       
        for (int i = 0; i < _SpawnableObjects.Length; i++)
        {
            SpawnableObject _Object = _SpawnableObjects[i];
            SetUpObject(_Object._ObjectTotalGroups, _Object.ObjectPos, _Object._ObjectPrefab, _Object._ObjectsPerPoint, _Object._ObjectRangeFromPoint, _Object._ObjectParent);
        }
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

        GameObject placedObject = Instantiate(obj, new Vector3(pos.x,pos.y + 60 ,pos.z), Quaternion.identity, Parent);

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
