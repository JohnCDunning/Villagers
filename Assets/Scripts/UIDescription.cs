using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIDescription : MonoBehaviour
{
    public TextMeshProUGUI _Description;
    public TextMeshProUGUI _WoodCost;
    public TextMeshProUGUI _StoneCost;
    public TextMeshProUGUI _FoodCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Clamp(Input.mousePosition.y + 100, 50, 200);
        float x = Mathf.Clamp(Input.mousePosition.x, 500, 1100);
        //transform.position = new Vector2(x,y);
        
    }
}
