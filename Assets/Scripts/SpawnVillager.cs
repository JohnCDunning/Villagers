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
        Villager villager = Instantiate(_Villager, transform.position, Quaternion.identity).GetComponent<Villager>();
        SetVillagerPositionChange(villager);
    }
    void SetVillagerPositionChange(Villager villager)
    {
        villager.SetSpawnPoint(_MoveToSpawn.position);
    }
}
