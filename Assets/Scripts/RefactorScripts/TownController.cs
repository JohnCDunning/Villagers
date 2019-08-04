using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownController : MonoBehaviour
{
    public List<VillagerController> VillagersInTown = new List<VillagerController>();
  
    public GameObject _Villager;
    public Transform _Spawn;

    void SpawnVillager()
    {
        GameObject Villager = Instantiate(_Villager, _Spawn.position, Quaternion.identity);
        Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
        Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.wood;

        int randRoll = 0;
        randRoll = Random.Range(0, 5);
        switch (randRoll)
        {
            case 1:
                Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
                Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.wood;
                break;
            case 2:
                Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
                Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.stone;
                break;
            case 3:
                Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
                Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.food;
                break;
            case 4:
                Villager.GetComponent<VillagerController>()._Task = VillagerTask.Combat;
                Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.combat;
                break;
        }

        Villager.GetComponent<VillagerController>()._Nav.SetDestination(_Spawn.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewVillager());
    }

    IEnumerator SpawnNewVillager()
    {
        yield return new WaitForSeconds(10);
        SpawnVillager();
        StartCoroutine(SpawnNewVillager());
    }
}
