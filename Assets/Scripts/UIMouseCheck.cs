using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIMouseCheck : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject UICost;
    public bool _MouseEntered = false;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _MouseEntered = true;
        UICost.transform.position = new Vector2(transform.position.x, 125);
        UICost.GetComponent<UIDescription>()._Description.text = GetComponent<UIBuilding>()._BuildingDetails._BuildingDescription.ToString();
        UICost.GetComponent<UIDescription>()._WoodCost.text = GetComponent<UIBuilding>()._BuildingDetails._WoodCost.ToString();
        UICost.GetComponent<UIDescription>()._StoneCost.text = GetComponent<UIBuilding>()._BuildingDetails._StoneCost.ToString();
        UICost.GetComponent<UIDescription>()._FoodCost.text = GetComponent<UIBuilding>()._BuildingDetails._FoodCost.ToString();
        UICost.SetActive(true);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _MouseEntered = false;
        UICost.SetActive(false);

    }

}
