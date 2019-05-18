using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuildCheck : MonoBehaviour
{
    [HideInInspector]
    public GameObject _ConstructionBuilding;
    [HideInInspector]
    public GameObject _BuildingToSpawn;
    public GameObject _CanBuildSphere;
    public Material _CanBuild;
    public Material _CantBuild;
    public bool CanBuild = true;

    private void Update()
    {

        if(CanBuild == true)
        {
            _CanBuildSphere.GetComponent<Renderer>().material = _CanBuild;
        }
        else
        {
            _CanBuildSphere.GetComponent<Renderer>().material = _CantBuild;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            CanBuild = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            CanBuild = true;
        }
    }
}
