using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.AI;
public class WorldEdit : MonoBehaviour
{
    public GameObject _Sphere;
    public GameObject _Ground;


    //NavMeshSurface _NavMesh;

    MeshFilter mf;

    bool UpdateMesh = false;
    // Start is called before the first frame update
    void Start()
    {
        mf = _Ground.GetComponent<MeshFilter>();
        //_NavMesh = FindObjectOfType<NavMeshSurface>();



    }

    void EditWorld()
    {
        Vector3[] vertices = mf.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 temp = _Ground.transform.TransformPoint(vertices[i]);
            float dist = (_Sphere.transform.position - temp).sqrMagnitude;
            if (dist < 10)
            {
                vertices[i] += Vector3.up * 1;
            }
            mf.mesh.vertices = vertices;

            //mf.mesh.RecalculateNormals();
            //mf.mesh.RecalculateBounds();
            _Ground.GetComponent<MeshCollider>().convex = true;
            _Ground.GetComponent<MeshCollider>().sharedMesh = mf.mesh;
            _Ground.GetComponent<MeshCollider>().convex = false;
            
        }
    }


    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            _Sphere.transform.position = hit.point;
        }

        if (Input.GetMouseButtonDown(0))
        {

            EditWorld();
        }
    }
}