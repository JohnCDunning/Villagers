﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldResource : MonoBehaviour, ISelectable,ITakeDamage
{
    public ResourceType _ResourceType;

    [Space]
    public int _SupplyAmount;
    private int _OriginalAmount;

    [HideInInspector] public bool _SupplyBeingTaken = false;
    [HideInInspector] public GameObject _VillagerTravelingToThis;

    public GameObject _AnimatedOutline;
    public GameObject _Outline;

    #region Interfaces
    public void TakeDamage(int damage)
    {
        _SupplyAmount -= damage;
    }
    public void Select()
    {
        _Outline.SetActive(true);
    }
    public void UnSelect()
    {
        _Outline.SetActive(false);
    }
    public void InteractSelect()
    {
        _AnimatedOutline.GetComponent<Animator>().SetTrigger("ShowOutline");
    }
    #endregion

    private void Start()
    {
        _OriginalAmount = _SupplyAmount;
    }
}
