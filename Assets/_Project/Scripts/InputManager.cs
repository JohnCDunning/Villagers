using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float CamHeight;
    public float zoomSpeed = 100f;
    public float zoomTime = 0.1f;

    public float maxHeight = 0;
    public float minHeight = -25f;
    [SerializeField]
    private float targetHeight;

    // Update is called once per frame
    void Update()
    {
        //RotateCamLeft
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 100 *Time.deltaTime,0),Space.World);
        }

        #region Camera Movement

        targetHeight += (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed/10) * -1f;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * 10 * Time.deltaTime,Space.Self); ;
        transform.position = new Vector3(this.transform.position.x, Mathf.MoveTowards(this.transform.position.y, targetHeight,10f * Time.deltaTime)  , this.transform.position.z);

        float zpos = transform.GetChild(0).transform.localPosition.y;
        #endregion
        #region Locking Camera Zoom
        //Cant go below or further than set height when zooming in camera;
        //if (zpos > 18)
        //{
          //  if (CamHeight < 0)
         //   {
       //         transform.GetChild(0).transform.localPosition += new Vector3(0, Mathf.Clamp(CamHeight, -39f, 19f), 0);
       //     }
       // }
       // if (zpos < -10)
       // {
        //    if (CamHeight > 0)
        //    {
//
        //        transform.GetChild(0).transform.localPosition += new Vector3(0, Mathf.Clamp(CamHeight, -39f, 19f), 0);
        //    }
       // }
       // if (zpos < 18 && zpos > -10)
       // {
       //     transform.GetChild(0).transform.localPosition += new Vector3(0, Mathf.Clamp(CamHeight, -39f, 19f),0);
       // }
        #endregion
    }
    
    public bool LeftMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool LeftMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RightMouseDown()
    {
        if (Input.GetMouseButton(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool LeftMouse()
    {
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RightMouse()
    {
        if (Input.GetMouseButton(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
