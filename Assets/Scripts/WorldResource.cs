using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldResource : MonoBehaviour
{
    [Header("Resource Variables")]
    public ResourceType _ResourceType;

    [Space]
    public int _SupplyAmount;

    private int _OriginalAmount;
    [HideInInspector]
    public bool _SupplyBeingTaken = false;
    [HideInInspector]
    public GameObject _VillagerTravelingToThis;

    public GameObject _Outline;

    private void Start()
    {
        _OriginalAmount = _SupplyAmount;
    }
    private void Update()
    {
        if (_ResourceType == ResourceType.wood)
        {
            if (_SupplyAmount <= _OriginalAmount / 2)
            {
                GetComponentInChildren<Rigidbody>().isKinematic = false;
            }
        }
    }

}
