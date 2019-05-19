using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Villager : MonoBehaviour
{
    [Header("Villager Task")]
    public VillagerTask _Task;
    bool Started = false;
    [Header("Resources")]
    public int _WoodHeld;
    public int _StoneHeld;

   
    public VillagerTask _PreviousTask;

    public NavMeshAgent _Nav;
    [HideInInspector]
    public WorldResource _ResourceOfInterest;

    private bool _CollectingResource = false;
    [HideInInspector]
    public bool _ReturningGoods = false;

    [Space]
    private FindObjectOfInterest _FindObject;
    
    private Vector3 OldPosition;

    [Header("Tools")]
    public GameObject _Axe;
    public GameObject _Pickaxe;
    [Header("GatheredResources")]
    public GameObject _Wood;
    public GameObject _Stone;
    [Space]
    public GameObject _Outline;
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(Random.Range(0, 2f));
        Started = true;
    }
    void Awake()
    {
       
        _FindObject = FindObjectOfType<FindObjectOfInterest>();
    }
    void Start()
    {
        StartCoroutine(StartDelay());
    }
    public void SetSpawnPoint(Vector3 _Spawn)
    {
        while (_Nav.isOnNavMesh == true)
        {
            _Nav.destination = _Spawn;
        }
    }
    public void SetTaskFromUI(int _TaskNumber)
    {
        
        if (_ResourceOfInterest != null)
        {
            _ReturningGoods = false;
            _ResourceOfInterest._VillagerTravelingToThis = null;
            _ResourceOfInterest._SupplyBeingTaken = false;
            _ResourceOfInterest = null;
        }



        switch (_TaskNumber)
        {
            case 1:
                _Task = VillagerTask.Gather_Wood;
                break;
            case 2:
                _Task = VillagerTask.Gather_Stone;
                break;
            case 3:
                _Task = VillagerTask.Hunt_Food;
                break;
            
        }
    }

    void Update()
    {
        


        //Run SetTask
        if (Started)
        {
            SetTask();
        }
        if(_StoneHeld >= 20 || _WoodHeld >= 20)
        {
            StopCoroutine(CollectResource(_ResourceOfInterest));
            if (_ReturningGoods == false)
            {
                CancelTool();
                _PreviousTask = _Task;
                _Task = VillagerTask.ReturnGoods;
                _ReturningGoods = true;
            }
        }
    }
    #region IsObjectWithinHittingDistance
    bool _ObjectOfInterestIsClose()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    //Set all tasks depending on villager role
    #region SetTask
    void SetTask()
    {
        switch (_Task)
        {
            case VillagerTask.Gather_Wood:
                _Axe.SetActive(true); _Pickaxe.SetActive(false);
                _Stone.SetActive(false); _Wood.SetActive(false);
                Gather_Wood();
                break;
            case VillagerTask.Gather_Stone:

                _Axe.SetActive(false); _Pickaxe.SetActive(true);
                _Stone.SetActive(false); _Wood.SetActive(false);
                Gather_Stone();
                break;
            case VillagerTask.Hunt_Food:
                Hunt_Food();
                break;
            case VillagerTask.Farm:
                Farm();
                break;
            case VillagerTask.ReturnGoods:
                _Axe.SetActive(false); _Pickaxe.SetActive(false);
                switch (_PreviousTask)
                {
                    case VillagerTask.Gather_Wood:
                        _Stone.SetActive(false); _Wood.SetActive(true);
                        break;
                    case VillagerTask.Gather_Stone:
                        _Stone.SetActive(true); _Wood.SetActive(false);
                        break;
                }
                ReturnGoods();
                break;
            case VillagerTask.Sleep:
                Sleep();
                break;
        }
    }
    #endregion
    #region Can the villager gather the resource
    void CanIGatherResource()
    {
        if (_ResourceOfInterest != null)
        {
            _Nav.destination = _ResourceOfInterest.transform.position;
            //If the villager is within range. Start collecting
            if (Vector3.Distance(transform.position, _ResourceOfInterest.transform.position) < 2 && _CollectingResource == false)
            {
                LookAtObjectOfInterest();
                if (_ObjectOfInterestIsClose())
                {
                    StartCoroutine(CollectResource(_ResourceOfInterest));
                }
            }
        }
    }
    #endregion
    #region LookAtObjectOfInterest
    void LookAtObjectOfInterest()
    {
        Quaternion lookOnLook = Quaternion.LookRotation(_ResourceOfInterest.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 1.5f *Time.deltaTime);
    }
    #endregion
    #region CollectingResourceCoRoutine
    IEnumerator CollectResource(WorldResource _ResourceOfInterest)
    {
        _CollectingResource = true;
        if (_Task != VillagerTask.ReturnGoods)
        {
            //Reduces wood amount
            while (_ResourceOfInterest._SupplyAmmount > 0)
            {
                if (_Task == VillagerTask.ReturnGoods)
                {
                    break;
                }
                yield return new WaitForSeconds(0.6f);
                UseTool();
                if ((_ResourceOfInterest._SupplyAmmount - 5) > 0)
                {
                    switch (_ResourceOfInterest._ResourceType)
                    {
                        case ResourceType.wood:
                            CollectedResources._Instance._CollectedWood += 5;
                            _WoodHeld += 5;
                            break;
                        case ResourceType.stone:
                            CollectedResources._Instance._CollectedStone += 5;
                            _StoneHeld += 5;
                            break;
                    }
                    _ResourceOfInterest._SupplyAmmount -= 5;
                }
                else
                {
                    switch (_ResourceOfInterest._ResourceType)
                    {
                        case ResourceType.wood:
                            CollectedResources._Instance._CollectedWood += _ResourceOfInterest._SupplyAmmount;
                            _WoodHeld += _ResourceOfInterest._SupplyAmmount;
                            break;
                        case ResourceType.stone:
                            CollectedResources._Instance._CollectedStone += _ResourceOfInterest._SupplyAmmount;
                            _StoneHeld += _ResourceOfInterest._SupplyAmmount;
                            break;
                    }

                    _ResourceOfInterest._SupplyAmmount -= _ResourceOfInterest._SupplyAmmount;
                }
            }
            if (_ResourceOfInterest != null && _ResourceOfInterest._SupplyAmmount <= 0)
            {
                _FindObject.DeleteWorldResource(_ResourceOfInterest);
                Destroy(_ResourceOfInterest.gameObject);
            }
            _CollectingResource = false;
            CancelTool();
            _FindObject.RefreshLists();

            yield return null;
        }
    }
    #endregion
    #region UseTool
    void UseTool()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().SetTrigger("Swing");
        }
       
    }
    void CancelTool()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().ResetTrigger("Swing");
        }
    }
    #endregion
    //Villager will find wood to cut
    #region Gather_Wood
    void Gather_Wood()
    {
        if (_ResourceOfInterest == null)
        {
            _ResourceOfInterest = _FindObject.ClosestResourceOfInterest(_FindObject._WoodSupplies, transform.position, gameObject);

        }
        else
        {
            CanIGatherResource();
        }
    
    }
    #endregion
    //Villager will find stone to mine
    #region Gather_Stone
    void Gather_Stone()
    {
        if (_ResourceOfInterest != null)
        {
            CanIGatherResource();
      
        }
        else
        {
            _ResourceOfInterest = _FindObject.ClosestResourceOfInterest(_FindObject._StoneSupplies, transform.position, gameObject);
        }
        
    }
    #endregion
    //Villager will find animals to hunt
    #region Hunt_Food
    void Hunt_Food()
    {
        
    }
    #endregion
    //Villager will find farm to gather food
    #region Farm
    void Farm()
    {

    }
    #endregion
    #region ReturnGoods
    void ReturnGoods()
    {
        //Pauses the villager from collecting anymore
        StopCoroutine(CollectResource(_ResourceOfInterest));

        Building _ResourceCollection = _FindObject.ClosestBuildingOfInterest(_FindObject._ResourceCollection, transform.position);
        if (_ResourceCollection != null)
        {

            _Nav.destination = _ResourceCollection.transform.position;


            //DropOff the goods
            if (Vector3.Distance(transform.position, _ResourceCollection.transform.position) < 2)
            {
                _WoodHeld = 0;
                _StoneHeld = 0;
                //Return to previous task
                _Task = _PreviousTask;
                _ReturningGoods = false;
            }
        }
    }
    #endregion
    //Villager will find their assigned bed and sleep
    #region Sleep
    void Sleep()
    {

    }
    #endregion

   
}
