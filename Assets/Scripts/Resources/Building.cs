using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Building", menuName = "Building", order = 1)]
public class BuildingObject : ScriptableObject
{
    public BuildingType _BuildingType;
    public GameObject _ConstructionPrefab;
    public GameObject _ObjectPrefab;
    public string _BuildingName;
    public int _WoodCost;
    public int _StoneCost;
    public int _FoodCost;

   
}