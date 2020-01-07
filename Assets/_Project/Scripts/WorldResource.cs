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

    public Vector3 _PosWherehit;

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
    #endregion

    private void Start()
    {
        _OriginalAmount = _SupplyAmount;
        Invoke("LateStart",1f);
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
        
        if(_SupplyAmount <= 0)
        {
            FindObjectOfType<FindObjectOfInterest>().DeleteWorldResource(this);
            Destroy(gameObject);
        }

        if(_SupplyAmount == (_OriginalAmount / 2))
        {
            if(GetComponentInChildren<Rigidbody>() != null)
            {
                GetComponentInChildren<Rigidbody>().isKinematic = false;
            }
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

