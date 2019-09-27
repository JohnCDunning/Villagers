using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstructObject : MonoBehaviour
{
    public static ConstructObject _Instance;
    
    public Transform _ContructionSphere;
    public Transform _BuildingParent;
    private CanBuildCheck _BuildCheck;
    [Space]
    public RaycastInfo _RayInfo;
    public InputManager _Input;

    private bool _CanPlaceObject = false;
    public bool _RotatingObject = false;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        _BuildCheck = _ContructionSphere.GetComponent<CanBuildCheck>();
    }
    public void StartTimer()
    {
        StartCoroutine(CanPlaceObject());
    }
    IEnumerator CanPlaceObject()
    {
        yield return new WaitForSeconds(0.4f);
        _CanPlaceObject = true;
    }
    private void Update()
    {
        //Default Update position
        if (_RotatingObject == false)
        {
            _ContructionSphere.transform.position = _RayInfo.LocationToBuild();
        }
        else
        {
            Vector3 pos = _RayInfo.LocationToBuild();
            _ContructionSphere.transform.LookAt(new Vector3(pos.x,_ContructionSphere.position.y,pos.z));
        }
        if (_BuildCheck.CanBuild == true)
        {
            if (Input.GetMouseButtonUp(0) && _CanPlaceObject == true)
            {
                _RotatingObject = true;
            }
            if (Input.GetMouseButtonDown(0) && _RotatingObject == true)
            {
                
                PlaceNewBuilding();
                
            }
        }
    }
    //Finalize building
    void PlaceNewBuilding()
    {
        GameObject newBuilding = Instantiate(_BuildCheck._BuildingToSpawn, _BuildCheck.transform.position, _ContructionSphere.transform.rotation, _BuildingParent);
        Destroy(_BuildCheck._ConstructionBuilding);
        _BuildCheck._ConstructionBuilding = null;
        _ContructionSphere.gameObject.SetActive(false);

        _CanPlaceObject = false;
        _RotatingObject = false;
    }
}
