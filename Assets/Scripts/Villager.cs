using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Villager : MonoBehaviour
{
    [Header("Villager Task")]
    public VillagerTask _Task;
    public VillagerTask _PreviousTask;

    [Space]
    public NavMeshAgent _Nav;
    
    [Header("Resources")]
    public int _WoodHeld;
    public int _StoneHeld;
    public int _FoodHeld;
    [HideInInspector]
    public bool _ReturningGoods = false;

    [HideInInspector]
    public WorldResource _ResourceOfInterest;

    private bool Started = false;
    private bool _CollectingResource = false;
    private FindObjectOfInterest _FindObject;

    [Space]
    public GameObject _Outline;
    public UIResourcePopup _ResourcePopup;

    public GameObject[] _ObjectsInHand;

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
    #region Move To spawn point
    public void SetSpawnPoint(Vector3 _Spawn)
    {
        _Task = VillagerTask.MoveToPoint;
        Debug.Log("Yeeee");
        _Nav.destination = _Spawn;
    }
    #endregion
    #region Set new task for villager
    public void NewTask(int _TaskNumber)
    {
        CancelTool();
        if (_ResourceOfInterest != null)
        {
            _ReturningGoods = false;
            _ResourceOfInterest._SupplyBeingTaken = false;
            _ResourceOfInterest._VillagerTravelingToThis = null;
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
                _Task = VillagerTask.Gather_Food;
                break;
        }
    }
    #endregion

    void Update()
    {
        //Run SetTask
        if (Started)
        {
            SetTask();
        }
        //See if the villager should return goods
        if (ShouldIReturnGoods() == true)
        {
        }

    }
    #region ShouldIReturnGoods
    bool ShouldIReturnGoods()
    {
        if (_StoneHeld >= 20 || _WoodHeld >= 20 || _FoodHeld >= 20)
        {
            if(_Task != VillagerTask.ReturnGoods)
            {
                _PreviousTask = _Task;
                _Task = VillagerTask.ReturnGoods;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region IsObjectWithinHittingDistance
    bool _ObjectOfInterestIsClose()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            LookAtObjectOfInterest();
            Debug.DrawLine(transform.position, hit.point, Color.red, 1);
            return true;
        }
        else
        {
            StartCoroutine(LookingAtInterest());
            return false;
        }
    }
    #endregion
    #region Disable everything but one thing
    void DisableEverythingBut(GameObject _ObjectToStay)
    {
        foreach(GameObject _object in _ObjectsInHand)
        {
            if (_object != _ObjectToStay)
            {
                _object.SetActive(false);
            }
            else
            {
                _object.SetActive(true);
            }
        }
    }
    #endregion
    #region SetTask
    void SetTask()
    {
        switch (_Task)
        {
            case VillagerTask.Gather_Wood:
                DisableEverythingBut(_ObjectsInHand[0]);
                Gather_Resource(_FindObject._WoodSupplies);
                break;
            case VillagerTask.Gather_Stone:
                DisableEverythingBut(_ObjectsInHand[1]);
                Gather_Resource(_FindObject._StoneSupplies);
                break;
            case VillagerTask.Gather_Food:
                DisableEverythingBut(_ObjectsInHand[2]);
                Gather_Resource(_FindObject._FoodSupplies);
                break;
                
            case VillagerTask.ReturnGoods:
                if (_WoodHeld > _StoneHeld && _WoodHeld > _FoodHeld)
                {
                    DisableEverythingBut(_ObjectsInHand[3]);
                }
                if (_StoneHeld > _WoodHeld && _StoneHeld > _FoodHeld)
                {
                    DisableEverythingBut(_ObjectsInHand[4]);
                }
                if(_FoodHeld > _WoodHeld && _FoodHeld > _StoneHeld)
                {
                    DisableEverythingBut(_ObjectsInHand[5]);
                }
                ReturnGoods();
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
                StartCoroutine(LookingAtInterest());
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

        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 1f * Time.deltaTime);
     
        
    }
    IEnumerator LookingAtInterest()
    {
        for(float i = 0;i<1;i+= 1 * Time.deltaTime)
        {
            if (_ResourceOfInterest != null)
            {
                Quaternion lookOnLook = Quaternion.LookRotation(_ResourceOfInterest.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, 0.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
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
                if(_Task == VillagerTask.MoveToPoint)
                {
                    break;
                }
                if (_Task == VillagerTask.ReturnGoods)
                {
                    break;
                }
                if (ShouldIReturnGoods() == false)
                {
                    StartCoroutine(LookingAtInterest());
                    UseTool();
                }
                yield return new WaitForSeconds(0.6f);
                
              
                if ((_ResourceOfInterest._SupplyAmmount - 5) > 0)
                {
                    switch (_ResourceOfInterest._ResourceType)
                    {
                        case ResourceType.wood:
                            _WoodHeld += 5;
                            break;
                        case ResourceType.stone:
                            _StoneHeld += 5;
                            break;
                        case ResourceType.food:
                            _FoodHeld += 5;
                            break;
                    }
                    _ResourceOfInterest._SupplyAmmount -= 5;
                }
                else
                {
                    switch (_ResourceOfInterest._ResourceType)
                    {
                        case ResourceType.wood:
                            _WoodHeld += _ResourceOfInterest._SupplyAmmount;
                            break;
                        case ResourceType.stone:
                            _StoneHeld += _ResourceOfInterest._SupplyAmmount;
                            break;
                        case ResourceType.food:
                            _FoodHeld += _ResourceOfInterest._SupplyAmmount;
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
    #region Gather_Resources
    void Gather_Resource(List<WorldResource> _WorldResource)
    {
        if (_ResourceOfInterest == null)
        {
            _ResourceOfInterest = _FindObject.ClosestResourceOfInterest(_WorldResource, transform.position, gameObject);

        }
        else
        {
            CanIGatherResource();
        }
    }
    #endregion
    #region Farm
    void Farm()
    {

    }
    #endregion
    #region ReturnGoods
    void ReturnGoods()
    {
        if (_Task == VillagerTask.ReturnGoods)
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

                    if (_WoodHeld != 0)
                    {
                        CollectedResources._Instance._CollectedWood += _WoodHeld;
                        _ResourcePopup.ShowResourcePopup(ResourceType.wood, _WoodHeld);
                    }
                    if (_StoneHeld != 0)
                    {
                        CollectedResources._Instance._CollectedStone += _StoneHeld;
                        _ResourcePopup.ShowResourcePopup(ResourceType.stone, _StoneHeld);
                    }
                    if (_FoodHeld != 0)
                    {
                        CollectedResources._Instance._CollectedFood += _FoodHeld;
                        _ResourcePopup.ShowResourcePopup(ResourceType.food, _FoodHeld);
                    }

                    _WoodHeld = 0;
                    _StoneHeld = 0;
                    _FoodHeld = 0;
                    //Return to previous task
                    _Task = _PreviousTask;
                    _ReturningGoods = false;
                }
            }
        }
    }
    #endregion
    #region Sleep
    void Sleep()
    {

    }
    #endregion

   
}
