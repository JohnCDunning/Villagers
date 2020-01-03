using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            if (VillagersUtilities.PrefabHandler.LoadPrefab(ref prefab, _AnimalType) == STATUS_CODE.IS_GOOD && prefab != null)
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
        _AnimatedOutline.GetComponent<Animator>().SetTrigger("Interact");
    }

    public void InteractWithObject(ISelectable selectableObject)
    {
    }

    public void InteractWithLocation(Vector3 location)
    {
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
