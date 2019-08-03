using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    public Transform _Target;
    public NavMeshAgent _Nav;

    private void Start()
    {
        StartCoroutine(RandomMove());
    }
    IEnumerator RandomMove()
    {
        _Nav.destination = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
        yield return new WaitForSeconds(Random.Range(2,7));
        StartCoroutine(RandomMove());
    }
}
