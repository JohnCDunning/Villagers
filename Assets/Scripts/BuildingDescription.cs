using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuildingDescription : MonoBehaviour
{
    [Header("DisplayedVariables")]
    public BuildingObject _Description;

    [Header("Text Links")]
    public TextMeshProUGUI _DescriptionText;
    public TextMeshProUGUI _BuildingName;
    public TextMeshProUGUI _WoodCost;
    public TextMeshProUGUI _StoneCost;
    public TextMeshProUGUI _FoodCost;
    
    // Update is called once per frame
    void Update()
    {
        if(_Description != null)
        {
            _BuildingName.text = _Description._BuildingName;
            _DescriptionText.text = _Description._BuildingDescription;
            _WoodCost.text = _Description._WoodCost.ToString();
            _StoneCost.text = _Description._StoneCost.ToString();
            _FoodCost.text = _Description._FoodCost.ToString();
        }
    }
}
