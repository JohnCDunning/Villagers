using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FollowMousePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x,60,2000), Input.mousePosition.y);
    }
}
