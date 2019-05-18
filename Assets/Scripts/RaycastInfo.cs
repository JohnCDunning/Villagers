using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInfo : MonoBehaviour
{
    
    public Vector3 LocationToBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            pos = hit.point;
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }

    }
    public GameObject ObjectRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            pos = hit.point;
            return hit.transform.gameObject;

        }
        else
        {
            return hit.transform.gameObject;
        }
    }
}
