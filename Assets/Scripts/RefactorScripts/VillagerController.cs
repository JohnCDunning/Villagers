using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class VillagerController : MonoBehaviour, ISelectable, ITakeDamage
{
    public VillagerTask _Task;
    public ResourceType _WantedResource;
    [Header("Components")]
    public Manager _Manager;
    public NavMeshAgent _Nav;
    public Animator _Anim;

    [Header("Resources")]
    public int _Health = 100;
    public int _CarryCapacity;
    public int _Wood;
    public int _Stone;
    public int _Food;
    [Header("Audio")]
    public AudioSource _Audio;
    public AudioClip _WoodHit;
    public AudioClip _StoneHit;
    [Header("Misc")]
    public GameObject _AnimatedOutline;
    public GameObject _Outline;
    public Transform _ToolSpawn;
    public Transform _ResourceSpawn;
    public GameObject[] _AllTools;
    public GameObject[] _SingleResource;
    private GameObject _CurrentTool;
    private WorldResource _ObjectOfInterest;

    [Header("Popups")]
    public GameObject _PopupCanvas;
    public GameObject _WoodPopup;
    public GameObject _StonePopup;
    public GameObject _FoodPopup;
    bool _ShowingPopup;

    private ToolType _ToolType;
    public Dictionary<ToolType, GameObject> _Tool = new Dictionary<ToolType, GameObject>();
    public Dictionary<ResourceType, GameObject> _ResourceToCarry = new Dictionary<ResourceType, GameObject>();

    private bool CanUseTool = true;

    #region Interfaces
    public void TakeDamage(int damage)
    {
        _Health -= damage;
    }
    public void Select()
    {
        _Outline.SetActive(true);
    }
    public void UnSelect()
    {
        _Outline.SetActive(false);
    }
    public void InteractSelect()
    {
        _AnimatedOutline.GetComponent<Animator>().SetTrigger("ShowOutline");
    }
    public void InteractWithObject(ISelectable selectableObject)
    {
        GameObject ObjectToInteractWith = selectableObject.GetThisObject();
        if (ObjectToInteractWith != null)
        {
            if (ObjectToInteractWith.GetComponent<WorldResource>() != null)
            {
                _WantedResource = ObjectToInteractWith.GetComponent<WorldResource>()._ResourceType;
                FindClosestResource();

                _Task = VillagerTask.GatherResources;
            }
        }

        _Nav.destination = ObjectToInteractWith.transform.position;

        SetTask();
    }
    public void InteractWithLocation(Vector3 location)
    {
        _Nav.destination = location;
        _Task = VillagerTask.DoNothing;
    }
    public GameObject GetThisObject()
    {
        return gameObject;
    }
    #endregion

    private void Start()
    {
        //Add Tools to Dictionary
        _Tool.Add(ToolType.Axe, _AllTools[0]);
        _Tool.Add(ToolType.Pickaxe, _AllTools[1]);
        _Tool.Add(ToolType.Basket, _AllTools[2]);
        _ResourceToCarry.Add(ResourceType.wood, _SingleResource[0]);
        _ResourceToCarry.Add(ResourceType.stone, _SingleResource[1]);
        _ResourceToCarry.Add(ResourceType.food, _SingleResource[2]);
    }
    private void Update()
    {
        SetAnimation();
        if (!Alive())
        {
            KillVillager();
        }
        else
        {
            RunTasks();
        }

        if (_Wood >= _CarryCapacity || _Stone >= _CarryCapacity || _Food >= _CarryCapacity)
        {
            _Task = VillagerTask.ReturnGoods;
        }
    }
    float CheckVelocity()
    {
        return _Nav.velocity.magnitude;
    }

    void SetAnimation()
    {
        if (CheckVelocity() > .3f)
        {
            _Anim.SetBool("walking", true);
        }
        else
        {
            _Anim.SetBool("walking", false);
        }
    }

    #region Animation and spawning of tool
    void SpawnTool(ToolType tool)
    {
        GameObject NewTool = null;
        if (tool != ToolType.Basket)
        {
            NewTool = Instantiate(_Tool[tool], _ToolSpawn);
            NewTool.transform.localPosition = Vector3.zero; NewTool.transform.localRotation = Quaternion.identity;
        }
        else
        {
            NewTool = Instantiate(_Tool[tool], _ResourceSpawn);
        }
        _CurrentTool = NewTool;
    }
    void ToolCheck(ToolType tool)
    {
        if (_CurrentTool == null)
        {
            SpawnTool(tool);
        }
        else
        {
            if (_CurrentTool.GetComponent<Tool>()._ToolType != tool)
            {
                Destroy(_CurrentTool);
                SpawnTool(tool);
            }
            else
            {
                return;
            }
        }
    }
    void UseTool() //Handles animations of tools
    {
        if (CheckVelocity() < 1)
        {
            if (CanUseTool == true)
            {
                switch (_ObjectOfInterest._ResourceType)
                {
                    case ResourceType.wood:
                        ToolCheck(ToolType.Axe);
                        GetComponent<Animator>().SetTrigger("UseAxe");
                        break;
                    case ResourceType.stone:
                        ToolCheck(ToolType.Pickaxe);
                        GetComponent<Animator>().SetTrigger("UseAxe");
                        break;
                    case ResourceType.food:
                        ToolCheck(ToolType.Basket);
                        GetComponent<Animator>().SetTrigger("UseBasket");
                        break;
                }
            }
        }
    }
    void ResetAllTriggers()
    {
        _Anim.ResetTrigger("UseAxe");
        _Anim.ResetTrigger("UseBasket");
    }
    #endregion

    bool Alive()
    {
        if (_Health > 0)
            return true;

        return false;
    }

    void KillVillager()
    {
        Rigidbody[] rbs;
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }

        _Nav.enabled = false;
        _Anim.enabled = false;
    }

    void RunTasks()
    {
        switch (_Task)
        {
            case VillagerTask.GatherResources:
                if (_ObjectOfInterest != null)
                {
                    GatherResource();
                }
                else
                {
                    FindClosestResource();
                }
                break;
            case VillagerTask.ReturnGoods:
                ReturnGoods();
                break;
            case VillagerTask.DoNothing:
                //literally do nothing ayy
                break;
        }
    }
    void DisplayResource()
    {
        if (_Wood > _Stone && _Wood > _Food)
        {
            _CurrentTool = Instantiate(_ResourceToCarry[ResourceType.wood], _ResourceSpawn);
            
        }
        if(_Stone > _Wood && _Stone > _Food)
        {
            _CurrentTool = Instantiate(_ResourceToCarry[ResourceType.stone], _ResourceSpawn);
            
        }
        if (_Food > _Wood && _Food > _Stone)
        {
            _CurrentTool = Instantiate(_ResourceToCarry[ResourceType.food], _ResourceSpawn);
            
        }
        if (_CurrentTool != null)
        {
            _CurrentTool.transform.localPosition = Vector3.zero;
        }
    }
    void ReturnGoods()
    {
        ResetAllTriggers();
        Transform _ResourceCollection = _Manager._FindObject.ClosestBuildingOfInterest(_Manager._FindObject._ResourceCollection, transform.position).transform;
        _Nav.SetDestination(_ResourceCollection.position);
        _Anim.SetLayerWeight(1, 1);

        if(_CurrentTool != null)
        {
            Destroy(_CurrentTool);
        }
        DisplayResource();
        
        if (Vector3.Distance(transform.position, _ResourceCollection.position) < 2)
        {
            if (!_ShowingPopup)
            {
                StartCoroutine(ShowPopup());
            }
            Destroy(_CurrentTool);
            _Task = VillagerTask.GatherResources;
            _Anim.SetLayerWeight(1, 0);
        }
    }
    IEnumerator ShowPopup()
    {
        _ShowingPopup = true;
        if (_Wood > 0)
        {
            yield return new WaitForSeconds(0.5f);
            _Manager._CollectedResources._CollectedWood += _Wood;
            Instantiate(_WoodPopup, _PopupCanvas.transform);
        }
        
        if (_Stone > 0)
        {
            yield return new WaitForSeconds(0.5f);
            _Manager._CollectedResources._CollectedStone += _Stone;
            Instantiate(_StonePopup, _PopupCanvas.transform);
        }
        
        if (_Food > 0)
        {
            yield return new WaitForSeconds(0.5f);
            _Manager._CollectedResources._CollectedStone += _Food;
            Instantiate(_FoodPopup, _PopupCanvas.transform);
        }
        _Wood = 0;
        _Stone = 0;
        _Food = 0;
        _ShowingPopup = false;
    }
    void FindClosestResource()
    {
        if(_ObjectOfInterest != null)
        {
            _ObjectOfInterest._SupplyBeingTaken = false;
        }
        switch (_WantedResource)
        {
            case ResourceType.wood:
                if(_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._WoodSupplies, transform.position, gameObject) != null)
                {
                    _ObjectOfInterest = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._WoodSupplies, transform.position, gameObject);
                    _ObjectOfInterest._SupplyBeingTaken = true;
                }
                break;
            case ResourceType.stone:
                if(_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._StoneSupplies, transform.position, gameObject))
                {
                    _ObjectOfInterest = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._StoneSupplies, transform.position, gameObject);
                    _ObjectOfInterest._SupplyBeingTaken = true;
                }
                break;
            case ResourceType.food:
                if (_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._FoodSupplies, transform.position, gameObject))
                {
                    _ObjectOfInterest = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._FoodSupplies, transform.position, gameObject);
                    _ObjectOfInterest._SupplyBeingTaken = true;
                }
                break;
        }
        
    }
    void GatherResource()
    {
        if (_ObjectOfInterest != null)
        {
            if (Vector3.Distance(transform.position, _ObjectOfInterest.transform.position) < 2)
            {
                Quaternion rotation = Quaternion.LookRotation(_ObjectOfInterest.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

                UseTool();
            }
            else
            {
                InteractWithObject(_ObjectOfInterest.GetComponent<ISelectable>());
            }
        }
    }
    void SetTask()
    {
        if (_ObjectOfInterest != null)
        {
            if (_ObjectOfInterest.GetComponent<WorldResource>())
            {
                _Task = VillagerTask.GatherResources;
            }
        }
    }

    public void OnGatherResource()
    {
        if (_ObjectOfInterest != null)
        {
            _ObjectOfInterest.GetComponent<ITakeDamage>().TakeDamage(5);
            switch (_WantedResource)
            {
                case ResourceType.wood:
                    _Wood += 5;
                    _Audio.pitch = Random.Range(0.8f, 1.2f);
                    PlaySound(_WoodHit);
                    break;
                case ResourceType.stone:
                    _Stone += 5;
                    _Audio.pitch = Random.Range(0.8f, 1.2f);
                    PlaySound(_StoneHit);
                    break;
                case ResourceType.food:
                    _Food += 5;
                    _Audio.pitch = Random.Range(0.8f, 1.2f);
                    PlaySound(_WoodHit);
                    break;
            }
        }
    }
    public void CanUseToolCheck(int ableToUse)
    {
        switch (ableToUse)
        {
            case 0: //True
                CanUseTool = true;
                break;
            case 1: //False
                CanUseTool = false;
                break;
        }
    }
    public void PlaySound(AudioClip _AudioClip)
    {
        _Audio.PlayOneShot(_AudioClip);
    }
}
