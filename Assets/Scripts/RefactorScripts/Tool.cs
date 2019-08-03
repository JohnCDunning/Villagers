using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Axe,
    Pickaxe,
    Basket,
    Sword
}
public class Tool : MonoBehaviour
{
    public ToolType _ToolType;
}
