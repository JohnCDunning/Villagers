using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Summary
//The town controller script will run an enemy town, controls when to spawn villagers
//what to assign the villagers role to, and build new buildings. 

public enum AIBehaviour
{
    NONE,
    AGGRESSIVE = 1
}

public class TownController : MonoBehaviour
{
    public List<VillagerController> VillagersInTown = new List<VillagerController>();
    public List<Building> BuildingsInTown = new List<Building>();

    public Transform BuildTester;
    public Transform BuildingParent;

    public GameObject _Villager;
    public GameObject[] _Buildings;
    private ResourceType _RequiredResource;
    public Transform _Spawn;

    public int wood;
    public int stone;
    public int food;

    public int _CurrentVillagerCount = 0;
    public int _MaxVillagers = 5;

    public float MinDist, MaxDist;

    public Vector3 PlaceToBuild;
    public AIBehaviour _AIBehaviour = AIBehaviour.NONE;
    private bool tryingToFindPlaceToBuild = false;

    public MarketCost _Costs;

    List<TeamSide> PotentialTarget = new List<TeamSide>();

    void SpawnVillager()
    {
        
        GameObject Villager = Instantiate(_Villager, _Spawn.position, Quaternion.identity);
        Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
        Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.wood;
        VillagersInTown.Add(Villager.GetComponent<VillagerController>());
        
        if (wood < 50)
        {
            Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
            Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.wood;
        }
        if (stone < 50)
        {
            Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
            Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.stone;
        }
        if (food < 15)
        {
            Villager.GetComponent<VillagerController>()._Task = VillagerTask.GatherResources;
            Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.food;
        }
        Villager.GetComponent<VillagerController>()._Nav.SetDestination(_Spawn.position);

        //what to do when you dont need anything
        if(wood > 50 || stone > 50 || food > 15)
        {
            int randRoll = 0;
            randRoll = Random.Range(0, 4); //make 5 to allow combat
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
                //case 4:
                    //Villager.GetComponent<VillagerController>()._Task = VillagerTask.Combat;
                    //Villager.GetComponent<VillagerController>()._WantedGoal = ResourceType.combat;
                    //break;
            }
        }
    }

    public List<GameObject> TownBuildings = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewVillager());
        StartCoroutine(PlaceBuilding());
    }

    private void Update()
    {
        switch (_AIBehaviour)
        {
            case AIBehaviour.AGGRESSIVE:
                {
                    if (VillagersInTown.Count > 6)
                    {

                        GameObject _enemyTarget = null;

                        foreach (var enemy in GameObject.FindObjectsOfType<TeamSide>())
                        {
                            if (enemy.GetComponent<VillagerController>() == null)
                                continue;

                            if (PotentialTarget.Contains(enemy))
                            {
                                if (enemy.GetComponent<VillagerController>()._Health <= 0)
                                {
                                    PotentialTarget.Remove(enemy);
                                }
                                continue;
                            }

                            if (enemy._team == Team.player)
                            {

                                PotentialTarget.Add(enemy);

                            }
                        }


                        foreach (var villager in VillagersInTown)
                        {
                            villager._Task = VillagerTask.Combat;
                            villager._WantedGoal = ResourceType.combat;
                            float min = 1000000f;
                            if (PotentialTarget.Count != 0)
                            {
                                foreach (var target in PotentialTarget)
                                {
                                    float cur = Vector3.Distance(target.transform.position, villager.transform.position);
                                    if (cur < min)
                                    {
                                        _enemyTarget = target.gameObject;
                                        min = cur;
                                    }
                                }
                            }
                            else
                            {
                                if (_enemyTarget != null)
                                {
                                    if (_enemyTarget.GetComponent<Animal>() == null)
                                    {
                                        _enemyTarget = GameObject.FindObjectOfType<Animal>().gameObject;
                                    }
                                }
                                else
                                {
                                    if (GameObject.FindObjectOfType<Animal>())
                                        _enemyTarget = GameObject.FindObjectOfType<Animal>().gameObject;
                                }

                            }
                            if (villager != null && _enemyTarget != null)
                            {
                                if (villager._ObjOfFocus != _enemyTarget.gameObject)
                                    villager._ObjOfFocus = _enemyTarget.gameObject;
                            }
                        }
                    }
                    break;
                }
            default:
                break;
        }

    }
    bool CheckIfCanAfford(Vector3 ItemToBuy) //Accesses global costs and finds out if it can afford the specific item.
    {
        if(wood >= ItemToBuy.x && stone >= ItemToBuy.y && food >= ItemToBuy.z)
        {
            //Deducts the costs
            wood -= (int)ItemToBuy.x;
            stone -= (int)ItemToBuy.y;
            food -= (int)ItemToBuy.z;
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator SpawnNewVillager()
    {
        yield return new WaitForSeconds(10);
        if (_CurrentVillagerCount < _MaxVillagers)
        {
            if (CheckIfCanAfford(_Costs._Villager) == true)
            {
                SpawnVillager();
                _CurrentVillagerCount++;

            }
        }

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
                //house logic
                if (_CurrentVillagerCount >= (_MaxVillagers * 0.6f))
                {
                    GameObject building = Instantiate(_Buildings[3], BuildPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 270), 0)),BuildingParent);
                    TownBuildings.Add(building);
                    wood -= (int)_Costs._House.x;
                    stone -= (int)_Costs._House.y;
                    food -= (int)_Costs._House.z;
                    _MaxVillagers += 5;
                }
                //Logic for how enemy to decide what to place.
                //farm
                if (food < 45 && VillagersInTown.Count > BuildingsInTown.Count)
                {
                    if (CheckIfCanAfford(_Costs._Farm))
                    {
                        GameObject building = Instantiate(_Buildings[2], BuildPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 270), 0)));
                        TownBuildings.Add(building);
                        wood -= (int)_Costs._Farm.x;
                        stone -= (int)_Costs._Farm.y;
                        food -= (int)_Costs._Farm.z;
                    }                        
                }
               
            }
        }
        StartCoroutine(PlaceBuilding());
    }
    
   
}
