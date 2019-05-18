using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuildCheck : MonoBehaviour
{
    [HideInInspector]
    public GameObject _ConstructionBuilding;
    [HideInInspector]
    public GameObject _BuildingToSpawn;
    public Material _CanBuild;
    public Material _CantBuild;
    public bool CanBuild = true;

    private void Update()
    {

        if(CanBuild == true)
        {
            GetComponent<Renderer>().material = _CanBuild;
        }
        else
        {
            GetComponent<Renderer>().material = _CantBuild;
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
