using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstructObject : MonoBehaviour
{
    public static ConstructObject _Instance;
    public GameObject _House;
    public Transform _ContructionSphere;
    private GameObject _HologramBuilding;
    public GameObject _BuildCanvas;
    private CanBuildCheck _BuildCheck;

    public Villager _SelectedVillager;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        _BuildCheck = _ContructionSphere.GetComponent<CanBuildCheck>();
    }

    void HighlightVillager()
    {
        if (ObjectRaycast().GetComponent<Villager>() != null)
        {

            Villager _Villager = ObjectRaycast().GetComponent<Villager>();
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
        if(ObjectRaycast().tag == "Ground")
        {
            _SelectedVillager._Outline.SetActive(false);
            _SelectedVillager = null;
        }
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            HighlightVillager();
        }

        _ContructionSphere.transform.position = LocationToBuild();
        if (Input.GetMouseButtonDown(0) && _BuildCheck._ConstructionBuilding != null && _BuildCheck.CanBuild)
        {
            GameObject newBuilding = Instantiate(_BuildCheck._BuildingToSpawn, _BuildCheck.transform.position, _BuildCheck._BuildingToSpawn.transform.rotation);
            Destroy(_BuildCheck._ConstructionBuilding);
            _BuildCheck._ConstructionBuilding = null;
            _ContructionSphere.gameObject.SetActive(false);
        }
    }
   
    GameObject ObjectRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            pos = hit.point;
            return hit.transform.gameObject;

        }
        else
        {
            return hit.transform.gameObject;
        }

    }
    Vector3 LocationToBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            pos = hit.point;
            return hit.point;
            
        }
        else
        {
            return Vector3.zero;
        }
     
    }
}
