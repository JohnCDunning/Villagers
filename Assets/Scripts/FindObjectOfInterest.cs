using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectOfInterest : MonoBehaviour
{
   
    [Header("Resource Lists")]
    public List<WorldResource> _WoodSupplies = new List<WorldResource>();
    [Header("Building Lists")]
    public List<Building> _ResourceCollection = new List<Building>();

    #region RefreshAllLists
    public void RefreshLists()
    {
        //Clear All Lists
        _WoodSupplies.Clear();
        _ResourceCollection.Clear();

        //Resources
        #region Find Resources
        WorldResource[] resource = FindObjectsOfType<WorldResource>();
        foreach (WorldResource _resource in resource)
        {
            if (_resource._ResourceType == ResourceType.wood)
            {
                _WoodSupplies.Add(_resource);
            }
        }
        #endregion
        
        //Buildings
        #region Find Buildings
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building _Building in buildings)
        {
            //Add building to resource collection list
            if (_Building._BuildingType == BuildingType.ResourceCollection)
            {
                _ResourceCollection.Add(_Building);
            }
        }
        #endregion
        
    }
    #endregion
    void Awake()
    {
        //Finds every wood in the world and adds to a list
        RefreshLists();
        InvokeRepeating("RefreshLists", 0, 1);

    }
   
    #region Delete world Resource
    public void DeleteWorldResource(WorldResource _Resource)
    {
        if (_WoodSupplies.Contains(_Resource))
        {
            _WoodSupplies.Remove(_Resource);
        }
    }
    #endregion

    #region Find Closest world resource
    public WorldResource ClosestResourceOfInterest(List<WorldResource> _ResourceToFind,Vector3 VillagerPosition, GameObject Villager)
    {
        float ClosestDistance = 50;
        WorldResource _ClosestResource = null;

        foreach(WorldResource _Supply in _ResourceToFind)
        {
            if (_Supply != null)
            {
                float Distance = Vector3.Distance(VillagerPosition, _Supply.transform.position);
                if (Distance < ClosestDistance)
                {
                    if (_Supply._SupplyBeingTaken == true)
                    {
                        continue;
                    }
                    else
                    {
                        ClosestDistance = Distance;
                        _ClosestResource = _Supply;
                    }
                }
            }
        }
        return _ClosestResource;
    }
    #endregion

    #region Find Closest building
    public Building ClosestBuildingOfInterest(List<Building> _BuildingToFind, Vector3 VillagerPosition)
    {
        float ClosestDistance = 50;
        Building _ClosestBuilding = null;

        foreach (Building _Building in _BuildingToFind)
        {

            if (_Building != null)
            {
                float Distance = Vector3.Distance(VillagerPosition, _Building.transform.position);
                if (Distance < ClosestDistance)
                {
                    ClosestDistance = Distance;
                    _ClosestBuilding = _Building;
                }
            }
        }
        return _ClosestBuilding;
    }
    #endregion

}
