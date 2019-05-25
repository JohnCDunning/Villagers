using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject _SelectedBuilding;
    public GameObject _BuildingUpgradeUI;
    private bool _AppliedUpgrades = false;
    public static UpgradeManager _Instance;

    private void Start()
    {
        _Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (_SelectedBuilding == null)
        {
            _BuildingUpgradeUI.SetActive(false);
        }
        else
        {
            _BuildingUpgradeUI.SetActive(true);
        }
        

    }

    public void ClearUpgrades()
    {

        List<GameObject> upgradeObjects = new List<GameObject>();
       
        int _UpgradeCount = _BuildingUpgradeUI.transform.childCount;
        for(int i = 0; i < _UpgradeCount; i++)
        {
            upgradeObjects.Add(_BuildingUpgradeUI.transform.GetChild(i).gameObject);
        }
        foreach(GameObject upgrade in upgradeObjects)
        {
            Destroy(upgrade);
        }
        _AppliedUpgrades = false;
    }
    public void ApplyUpgrades(GameObject _Building)
    {
        _SelectedBuilding = _Building;
        ClearUpgrades();

        foreach (GameObject _Uprade in _Building.GetComponent<Building>()._BuildingDetails._Upgrades)
        {
            Instantiate(_Uprade, _BuildingUpgradeUI.transform);
        }
    }
}
