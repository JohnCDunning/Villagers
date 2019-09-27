using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public Vector2 _WorldX;
    public Vector2 _WorldZ;

    [Header("Tree Settings")]
    public int _TreesPerPoint = 10;
    public float _TreeRangeFromPoint;
    public int _TreeGroups;
    Vector3[] TreePos;
    public GameObject _Tree;
    [Header("Rock Settings")]
    public int _RockPerPoint = 10;
    public float _RockRangeFromPoint;
    public int _RockGroups;
    Vector3[] RockPos;
    public GameObject _Rock;
    [Header("BerryBush Settings")]
    public int _BerryPerPoint = 10;
    public float BerryRangeFromPoint;
    public int _BerryGroups;
    Vector3[] BerryPos;
    public GameObject _BerryBush;
    [Header("Chicken Settings")]
    public int _ChickenPerPoint = 10;
    public float ChickenRangeFromPoint;
    public int _ChickenGroups;
    Vector3[] ChickenPos;
    public GameObject _Chicken;

    //ParentObjects
    [Header("Parent Assignments")]
    public Transform _TreeParent;
    public Transform _RockParent;
    public Transform _BushParent;
    public Transform _ChickenParent;


    // Start is called before the first frame update
    void Start()
    {
        SetUpObject(_RockGroups, RockPos, _Rock, _RockPerPoint,_RockRangeFromPoint,_RockParent);
        SetUpObject(_ChickenGroups, ChickenPos, _Chicken, _ChickenPerPoint, ChickenRangeFromPoint,_ChickenParent);
        SetUpObject(_BerryGroups, BerryPos, _BerryBush, _BerryPerPoint, BerryRangeFromPoint,_BushParent);
        SetUpObject(_TreeGroups, TreePos, _Tree, _TreesPerPoint,_TreeRangeFromPoint,_TreeParent);
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

        Instantiate(obj, pos, Quaternion.identity,Parent);
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
