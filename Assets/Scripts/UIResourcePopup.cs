using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIResourcePopup : MonoBehaviour
{
    public GameObject _WoodPopup;
    public GameObject _StonePopup;
    
    public void ShowResourcePopup(ResourceType _ResourceType, int _ResourceAmount)
    {
        switch (_ResourceType)
        {
            case ResourceType.wood:
                GameObject WoodPopup = Instantiate(_WoodPopup, transform.position, Quaternion.identity, transform);
                WoodPopup.transform.position = transform.position;
                WoodPopup.GetComponentInChildren<TextMeshProUGUI>().text = "+" + _ResourceAmount.ToString();
                break;
            case ResourceType.stone:
                GameObject StonePopup = Instantiate(_StonePopup, transform.position, Quaternion.identity, transform);
                StonePopup.GetComponentInChildren<TextMeshProUGUI>().text = "+" + _ResourceAmount.ToString();
                break;
        }
    }
}
