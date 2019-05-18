using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectOfInterest : MonoBehaviour
{
   
    [Header("Resource Lists")]
    public List<WorldResource> _WoodSupplies = new List<WorldResource>();
    public List<WorldResource> _StoneSupplies = new List<WorldResource>();
    
    void Awake()
    {
        //Finds every wood in the world and adds to a list
        #region Find Wood
        WorldResource[] wood = FindObjectsOfType<WorldResource>();
        foreach (WorldResource _wood in wood)
        {
            if(_wood._ResourceType == ResourceType.wood)
            {
                _WoodSupplies.Add(_wood);
            }
        }
        #endregion
        #region Find Stone
        WorldResource[] stone = FindObjectsOfType<WorldResource>();
        foreach (WorldResource _stone in stone)
        {
            if (_stone._ResourceType == ResourceType.stone)
            {
                _StoneSupplies.Add(_stone);
            }
        }
        #endregion

    }
   
    #region Delete world Resource
    public void DeleteWorldResource(WorldResource _Resource)
    {
        if (_WoodSupplies.Contains(_Resource))
        {
            _WoodSupplies.Remove(_Resource);
        }
        if (_StoneSupplies.Contains(_Resource))
        {
            _StoneSupplies.Remove(_Resource);
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
                    if (_Supply._SupplyBeingTaken == false)
                    {
                        ClosestDistance = Distance;
                        _ClosestResource = _Supply;
                    }
                }
            }
        }
        if(_ClosestResource != null)
        {
            _ClosestResource._VillagerTravelingToThis = Villager;
            _ClosestResource._SupplyBeingTaken = true;
        }
        return _ClosestResource;
    }
    #endregion

   
    
}
