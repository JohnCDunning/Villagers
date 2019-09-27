using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTest : MonoBehaviour
{
    public int collisionCount = 0;
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
       
        Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude /2);
    }
    void OnTriggerEnter(Collider col)
    {
        collisionCount++;
    }

    void OnTriggerExit(Collider col)
    {
        collisionCount--;
    }

}
