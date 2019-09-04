using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MarketCost", menuName = "Market", order = 1)]
public class MarketCost : ScriptableObject
{
    [Header("Cost of goods")]
    [Header("X=Wood,Y=Stone,Z=Food")]
    [Space]
    public Vector3 _Villager;
    public Vector3 _House;
    public Vector3 _Campfire;
    public Vector3 _ResourceCache;
    public Vector3 _Farm;
    public Vector3 _Windmill;
    public Vector3 _Barracks;
    public Vector3 _TownCenter;
    

}