using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Villager : MonoBehaviour
{
    [Header("Villager Task")]
    public VillagerTask _Task;
    [HideInInspector] public VillagerTask _PreviousTask;
    [HideInInspector] public VillagerAnimState _AnimState;

    public GameObject _CurrentToolHeld;
    [HideInInspector] public GameObject _CurrentResourceHeld;
    
    [Space]
    public NavMeshAgent _Nav;
    public Animator _VillagerAnimator;
    
    [Header("Resources")]
    public int _CarryCapacity;
    public int _CarryAmount;

    private bool _ReturningGoods = false;
    private WorldResource _ResourceOfInterest;

    private bool _CollectingResource = false;
    private FindObjectOfInterest _FindObject;
    [Space]
    public GameObject _Outline;

    [Header("Tools spawn")]
    public Transform _ToolSpawn;
    public Transform _ResourceSpawn;

    [Header("UIPopup")]
    public UIResourcePopup _ResourcePopup;
    
    public Dictionary<string, int> _Supplies = new Dictionary<string, int>();
    public Dictionary<string, GameObject> _HeldItem = new Dictionary<string, GameObject>();

    public GameObject[] _Tools;
    public GameObject[] _ResourcesHeld;

    void Awake()
    {
        _Supplies.Add("Wood", 0);
        _Supplies.Add("Stone", 0);
        _Supplies.Add("Food", 0);

        _HeldItem.Add("Axe", _Tools[0]);
        _HeldItem.Add("Pickaxe", _Tools[1]);
        _HeldItem.Add("Basket", _Tools[2]);

        _HeldItem.Add("Wood",_ResourcesHeld[0]);
        _HeldItem.Add("Stone", _ResourcesHeld[1]);
        _HeldItem.Add("Food", _ResourcesHeld[2]);

        _FindObject = FindObjectOfType<FindObjectOfInterest>();
    }
    void Update()
    {
        SetTask();
        SetAnimationState();
        
        _CarryAmount = (_Supplies["Wood"] + _Supplies["Stone"] + _Supplies["Food"]);
        if(_CarryAmount >= _CarryCapacity)
        {
            _Task = VillagerTask.ReturnGoods;
            ReturnGoods();
        }
        if (_Nav.velocity.magnitude > 0.1)
        {
            _AnimState = VillagerAnimState.walking;
        }
        else
        {
            _AnimState = VillagerAnimState.idle;
        }
    }
    #region Move To spawn point
    public void SetSpawnPoint(Vector3 _Spawn)
    {
        _Task = VillagerTask.MoveToPoint;
        _Nav.destination = _Spawn;
        if (_CurrentToolHeld != null)
            Destroy(_CurrentToolHeld.gameObject);

    }
    #endregion
    #region Set new task for villager
    public void NewTask(int _TaskNumber)
    {
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
    #region SetAnimationState
    void SetAnimationState()
    {
        switch (_AnimState)
        {
            case VillagerAnimState.idle:
                _VillagerAnimator.SetBool("idle", true);
                _VillagerAnimator.SetBool("walking", false);
                break;
            case VillagerAnimState.walking:
                _VillagerAnimator.SetBool("walking", true);
                _VillagerAnimator.SetBool("idle", false);
                break;
        }
    }
    #endregion
    #region ShouldIReturnGoods
    bool ShouldIReturnGoods()
    {
        if (_Supplies["Stone"] >= _CarryCapacity || _Supplies["Wood"] >= _CarryCapacity || _Supplies["Food"] >= _CarryCapacity)
        {
            if(_Task != VillagerTask.ReturnGoods)
            {
                _PreviousTask = _Task;
                _Task = VillagerTask.ReturnGoods;
                Destroy(_CurrentToolHeld);
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
    #region SetTask
    void SetTask()
    {
        switch (_Task)
        {
            case VillagerTask.Gather_Wood:
                Gather_Resource(_FindObject._WoodSupplies);
                break;
            case VillagerTask.Gather_Stone:
                Gather_Resource(_FindObject._StoneSupplies);
                break;
            case VillagerTask.Gather_Food:
                Gather_Resource(_FindObject._FoodSupplies);
                break;

            case VillagerTask.ReturnGoods:
                if (_CurrentResourceHeld == null)
                {
                    SpawnCorrectHeldResource();
                }
                ReturnGoods();
                break;
        }
    }

    /// <summary>
    /// REFACTOR
    /// </summary>
    void SpawnCorrectHeldResource()
    {
       
        List<int> resources = new List<int>();
        
        resources.Add(_Supplies["Wood"]);
        resources.Add(_Supplies["Stone"]);
        resources.Add(_Supplies["Food"]);
    
        int highestResource = 0;
        int resourceCount = 0;

        for(int i = 0;i<3; i++)
        {
            if (resources[i] >= highestResource)
            {
                highestResource = resources[i];
                resourceCount = i;
            }
        }
        switch (resourceCount)
        {
            case 0:
                SpawnHeldResource(0);
                break;
            case 1:
                SpawnHeldResource(1);
                break;
            case 2:
                SpawnHeldResource(2);
                break;
        }
    }

    void SpawnHeldResource(int resourceType)
    {
        GameObject resource = Instantiate(_ResourcesHeld[resourceType], _ResourceSpawn);
        resource.transform.localPosition = Vector3.zero;
        _CurrentResourceHeld = resource;
    }
    #endregion
    #region Can the villager gather the resource
    void CanIGatherResource()
    {
        if (_ResourceOfInterest != null)
        {
            _Nav.destination = _ResourceOfInterest.transform.position;
            
            //If the villager is within range. Start collecting
            if (Vector3.Distance(transform.position, _ResourceOfInterest.transform.position) < 2)
            {
                if (_CollectingResource == false)
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
    IEnumerator CollectResource(WorldResource _ResourceOfInterest) //Manages hitting resources;
    {
        _CollectingResource = true; //So the coroutine only starts once
        if (_CarryAmount < _CarryCapacity)
        {
            yield return new WaitForSeconds(1);

            UseTool(); //Starts hit animation

            if (_Task != VillagerTask.ReturnGoods)
            {
                if (_ResourceOfInterest._SupplyAmount > 0)
                {
                    AddResource(1); //Adds 1 resource of the mined type
                    _ResourceOfInterest._SupplyAmount -= 1;
                }
                else
                {
                    Destroy(_ResourceOfInterest.gameObject);
                    _FindObject.RefreshLists();
                }
            }
        }
        _CollectingResource = false;
        yield return null;
    }
    void AddResource(int amount)
    {
        switch (_ResourceOfInterest._ResourceType)
        {
            case ResourceType.wood:
                _Supplies["Wood"] += amount;
                break;
            case ResourceType.stone:
                _Supplies["Stone"] += amount;
                break;
            case ResourceType.food:
                _Supplies["Food"] += amount;
                break;
        }
    }
    #endregion
    void SpawnTool(string tool)
    {
        if (_CurrentToolHeld == null)
        {
            GameObject selectedTool = Instantiate(_HeldItem[tool], _ToolSpawn);
            selectedTool.transform.localPosition = Vector3.zero;
            selectedTool.transform.rotation = _ToolSpawn.rotation;
            _CurrentToolHeld = selectedTool;
        }
    }
    #region UseTool
    void UseTool()
    {
        switch (_Task)
        {
            case VillagerTask.Gather_Wood:
                SpawnTool("Axe");
                _VillagerAnimator.SetTrigger("UseAxe");
                break;
            case VillagerTask.Gather_Stone:
                SpawnTool("Pickaxe");
                _VillagerAnimator.SetTrigger("UsePickaxe");
                break;
            case VillagerTask.Gather_Food:
                SpawnTool("Basket");
                _VillagerAnimator.SetTrigger("UseBasket");
                break;
        }
        _VillagerAnimator.SetLayerWeight(1, 0);
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
    #region ReturnGoods
    void ReturnGoods()
    {
        if (_Task == VillagerTask.ReturnGoods)
        {
            if (_CurrentToolHeld != null)
                Destroy(_CurrentToolHeld);

            //Pauses the villager from collecting anymore
            StopCoroutine(CollectResource(_ResourceOfInterest));

            Building _ResourceCollection = _FindObject.ClosestBuildingOfInterest(_FindObject._ResourceCollection, transform.position);
            if (_ResourceCollection != null)
            {
                _VillagerAnimator.SetLayerWeight(1, 1);
                _Nav.destination = _ResourceCollection.transform.position;
                //DropOff the goods
                if (Vector3.Distance(transform.position, _ResourceCollection.transform.position) < 2)
                {
                    if (_Supplies["Wood"] != 0)
                    {
                        CollectedResources._Instance._CollectedWood += _Supplies["Wood"];
                        _ResourcePopup.ShowResourcePopup(ResourceType.wood, _Supplies["Wood"]);
                    }
                    if (_Supplies["Stone"] != 0)
                    {
                        CollectedResources._Instance._CollectedStone += _Supplies["Stone"];
                        _ResourcePopup.ShowResourcePopup(ResourceType.stone, _Supplies["Stone"]);
                    }
                    if (_Supplies["Food"] != 0)
                    {
                        CollectedResources._Instance._CollectedFood += _Supplies["Food"];
                        _ResourcePopup.ShowResourcePopup(ResourceType.food, _Supplies["Food"]);
                    }

                    _Supplies["Wood"] = 0;
                    _Supplies["Stone"] = 0;
                    _Supplies["Food"] = 0;

                    //Return to previous task
                    _Task = _PreviousTask;
                    _ReturningGoods = false;
                    Destroy(_CurrentResourceHeld);
                    _VillagerAnimator.SetLayerWeight(1, 0);
                }
            }
        }
    }
    #endregion
}
