using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public float _TimeSinceLastPlant = 0;
    public float _TimeToGrow;
    public Vector2 _GrowthRandomness;
    public bool Planted = false;

    public GameObject _PlantToGrow;
    public GameObject _CurrentPlant;
    private void Start()
    {
        _TimeToGrow = _TimeToGrow + Random.Range(_GrowthRandomness.x, _GrowthRandomness.y);
    }
    private void Update()
    {
        if(_TimeSinceLastPlant >= _TimeToGrow)
        {
            NewPlant();
        }
        if(Planted == true)
        {
            if(_CurrentPlant == null)
            {
                _TimeSinceLastPlant = 0;
                Planted = false;
            }
        }
        else
        {
            _TimeSinceLastPlant += 1 * Time.deltaTime;
        }
    }
    void NewPlant()
    {
        if (_CurrentPlant == null)
        {
            _CurrentPlant = Instantiate(_PlantToGrow, transform.position, Quaternion.identity, transform);
            _CurrentPlant.transform.localScale = Vector3.zero;
            StartCoroutine(GrowPlant());
            _TimeSinceLastPlant = 0;
            Planted = true;
        }
    }
    IEnumerator GrowPlant()
    {
        for(float i = 0;i<1;i+= 1 * Time.deltaTime)
        {
            _CurrentPlant.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
