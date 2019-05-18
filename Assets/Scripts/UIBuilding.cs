using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIBuilding : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public BuildingObject _BuildingDetails;
    public GameObject _Description;
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

    }
    //Shows the object the user is about to place
    public void DisplayConstructionHoligram()
    {
        CanBuildCheck _BuildCheck = ConstructObject._Instance._ContructionSphere.GetComponent<CanBuildCheck>();
        if (_BuildCheck != _BuildingDetails._ConstructionPrefab)
        {
            Destroy(_BuildCheck._ConstructionBuilding);
            GameObject _Building = Instantiate(_BuildingDetails._ConstructionPrefab, Vector3.zero, _BuildingDetails._ConstructionPrefab.transform.rotation, _BuildCheck.transform);
            _Building.transform.localPosition = Vector3.zero;
            _BuildCheck._ConstructionBuilding = _Building;
            _BuildCheck._BuildingToSpawn = _BuildingDetails._ObjectPrefab;
        }
    }
}
