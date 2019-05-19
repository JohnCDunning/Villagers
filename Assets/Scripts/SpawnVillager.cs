using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVillager : MonoBehaviour
{
    public GameObject _Villager;
    public Transform _MoveToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_Villager, transform.position, Quaternion.identity);
        Invoke("SetVillagerPositionChange", 1);
    }
    void SetVillagerPositionChange()
    {
        _Villager.GetComponent<Villager>().SetSpawnPoint(_MoveToSpawn.position);
    }
}
