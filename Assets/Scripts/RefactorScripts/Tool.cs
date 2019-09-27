using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Villagers
{
    public enum ToolType
    {
        Axe,
        Pickaxe,
        Basket,
        Sword,
        Hammer
    }
    public class Tool : MonoBehaviour
    {
        public ToolType _ToolType;
    }
}
