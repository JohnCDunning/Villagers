using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject _WallPart;
    public GameObject _RockTower;
    private GameObject _LatestWall;
    private int num = 0;

    public List<GameObject> wallSegments = new List<GameObject>();

    Transform originPoint;

    public LayerMask _Layer;
    // Start is called before the first frame update
    void Start()
    {
        Transform tempTran = transform;
        if(wallSegments.Count != 0)
        {
            if (wallSegments[0].gameObject != null)
            {
                tempTran = wallSegments[0].transform;
            }

        }

        SpawnTower(tempTran.position, tempTran.rotation,transform); //tempTran.position + transform.TransformDirection(Vector3.forward)
        originPoint = transform;
    }
    void SpawnTower(Vector3 spawnPosition, Quaternion rotation,Transform parent)
    {
        GameObject tower = Instantiate(_RockTower, parent);
        tower.transform.position = spawnPosition;
        tower.transform.rotation = rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Destroy(wallSegments[wallSegments.Count - 1]);
            Wall newWall =_LatestWall.AddComponent<Wall>();
            newWall._RockTower = _RockTower;
            newWall._WallPart = _WallPart;
            newWall._Layer = _Layer;
            Destroy(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            SpawnTower(transform.position , transform.rotation,null);
            Destroy(gameObject);
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
            Destroy(this);
            
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, _Layer))
        {
            transform.LookAt(new Vector3(hit.point.x, hit.point.y,hit.point.z));

            float dist = Vector3.Distance(originPoint.position, hit.point);
            if(dist > 1)
            {
                if(Mathf.RoundToInt(dist) > wallSegments.Count && num < 10)
                {
                    GameObject temp = Instantiate(_WallPart, new Vector3(originPoint.position.x, originPoint.position.y, originPoint.position.z + num), Quaternion.identity, originPoint);
                    temp.transform.localPosition = new Vector3(0, 0, 0 + num);
                    temp.transform.rotation = originPoint.rotation;
                    wallSegments.Add(temp);
                    num++;
                    _LatestWall = temp;
                }
                if(Mathf.RoundToInt(dist) < wallSegments.Count)
                {
                    GameObject wall = wallSegments[wallSegments.Count -1];
                    wallSegments.Remove(wall);
                    num--;
                    Destroy(wall.gameObject);
                    _LatestWall = wallSegments[wallSegments.Count - 1];
                }
            }
        }
    }
}
