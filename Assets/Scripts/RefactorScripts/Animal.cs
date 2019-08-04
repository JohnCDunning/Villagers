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
