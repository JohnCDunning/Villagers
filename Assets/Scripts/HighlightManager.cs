using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public InputManager _Input;
    public RaycastInfo _RayInfo;

    public List<ISelectable> _MultiSelectedVillagers = new List<ISelectable>();
    public ISelectable _CurrentlySelectedObject;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClickSelection();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickSelection();
        }
    }
    void InteractObject(ISelectable SelectableObject)
    {
        if(SelectableObject != _CurrentlySelectedObject)
            SelectableObject.InteractSelect();
    }
    void LeftClickSelection()
    {
        //Highlight if hits a selectable object
        if (_RayInfo.ObjectRaycast().GetComponent<ISelectable>() != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (_RayInfo.ObjectRaycast().GetComponent<TeamSide>()._team == Team.player)
                {
                    _MultiSelectedVillagers.Add(_RayInfo.ObjectRaycast().GetComponent<ISelectable>());
                    foreach (ISelectable selectable in _MultiSelectedVillagers)
                    {
                        SelectObject(selectable, false);
                    }
                }
                return;
            }
            foreach (ISelectable selectable in _MultiSelectedVillagers)
            {
                selectable.UnSelect();
            }
            _MultiSelectedVillagers.Clear();
            _CurrentlySelectedObject = null;
            if (_RayInfo.ObjectRaycast().GetComponent<TeamSide>() != null)
            {
                if (_RayInfo.ObjectRaycast().GetComponent<TeamSide>()._team == Team.player)
                {
                    SelectObject(_RayInfo.ObjectRaycast().GetComponent<ISelectable>(), true);
                    _MultiSelectedVillagers.Add(_RayInfo.ObjectRaycast().GetComponent<ISelectable>());
                }
            }
            return;
        }
        if (_CurrentlySelectedObject != null)
        {
            _CurrentlySelectedObject.UnSelect();
            _CurrentlySelectedObject = null;
        }
        foreach (ISelectable selectable in _MultiSelectedVillagers)
        {
            selectable.UnSelect();
        }
        _MultiSelectedVillagers.Clear();


    }
    void SelectObject(ISelectable SelectableObject,bool UnSelectCurrent)
    {
        if (UnSelectCurrent == true)
        {
            if (_CurrentlySelectedObject != null)
            {
                _CurrentlySelectedObject.UnSelect();
                _MultiSelectedVillagers.Add(_CurrentlySelectedObject);
            }
        }
        _CurrentlySelectedObject = SelectableObject;
        _CurrentlySelectedObject.Select();

        
    }
    void RightClickSelection()
    {
        if (_RayInfo.ObjectRaycast().GetComponent<ISelectable>() != null)
        {
            ISelectable selectObject = _RayInfo.ObjectRaycast().GetComponent<ISelectable>();
            selectObject.GetThisObject();
            InteractObject(selectObject);
            //Currently selected object will attempt to interact with the object
            if(_CurrentlySelectedObject != null)
            {
                _CurrentlySelectedObject.InteractWithObject(selectObject);
                foreach (ISelectable selectable in _MultiSelectedVillagers)
                {
                    selectable.InteractWithObject(selectObject);
                }
                return; 
            }
        }
        if (_CurrentlySelectedObject != null)
        {
            _CurrentlySelectedObject.InteractWithLocation(_RayInfo.LocationToBuild());
            foreach (ISelectable selectable in _MultiSelectedVillagers)
            {
                selectable.InteractWithLocation(_RayInfo.LocationToBuild());
            }
        }
    }
    
}
