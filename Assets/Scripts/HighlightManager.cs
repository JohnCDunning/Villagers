using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public InputManager _Input;
    public RaycastInfo _RayInfo;
    public UpgradeManager _UpgradeManager;

    ISelectable _CurrentlySelectedObject;
    
    private void Update()
    {
        if (_Input.LeftMouseDown())
        {
            //Highlight if hits a selectable object
            if (_RayInfo.ObjectRaycast().GetComponent<ISelectable>() != null)
            {
                SelectObject(_RayInfo.ObjectRaycast().GetComponent<ISelectable>());
                return;
            }
            //Unselect if hits nothing
            if (_UpgradeManager._BuildingUpgradeUI.GetComponent<UIMouseCheck>()._MouseEntered == false) //So clicking on UI doesnt deselect objects
            {
                if(_CurrentlySelectedObject != null)
                    _CurrentlySelectedObject.UnSelect();
            }
        }
        if (_Input.RightMouseDown())
        {
            if (_RayInfo.ObjectRaycast().GetComponent<ISelectable>() != null)
            {
                InteractObject(_RayInfo.ObjectRaycast().GetComponent<ISelectable>());
            }
        }
    }
    void InteractObject(ISelectable SelectableObject)
    {
        if(SelectableObject != _CurrentlySelectedObject)
            SelectableObject.InteractSelect();
    }
    void SelectObject(ISelectable SelectableObject)
    {
        if(_CurrentlySelectedObject != null)
            _CurrentlySelectedObject.UnSelect();

        _CurrentlySelectedObject = SelectableObject;
        _CurrentlySelectedObject.Select();
    }
    
}
