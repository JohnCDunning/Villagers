using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollectedResources : MonoBehaviour
{
    public static CollectedResources _Instance;
    [Header("Resource Values")]
    public int _CollectedWood;
    public int _CollectedStone;

    [Header("Resource Text Boxes")]
    public TextMeshProUGUI _CollectedWoodText;
    public TextMeshProUGUI _CollectedStoneText;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
    }

    private void Update()
    {
        SetCollectedUI(_CollectedWood, _CollectedWoodText);
        SetCollectedUI(_CollectedStone, _CollectedStoneText);
    }

    void SetCollectedUI(int _ResourceAmount, TextMeshProUGUI _ResourceText)
    {
        _ResourceText.text = _ResourceAmount.ToString();
    }
}
