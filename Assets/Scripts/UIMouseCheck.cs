using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIMouseCheck : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public bool _MouseEntered = false;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _MouseEntered = true;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _MouseEntered = false;
    }

}
