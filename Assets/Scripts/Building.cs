using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour,ISelectable,ITakeDamage
{
    public BuildingType _BuildingType;

    public GameObject _AnimatedOutline;
    public GameObject _Outline;
    public BuildingDetails _BuildingDetails;

    private int _Health;

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
    #endregion
    
}
