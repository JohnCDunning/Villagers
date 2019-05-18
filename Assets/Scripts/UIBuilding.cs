﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIBuilding : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public BuildingObject _BuildingDetails;
    public GameObject _Description;
    public CollectedResources _CurrentResources;

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        _Description.SetActive(true);
        _Description.GetComponent<BuildingDescription>()._Description = _BuildingDetails;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _Description.SetActive(false);
    }
    void Update()
    {
        //NOT COMPATIBLE WITH FOOD YET
        if(_BuildingDetails._WoodCost < _CurrentResources._CollectedWood && _BuildingDetails._StoneCost < _CurrentResources._CollectedStone)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    //Shows the object the user is about to place
    public void DisplayConstructionHoligram()
    {
        _CurrentResources._CollectedWood -= _BuildingDetails._WoodCost;
        _CurrentResources._CollectedStone -= _BuildingDetails._StoneCost;

        CanBuildCheck _BuildCheck = ConstructObject._Instance._ContructionSphere.GetComponent<CanBuildCheck>();

        if (_BuildCheck != _BuildingDetails._ConstructionPrefab)
        {
            //Sets the construction size for this building;
            _BuildCheck.transform.localScale = new Vector3(_BuildingDetails._ConstructionSize, _BuildingDetails._ConstructionSize, _BuildingDetails._ConstructionSize);
            Destroy(_BuildCheck._ConstructionBuilding);

            GameObject _Building = Instantiate(_BuildingDetails._ConstructionPrefab, Vector3.zero, _BuildingDetails._ConstructionPrefab.transform.rotation, _BuildCheck.transform);
            _Building.transform.localPosition = Vector3.zero;
            _Building.transform.localScale = Vector3.one / _BuildingDetails._ConstructionSize;

            _BuildCheck._ConstructionBuilding = _Building;
            _BuildCheck._BuildingToSpawn = _BuildingDetails._ObjectPrefab;
        }
    }
}
