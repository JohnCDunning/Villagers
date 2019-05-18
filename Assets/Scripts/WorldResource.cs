using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    wood,
    stone
}
public class WorldResource : MonoBehaviour
{
    
    public ResourceType _ResourceType;
    public int _SupplyAmmount;
    private int _OriginalAmount;
    public bool _SupplyBeingTaken = false;
    public GameObject _VillagerTravelingToThis;

    private void Start()
    {
        _OriginalAmount = _SupplyAmmount;
    }
    private void Update()
    {
        if (_ResourceType == ResourceType.wood)
        {
            if (_SupplyAmmount <= _OriginalAmount / 2)
            {
                GetComponentInChildren<Rigidbody>().isKinematic = false;
            }
        }
    }

}
