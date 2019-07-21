using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVillager : MonoBehaviour
{
    public Transform _MoveTo;
    public GameObject _Villager;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Villager = Instantiate(_Villager, transform.position, Quaternion.identity);
        Villager.GetComponent<VillagerController>()._Nav.SetDestination(_MoveTo.position);
    }
    
}
