using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectOfInterest : MonoBehaviour
{
   
    [Header("Resource Lists")]
    public List<WorldResource> _WoodSupplies = new List<WorldResource>();
    public List<WorldResource> _StoneSupplies = new List<WorldResource>();
    public List<WorldResource> _FoodSupplies = new List<WorldResource>();
    
    [Header("Building Lists")]
    public List<Building> _ResourceCollection = new List<Building>();
    [Header("Villager Lists")]
    public List<VillagerController> _Villagers = new List<VillagerController>();
    #region RefreshAllLists
    public void RefreshLists()
    {
        //Clear All Lists
        _WoodSupplies.Clear();
        _StoneSupplies.Clear();
        _FoodSupplies.Clear();
        _ResourceCollection.Clear();
        _Villagers.Clear();

        //Resources
        #region Find Resources
        WorldResource[] resource = FindObjectsOfType<WorldResource>();
        foreach (WorldResource _resource in resource)
        {
            if (_resource._ResourceType == ResourceType.wood)
            {
                _WoodSupplies.Add(_resource);
            }
            if (_resource._ResourceType == ResourceType.stone)
            {
                _StoneSupplies.Add(_resource);
            }
            if (_resource._ResourceType == ResourceType.food)
            {
                _FoodSupplies.Add(_resource);
            }
            
        }
        #endregion
        
        //Buildings
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building _Building in buildings)
        {
            //Add building to resource collection list
            if (_Building._BuildingType == BuildingType.ResourceCollection)
            {
                _ResourceCollection.Add(_Building);
            }
        }
        VillagerController[] villagers = FindObjectsOfType<VillagerController>();
        foreach (VillagerController _VillagerPerson in villagers)
        {
            _Villagers.Add(_VillagerPerson);
        }
    }
    #endregion
    void Awake()
    {
        //Finds every wood in the world and adds to a list
        RefreshLists();
        InvokeRepeating("RefreshLists", 0, 1);

    }
   
 
    public void DeleteWorldResource(WorldResource _Resource)
    {
        if (_WoodSupplies.Contains(_Resource))
        {
            _WoodSupplies.Remove(_Resource);
        }
    }

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
    public VillagerController ClosestEnemyVillager(List<VillagerController> _VillagerToFind, GameObject AgressiveVillager)
    {
        float ClosestDistance = 50;
        VillagerController _ClosestVillager = null;

        foreach (VillagerController _Villager in _VillagerToFind)
        {
            if (_Villager != null)
            {
                if (_Villager.GetComponent<TeamSide>()._team != AgressiveVillager.GetComponent<TeamSide>()._team && _Villager.gameObject != AgressiveVillager)
                {
                    float Distance = Vector3.Distance(AgressiveVillager.transform.position, _Villager.transform.position);
                    if (Distance < ClosestDistance)
                    {
                        ClosestDistance = Distance;
                        _ClosestVillager = _Villager;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        return _ClosestVillager;
    }

}
