using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject _WallPart;
    private GameObject _LatestWall;
    private int num = 1;

    public List<GameObject> wallSegments = new List<GameObject>();

    Transform originPoint;
    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Wall newWall =_LatestWall.AddComponent<Wall>();
            newWall._WallPart = _WallPart;
            Destroy(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < wallSegments.Count; i++)
            {
                Destroy(wallSegments[i].gameObject);
            }
            Destroy(this);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            transform.LookAt(new Vector3(hit.point.x, hit.point.y,hit.point.z));

            float dist = Vector3.Distance(originPoint.position, hit.point);
            if(dist > 1)
            {
                if(Mathf.RoundToInt(dist) > wallSegments.Count)
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
