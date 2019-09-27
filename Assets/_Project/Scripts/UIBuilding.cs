using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIBuilding : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public BuildingDetails _BuildingDetails;
    public CollectedResources _CurrentResources;

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        //_Description.SetActive(true);
        //_Description.GetComponent<BuildingDescription>()._Description = _BuildingDetails;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //_Description.SetActive(false);
    }
    void Update()
    {
     
        if(_BuildingDetails._WoodCost <= _CurrentResources._CollectedWood && _BuildingDetails._StoneCost <= _CurrentResources._CollectedStone && _BuildingDetails._FoodCost <= _CurrentResources._CollectedFood)
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


        CanBuildCheck _BuildCheck = ConstructObject._Instance._ContructionSphere.GetComponent<CanBuildCheck>();

        if (_BuildCheck != _BuildingDetails._ConstructionPrefab)
        {
            //Sets the construction size for this building;
            _BuildCheck.transform.localScale = new Vector3(_BuildingDetails._ConstructionSize, _BuildingDetails._ConstructionSize, _BuildingDetails._ConstructionSize);
            Destroy(_BuildCheck._ConstructionBuilding);

            GameObject _Building = Instantiate(_BuildingDetails._ConstructionPrefab, Vector3.zero, _BuildingDetails._ConstructionPrefab.transform.rotation, _BuildCheck.transform);
            _Building.transform.localPosition = Vector3.zero;
            _Building.transform.localRotation = Quaternion.identity;
            _Building.transform.localScale = Vector3.one / _BuildingDetails._ConstructionSize; //To correct the over scaled parent object - Over scaled for bigger hitbox

            _BuildCheck._ConstructionBuilding = _Building;
            _BuildCheck._BuildingToSpawn = _BuildingDetails._ObjectPrefab;
        }
    }
}
