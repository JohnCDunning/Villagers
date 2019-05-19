using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public InputManager _Input;
    public RaycastInfo _RayInfo;
    Villager _SelectedVillager;

    private void Update()
    {
        if (_Input.LeftMouseDown())
        {
            HighlightVillager();
        }
        if (_Input.RightMouseDown())
        {
            WorldResourceOutlineAnimation();
            CancelVillagerSelection();
        }
    }
    void HighlightVillager()
    {

        if (_RayInfo.ObjectRaycast().GetComponent<Villager>() != null)
        {
            Villager _Villager = _RayInfo.ObjectRaycast().GetComponent<Villager>();
            if (_Villager != _SelectedVillager)
            {
                _Villager._Outline.SetActive(true);
               
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
