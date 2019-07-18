using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerController : MonoBehaviour, ISelectable, ITakeDamage
{
    public VillagerTask _Task;
    private ResourceType _WantedResource;
    public FindObjectOfInterest _FindObject;
    public NavMeshAgent _Nav;
    public Animator _Anim;

    public int _Health = 100;
    public int _CarryCapacity;

    public int _Wood;

    public GameObject _AnimatedOutline;
    public GameObject _Outline;

    public GameObject _ObjectOfInterest;
    
    public Transform _ToolSpawn;
    public GameObject _CurrentTool;

    public GameObject[] _AllTools;
    public ToolType _ToolType;
    public Dictionary<ToolType, GameObject> _Tool = new Dictionary<ToolType, GameObject>();

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
        _ObjectOfInterest = ObjectToInteractWith;
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
        Debug.Log(_Tool[ToolType.Axe]);
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

        if(_Wood > _CarryCapacity)
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
        GameObject _NewTool = Instantiate(_Tool[tool], _ToolSpawn);
        _NewTool.transform.localPosition = Vector3.zero; _NewTool.transform.localRotation = Quaternion.identity;
        _CurrentTool = _NewTool;
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
                switch (_ObjectOfInterest.GetComponent<WorldResource>()._ResourceType)
                {
                    case ResourceType.wood:
                        ToolCheck(ToolType.Axe);
                        GetComponent<Animator>().SetTrigger("UseAxe");
                        break;
                }
            }
        }
    }
    void ResetAllTriggers()
    {
        _Anim.ResetTrigger("UseAxe");
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
                break;
            case VillagerTask.ReturnGoods:
                ReturnGoods();
                break;
            case VillagerTask.DoNothing:
                //literally do nothing ayy
                break;
        }
    }
    void ReturnGoods()
    {
        ResetAllTriggers();
        Transform _ResourceCollection = _FindObject.ClosestBuildingOfInterest(_FindObject._ResourceCollection, transform.position).transform;
        _Nav.SetDestination(_ResourceCollection.position);

        if(Vector3.Distance(transform.position,_ResourceCollection.position) < 2)
        {
            //Add resources to global collection
            _Wood = 0;
            _Task = VillagerTask.GatherResources;
        }
        
    }
    void FindClosestResource()
    {
        switch (_WantedResource)
        {
            case ResourceType.wood:
                _ObjectOfInterest = _FindObject.ClosestResourceOfInterest(_FindObject._WoodSupplies, transform.position, gameObject).gameObject;
                break;
        }
    }
    void GatherResource()
    {
        if (Vector3.Distance(transform.position, _ObjectOfInterest.transform.position) < 2)
        {
            UseTool();
            
            _WantedResource = _ObjectOfInterest.GetComponent<WorldResource>()._ResourceType;
        }
        else
        {
            InteractWithObject(_ObjectOfInterest.GetComponent<ISelectable>());
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
            else
            {
                FindClosestResource();
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
}
