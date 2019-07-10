using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerController : MonoBehaviour
{
    public int _Health = 100;
    public NavMeshAgent _Nav;
    public Animator _Anim;
    
    bool Alive()
    {
        if(_Health > 0)
            return true;
        
        return false;
    }
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
    private void Update()
    {
        if (!Alive())
        {
            KillVillager();
        }
    }
}
