using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    float CamHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamHeight = Input.mouseScrollDelta.y * 100 * Time.deltaTime;

        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 10 * Time.deltaTime;


        float zpos = transform.GetChild(0).transform.localPosition.z;
      

       //Cant go below or further than set height when zooming in camera;
        if(zpos > 18)
        {
            if (CamHeight < 0)
            {
              
                transform.GetChild(0).transform.localPosition += new Vector3(0, 0, Mathf.Clamp(CamHeight, -39f, 19f));
            }
        }
        if (zpos < -30)
        {
            if (CamHeight > 0)
            {

                transform.GetChild(0).transform.localPosition += new Vector3(0, 0, Mathf.Clamp(CamHeight, -39f, 19f));
            }
        }
        if (zpos < 18 && zpos > -30f)
        {
            transform.GetChild(0).transform.localPosition += new Vector3(0, 0, Mathf.Clamp(CamHeight, -39f, 19f));
        }
    }
}
