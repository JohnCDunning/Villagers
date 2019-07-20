using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldResource : MonoBehaviour, ISelectable,ITakeDamage
{
    public ResourceType _ResourceType;

    [Space]
    public int _SupplyAmount;
    private int _OriginalAmount;

    public bool _SupplyBeingTaken = false;
    public GameObject _VillagerTravelingToThis;

    public GameObject _AnimatedOutline;
    public GameObject _Outline;

    #region Interfaces
    public void TakeDamage(int damage)
    {
        _SupplyAmount -= damage;
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
    #endregion

    private void Start()
    {
        _OriginalAmount = _SupplyAmount;
    }
    private void Update()
    {
        if(_SupplyAmount <= 0)
        {
            FindObjectOfType<FindObjectOfInterest>().DeleteWorldResource(this);
            Destroy(gameObject);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (_SupplyBeingTaken == true)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}

