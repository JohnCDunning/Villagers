using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    public Transform _Target;
    public NavMeshAgent _Nav;
  
    // Update is called once per frame
    void Update()
    {
        _Nav.SetDestination(_Target.position);
    }
}
