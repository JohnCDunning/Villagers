using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Villagers;
public class VillagerController : MonoBehaviour, ISelectable, ITakeDamage
{
    public VillagerTask _Task;
    public ResourceType _WantedGoal;
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
    public AudioClip _Death;
    [Header("Misc")]
    public GameObject _AnimatedOutline;
    public GameObject _Outline;
    public Transform _ToolSpawn;
    public Transform _WeaponSpawn;
    public Transform _ResourceSpawn;
    public GameObject[] _AllTools;
    public GameObject[] _SingleResource;
    private GameObject _CurrentTool;
    [HideInInspector]public GameObject _ObjOfFocus;

    
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
    private bool Dead = false;
   
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
        _AnimatedOutline.GetComponent<Animator>().SetTrigger("Interact");
    }
    public void InteractWithObject(ISelectable selectableObject)
    {
        if (selectableObject != null)
        {
            if (selectableObject != this)
            {
                GameObject obj = selectableObject.GetObject();
                if (obj != null)
                {
                    //World resource check (Wood,Stone,Food)
                    if (obj.GetComponent<WorldResource>() != null)
                    {
                        _WantedGoal = obj.GetComponent<WorldResource>()._ResourceType;

                        if (_CurrentTool != null)
                        {
                            Destroy(_CurrentTool);
                        }

                        ResetAllTriggers();
                        FindClosestResource();


                        if(_WantedGoal != ResourceType.building)
                        {
                            _Task = VillagerTask.GatherResources;
                        }
                        else
                        {
                            _Task = VillagerTask.Building;
                            _ObjOfFocus = obj;
                        }
                        
                    }
                    //Move to the point if alive
                    if (Alive())
                    {
                        _Nav.destination = obj.transform.position;

                        SetTask();
                    }
                    //Villager Check & checks to see if they are not on the same team
                    if (obj.GetComponent<VillagerController>())
                    {
                        if (obj.GetComponent<TeamSide>()._team != Team.player)
                            Attack(obj);
                    }
                    //Attack Animal (Chickens)
                    if (obj.GetComponent<Animal>())
                    {
                        Attack(obj);
                    }

                }
            }
        }
    }
    void Attack(GameObject obj)
    {
        FreeWorldResource();
        _Task = VillagerTask.Combat;
        _WantedGoal = ResourceType.combat;
        _ObjOfFocus = obj;
    }

    public void InteractWithLocation(Vector3 location)
    {
        if(Alive())
            _Nav.destination = location;
        _Task = VillagerTask.DoNothing;
    }
    public GameObject GetObject()
    {
        return gameObject;
    }
    #endregion
    private void Awake()
    {
        _Manager = FindObjectOfType<Manager>();
    }
    private void Start()
    {
        //Add Tools to Dictionary
        _Tool.Add(ToolType.Axe, _AllTools[0]);
        _Tool.Add(ToolType.Pickaxe, _AllTools[1]);
        _Tool.Add(ToolType.Basket, _AllTools[2]);
        _Tool.Add(ToolType.Sword, _AllTools[3]);
        _Tool.Add(ToolType.Hammer, _AllTools[4]);
        _ResourceToCarry.Add(ResourceType.wood, _SingleResource[0]);
        _ResourceToCarry.Add(ResourceType.stone, _SingleResource[1]);
        _ResourceToCarry.Add(ResourceType.food, _SingleResource[2]);
        Invoke("LateStart", 1);
    }
    private void LateStart()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            transform.position = hit.point;
        }
    }
    private void Update()
    {
        if(_Task == VillagerTask.Combat)
        {
            if (_ObjOfFocus != null)
            {
                
                if (_ObjOfFocus.GetComponent<VillagerController>())
                {
                    if (_ObjOfFocus.GetComponent<VillagerController>().enabled == false)
                    {
                        _ObjOfFocus = null;
                    }
                }
                if (_ObjOfFocus != null)
                {
                    if (_ObjOfFocus.GetComponent<Animal>() != null)
                    {
                        if (_ObjOfFocus.GetComponent<Animal>().enabled == false)
                        {
                            _ObjOfFocus = null;
                        }
                    }
                }
            }
            else
            {
                if (_Manager._FindObject.ClosestEnemyVillager(_Manager._FindObject._Villagers, gameObject) != null)
                {
                    _ObjOfFocus = _Manager._FindObject.ClosestEnemyVillager(_Manager._FindObject._Villagers, gameObject).gameObject;
                }
            }
        }
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
        //If the villager is moving, the walk animation will start
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
            if(tool != ToolType.Sword && tool != ToolType.Hammer) //Tools that need to spawn in right hand
            {
                NewTool = Instantiate(_Tool[tool], _ToolSpawn);
                NewTool.transform.localPosition = Vector3.zero; NewTool.transform.localRotation = Quaternion.identity;
            }
            
            else
            {
                NewTool = Instantiate(_Tool[tool], _WeaponSpawn);
                NewTool.transform.localPosition = Vector3.zero; NewTool.transform.localRotation = Quaternion.identity;
            }
            
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
            if (_CurrentTool.GetComponent<Tool>() != null)
            {
                if (_CurrentTool.GetComponent<Tool>()._ToolType == tool)
                {
                    return;
                }
            }
            else
            {
                Destroy(_CurrentTool);
                SpawnTool(tool);
            }
        }
    }
    void UseTool() //Handles animations of tools
    {
        if (CheckVelocity() < 1)
        {
            if (CanUseTool == true)
            {
                if (_Task == VillagerTask.Combat)
                {
                    if(_ObjOfFocus.GetComponent<ITakeDamage>() != null)
                    {
                        ToolCheck(ToolType.Sword);
                        GetComponent<Animator>().SetTrigger("Attack");
                    }
                    return;
                }

                if (_ObjOfFocus.GetComponent<WorldResource>() != null)
                {
                    switch (_ObjOfFocus.GetComponent<WorldResource>()._ResourceType)
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
                        case ResourceType.building:
                            ToolCheck(ToolType.Hammer);
                            GetComponent<Animator>().SetTrigger("Hammer");
                            break;

                    }
                }
            }
        }
    }
    void ResetAllTriggers()
    {
        _Anim.ResetTrigger("UseAxe");
        _Anim.ResetTrigger("UseBasket");
        _Anim.ResetTrigger("Attack");
        _Anim.ResetTrigger("Hammer");
    }
    #endregion

    bool Alive()
    {
        if (_Health > 0)
            return true;

        return false;
    }
    void FreeWorldResource()
    {
        if (_ObjOfFocus != null)
        {
            if (_ObjOfFocus.GetComponent<WorldResource>() != null)
            {
                _ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = false;
            }
        }
    }
    void KillVillager()
    {
        if (Dead == false)
        {
            FreeWorldResource();
            _Audio.PlayOneShot(_Death);
            Dead = true;
            GetComponent<Collider>().enabled = false;
            _Outline.SetActive(false);
            _AnimatedOutline.SetActive(false);
            enabled = false;
        }
        Rigidbody[] rbs;
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }

        _Nav.enabled = false;
        _Anim.enabled = false;
        Destroy(this);
    }

    public void RunTasks()
    {
        switch (_Task)
        {
            case VillagerTask.GatherResources:
                if (_ObjOfFocus != null)
                {
                    GatherResource();
                }
                else
                {
                    FindClosestResource();
                }
                break;
            case VillagerTask.Building:
                if(_ObjOfFocus != null) { 
                    GatherResource();
                }
                break;
            case VillagerTask.ReturnGoods:
                ReturnGoods();
                break;
            case VillagerTask.Combat:
                FreeWorldResource();
                GatherResource();
                ToolCheck(ToolType.Sword);
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
        Transform _ResourceCollection = _Manager._FindObject.ClosestBuildingOfInterest(_Manager._FindObject._ResourceCollection,gameObject, transform.position).transform;
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
    void UpdateCollectedResources(ResourceType resource,int amount)
    {
        if(GetComponent<TeamSide>()._team == Team.player)
        {
            switch (resource)
            {
                case ResourceType.wood:
                    _Manager._CollectedResources._CollectedWood += amount;
                    break;
                case ResourceType.stone:
                    _Manager._CollectedResources._CollectedStone += amount;
                    break;
                case ResourceType.food:
                    _Manager._CollectedResources._CollectedFood += amount;
                    break;
            }
        }
        else
        {
            switch (resource)
            {
                case ResourceType.wood:
                    _Manager._EnemyTownController.wood += amount;
                    break;
                case ResourceType.stone:
                    _Manager._EnemyTownController.stone += amount;
                    break;
                case ResourceType.food:
                    _Manager._EnemyTownController.food += amount;
                    break;
            }
        }
    }
    IEnumerator ShowPopup()
    {
        _ShowingPopup = true;
        if (_Wood > 0)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateCollectedResources(ResourceType.wood, _Wood);
            GameObject popup = Instantiate(_WoodPopup, _PopupCanvas.transform);
            popup.GetComponentInChildren<TextMeshProUGUI>().text = _Wood.ToString();
        }
        
        if (_Stone > 0)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateCollectedResources(ResourceType.stone, _Stone);
            GameObject popup = Instantiate(_StonePopup, _PopupCanvas.transform);
            popup.GetComponentInChildren<TextMeshProUGUI>().text = _Stone.ToString();
        }
        
        if (_Food > 0)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateCollectedResources(ResourceType.food, _Food);
            GameObject popup = Instantiate(_FoodPopup, _PopupCanvas.transform);
            popup.GetComponentInChildren<TextMeshProUGUI>().text = _Food.ToString();
        }
        _Wood = 0;
        _Stone = 0;
        _Food = 0;
        _ShowingPopup = false;
    }
    void FindClosestResource()
    {
        if(_ObjOfFocus != null)
        {
            if (_ObjOfFocus.GetComponent<WorldResource>() != null)
            {
                _ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = false;
            }
        }
        switch (_WantedGoal)
        {
            case ResourceType.wood:
                if(_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._WoodSupplies, transform.position, gameObject) != null)
                {
                    _ObjOfFocus = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._WoodSupplies, transform.position, gameObject).gameObject;
                    _ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = true;
                }
                break;
            case ResourceType.stone:
                if(_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._StoneSupplies, transform.position, gameObject))
                {
                    _ObjOfFocus = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._StoneSupplies, transform.position, gameObject).gameObject;
                    _ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = true;
                }
                break;
            case ResourceType.food:
                if (_Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._FoodSupplies, transform.position, gameObject))
                {
                    _ObjOfFocus = _Manager._FindObject.ClosestResourceOfInterest(_Manager._FindObject._FoodSupplies, transform.position, gameObject).gameObject;
                    _ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = true;
                }
                break;
        }
    }
    void GatherResource()
    {
        if (_ObjOfFocus != null)
        {
            if(_WantedGoal == ResourceType.building)
            {
                if (Vector3.Distance(transform.position, _ObjOfFocus.transform.position) < 5)
                {
                    Quaternion rotation = Quaternion.LookRotation(_ObjOfFocus.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

                    UseTool();
                    return;
                }
            }
            if (Vector3.Distance(transform.position, _ObjOfFocus.transform.position) < 2)
            {
                Quaternion rotation = Quaternion.LookRotation(_ObjOfFocus.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);

                UseTool();
            }
            else
            {
                InteractWithObject(_ObjOfFocus.GetComponent<ISelectable>());
            }
        }
    }
    void SetTask()
    {
        if (_ObjOfFocus != null)
        {
            if (_ObjOfFocus.GetComponent<WorldResource>())
            {
                _Task = VillagerTask.GatherResources;
            }
        }
    }

    public void OnGatherResource()
    {
        if (_ObjOfFocus != null)
        {
            if(_Task == VillagerTask.Combat)
            {
                if (_ObjOfFocus.GetComponent<VillagerController>()) //Makes the other villager attack this villager back
                {
                    VillagerController Villager = _ObjOfFocus.GetComponent<VillagerController>();
                    if (Villager._ObjOfFocus != null)
                    {
                        if (Villager._ObjOfFocus.GetComponent<WorldResource>() != null)
                        {
                            Villager._ObjOfFocus.GetComponent<WorldResource>()._SupplyBeingTaken = false;
                        }
                    }
                    Villager._Wood = 0; Villager._Stone = 0; Villager._Food = 0;
                    Villager._Task = VillagerTask.Combat;
                    Villager._ObjOfFocus = this.gameObject;
                    Villager._WantedGoal = ResourceType.combat;
                }
                if (_ObjOfFocus.GetComponent<ITakeDamage>() != null && _ObjOfFocus.GetComponent<WorldResource>() == null)
                {
                    _ObjOfFocus.GetComponent<ITakeDamage>().TakeDamage(20); //Deal damage to other entity
                }
                else { Destroy(_CurrentTool);_Task = VillagerTask.GatherResources; }
            }
            else
            {
                if (_ObjOfFocus.GetComponent<ITakeDamage>() != null)
                {
                    _ObjOfFocus.GetComponent<ITakeDamage>().TakeDamage(1); //Deal damage to resources
                }
                
                
            }
            if (_CurrentTool != null)
            {
                if(_CurrentTool.GetComponentInChildren<ParticleSystem>() != null)
                    _CurrentTool.GetComponentInChildren<ParticleSystem>().Play();
            }
            if (_Task == VillagerTask.GatherResources) //Gather resources
            {
                switch (_WantedGoal)
                {
                    case ResourceType.wood:
                        _Wood += 1;
                        _Audio.pitch = Random.Range(0.8f, 1.2f);
                        PlaySound(_WoodHit);
                        break;
                    case ResourceType.stone:
                        _Stone += 1;
                        _Audio.pitch = Random.Range(0.8f, 1.2f);
                        PlaySound(_StoneHit);
                        break;
                    case ResourceType.food:
                        _Food += 1;
                        _Audio.pitch = Random.Range(0.8f, 1.2f);
                        PlaySound(_WoodHit);
                        break;
                }
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
