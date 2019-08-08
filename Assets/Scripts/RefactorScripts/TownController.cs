using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownController : MonoBehaviour
{
    public List<VillagerController> VillagersInTown = new List<VillagerController>();
    public List<Building> BuildingsInTown = new List<Building>();
    public Transform BuildTester;
    public GameObject _Villager;
    public Transform _Spawn;

    public int wood;
    public int stone;
    public int food;

    public GameObject[] _Buildings;
    public float MinDist, MaxDist;

    public Vector3 PlaceToBuild;
    private bool tryingToFindPlaceToBuild = false;
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

    public List<GameObject> TownBuildings = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewVillager());
        StartCoroutine(PlaceBuilding());
    }

    IEnumerator SpawnNewVillager()
    {
        yield return new WaitForSeconds(10);
        SpawnVillager();
        StartCoroutine(SpawnNewVillager());
    }
    IEnumerator PlaceBuilding()
    {
        GameObject SelectedBuilding = _Buildings[Random.Range(0, _Buildings.Length)];
        GameObject SelectedCurrentBuilding = BuildingsInTown[Random.Range(0, BuildingsInTown.Count)].gameObject;
        float buildSize = SelectedBuilding.GetComponent<Building>()._BuildingDetails._ConstructionSize /2;
        BuildTester.localScale = new Vector3(buildSize, buildSize, buildSize);
        yield return new WaitForSeconds(1);
        float xpos, ypos, zpos;
        xpos = SelectedCurrentBuilding.transform.position.x;
        ypos = SelectedCurrentBuilding.transform.position.y;
        zpos = SelectedCurrentBuilding.transform.position.z;

        Vector3 BuildPos = new Vector3(Random.insideUnitCircle.x * MaxDist + xpos, ypos, Random.insideUnitCircle.y * MaxDist + zpos);
        if (Vector3.Distance(BuildPos, SelectedBuilding.transform.position)> MinDist)
        {
            BuildTester.transform.position = BuildPos;
            yield return new WaitForSeconds(0.5f);
            if (BuildTester.GetComponent<BuildTest>().collisionCount == 0)
            {
                GameObject building = Instantiate(SelectedBuilding, BuildPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 270), 0)));
                TownBuildings.Add(building);
            }
        }
        
        
        StartCoroutine(PlaceBuilding());
    }
    
   
}
