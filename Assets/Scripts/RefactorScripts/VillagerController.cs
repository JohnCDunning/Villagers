using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerController : MonoBehaviour, ISelectable, ITakeDamage
{
    public GameObject _AnimatedOutline;
    public GameObject _Outline;
    public int _Health = 100;
    public NavMeshAgent _Nav;
    public Animator _Anim;

    public VillagerTask _Task;

    public int _Wood;
    public GameObject _ResourceOfInterest;

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

    bool Alive()
    {
        if(_Health > 0)
            return true;
        
        return false;
    }

    #region Villager Death
    void KillVillager()
    {
        Rigidbody[] rbs;
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }

        _Nav.enabled = false;
        _Anim.enabled = false;
    }
    #endregion
    private void Update()
    {
        if (!Alive())
        {
            KillVillager();
        }
    }
}
