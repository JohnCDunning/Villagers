using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public InputManager _Input;
    public RaycastInfo _RayInfo;
    public UpgradeManager _UpgradeManager;

    Villager _SelectedVillager;
    Building _SelectedBuilding;

    
    private void Update()
    {
        if(_SelectedBuilding != null)
        {
            _UpgradeManager._SelectedBuilding = _SelectedBuilding.gameObject;
        }
        else
        {
            _UpgradeManager._SelectedBuilding = null;
        }

        if (_Input.LeftMouseDown())
        {
            HighlightVillager();
            HighlightBuilding();
          
            if (_UpgradeManager._BuildingUpgradeUI.GetComponent<UIMouseCheck>()._MouseEntered == false) //So clicking on UI doesnt deselect objects
            {
                if (_RayInfo.ObjectRaycast().GetComponent<Building>() == null)
                {
                    CancelBuildingSelection();

                }
                if (_RayInfo.ObjectRaycast().GetComponent<Villager>() == null)
                {
                    CancelVillagerSelection();
                }
            }
        }
        if (_Input.RightMouseDown())
        {
            WorldResourceOutlineAnimation();
            CancelVillagerSelection();
        }

        if(_SelectedVillager != null)
        {
            _SelectedVillager._Outline.SetActive(true);
        }
        if (_SelectedBuilding != null)
        {
            _SelectedBuilding._Outline.SetActive(true);
        }
    }
    
    void HighlightBuilding()
    {
        if (_RayInfo.ObjectRaycast().GetComponent<Building>() != null)
        {
            Building _Building = _RayInfo.ObjectRaycast().GetComponent<Building>();
            if(_Building != _SelectedBuilding)
            {
                if(_SelectedBuilding != null)
                {
                    _SelectedBuilding._Outline.SetActive(false);
                }
                
                _SelectedBuilding = _Building;
                _UpgradeManager.ApplyUpgrades(_Building.gameObject);
            }
        }
    }
    void HighlightVillager()
    {
        if (_RayInfo.ObjectRaycast().GetComponent<Villager>() != null)
        {
            Villager _Villager = _RayInfo.ObjectRaycast().GetComponent<Villager>();
            if (_Villager != _SelectedVillager)
            {
                if (_SelectedVillager != null)
                {
                    _SelectedVillager._Outline.SetActive(false);
                }
                _SelectedVillager = _Villager;
            }
        }

    }
    public void CancelVillagerSelection()
    {
        if (_SelectedVillager != null)
        {
            _SelectedVillager._Outline.SetActive(false);
            _SelectedVillager = null;
        }
    }
    public void CancelBuildingSelection()
    {
        if (_SelectedBuilding != null)
        {
            _SelectedBuilding._Outline.SetActive(false);
            _SelectedBuilding = null;
        }
    }
    public void SetVillagerTask(int _TaskNumber)
    {
        _SelectedVillager.NewTask(_TaskNumber);

    }
    void WorldResourceOutlineAnimation()
    {
        if (_RayInfo.ObjectRaycast().GetComponent<WorldResource>() == true && _SelectedVillager != null)
        {
            WorldResource _Object = _RayInfo.ObjectRaycast().GetComponent<WorldResource>();
            if (_Object._ResourceType == ResourceType.wood)
            {
                _Object._Outline.GetComponent<Animator>().SetTrigger("ShowOutline");
                SetVillagerTask(1);
            }
            if (_Object._ResourceType == ResourceType.stone)
            {
                _Object._Outline.GetComponent<Animator>().SetTrigger("ShowOutline");
                SetVillagerTask(2);
            }
            if (_Object._ResourceType == ResourceType.food)
            {
                _Object._Outline.GetComponent<Animator>().SetTrigger("ShowOutline");
                SetVillagerTask(3);
            }
        }
        CancelVillagerSelection();
    }
}
