using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [HideInInspector]
    public UpgradeManager _UpgradeManager;

    public GameObject _VillagerUnit;
    private void Start()
    {
        _UpgradeManager = UpgradeManager._Instance;
    }
    public void SpawnUnit()
    {
        _UpgradeManager._SelectedBuilding.GetComponentInChildren<SpawnVillager>().Spawn(_VillagerUnit);
    }
}
