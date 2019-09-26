using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalType
{
    NONE,
    CHICKEN
}
public enum STATUS_CODE
{
    IS_GOOD = 0,
    IS_BAD = 1,
    BAD_ANIMAL = 2
}
public class PrefabHandler
{
    static readonly string _ChickenResourcePrefab = @"Prefabs\ChickenCooked";
    public static STATUS_CODE LoadPrefab(ref GameObject a_prefab, AnimalType a_animalType)
    {
        STATUS_CODE status = STATUS_CODE.IS_GOOD;
        
        GameObject outObject = null;
        try
        {
            switch(a_animalType)
            {
                case AnimalType.CHICKEN:
                    {
                        outObject = Resources.Load(_ChickenResourcePrefab, typeof(GameObject)) as GameObject;
                        break;
                    }
                default:
                    {
                        outObject = null;
                        status = STATUS_CODE.BAD_ANIMAL;
                        break;
                    }
            }
        }
        catch (System.Exception exc)
        {
            Debug.LogError(exc.ToString());
            status = STATUS_CODE.IS_BAD;
        }
        a_prefab = outObject;
        return status;
    }

}
public class Animal : MonoBehaviour, ITakeDamage,ISelectable
{
    public Transform _Target;
    public NavMeshAgent _Nav;
    public int _Health = 50;
    public GameObject _Outline;
    public GameObject _AnimatedOutline;
    public AnimalType _AnimalType = AnimalType.NONE;
    private void Start()
    {
        StartCoroutine(RandomMove());
    }
    void Update()
    {
        if(_Health <= 0)
        {
            Destroy(_Nav);
            GetComponentInChildren<Rigidbody>().isKinematic = false;
            gameObject.AddComponent<WorldResource>();
            GetComponent<WorldResource>()._ResourceType = ResourceType.food;
            GetComponent<WorldResource>()._SupplyAmount = 60;
            GetComponent<WorldResource>()._Outline = _Outline; GetComponent<WorldResource>()._AnimatedOutline = _AnimatedOutline;
            GameObject prefab = null;
            foreach(var renderer in GetComponentsInChildren<Renderer>())
            {
                Destroy(renderer);
            }
            if (PrefabHandler.LoadPrefab(ref prefab, _AnimalType) == STATUS_CODE.IS_GOOD && prefab != null)
            {
                Instantiate(prefab, this.transform.position, this.transform.rotation,this.transform);
            }
            Destroy(GetComponent<Animator>());
            Destroy(this);
        }
    }
    IEnumerator RandomMove()
    {
        if (_Health <= 0)
            yield return null;
        if(_Nav != null)
            _Nav.destination = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
        yield return new WaitForSeconds(Random.Range(2,7));
        
        StartCoroutine(RandomMove());
    }

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
    }

    public void InteractWithLocation(Vector3 location)
    {
    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }
}
