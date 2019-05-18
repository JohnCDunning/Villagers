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
    [Header("Resource Variables")]
    public ResourceType _ResourceType;

    [Space]
    public int _SupplyAmmount;

    private int _OriginalAmount;
    [HideInInspector]
    public bool _SupplyBeingTaken = false;
    [HideInInspector]
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
