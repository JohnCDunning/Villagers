using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Building", menuName = "Building", order = 1)]
public class BuildingObject : ScriptableObject
{
    [Header("Building Settings")]
    [Space]
    public BuildingType _BuildingType;
    public GameObject _ConstructionPrefab;
    public GameObject _ObjectPrefab;
    public string _BuildingName;
    public string _BuildingDescription;
    public int _WoodCost;
    public int _StoneCost;
    public int _FoodCost;
    [Space]
    [Header("Building Construction Settings")]
    [Space]
    public float _ConstructionSize;

}