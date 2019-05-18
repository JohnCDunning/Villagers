using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstructObject : MonoBehaviour
{
    public static ConstructObject _Instance;
    
    public Transform _ContructionSphere;
    private CanBuildCheck _BuildCheck;
    [Space]
    public RaycastInfo _RayInfo;

    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        _BuildCheck = _ContructionSphere.GetComponent<CanBuildCheck>();
    }
    private void Update()
    {
        _ContructionSphere.transform.position = _RayInfo.LocationToBuild();
        if (Input.GetMouseButtonDown(0) && _BuildCheck._ConstructionBuilding != null && _BuildCheck.CanBuild)
        {
            GameObject newBuilding = Instantiate(_BuildCheck._BuildingToSpawn, _BuildCheck.transform.position, _BuildCheck._BuildingToSpawn.transform.rotation);
            Destroy(_BuildCheck._ConstructionBuilding);
            _BuildCheck._ConstructionBuilding = null;
            _ContructionSphere.gameObject.SetActive(false);
        }
    }
}
