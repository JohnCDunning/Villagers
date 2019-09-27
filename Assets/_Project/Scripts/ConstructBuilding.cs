using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConstructBuilding : MonoBehaviour, ISelectable,ITakeDamage
{
    public GameObject _AnimatedOutline;
    public float builtStagePercentage = 0;
    public int stages = 1;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ConstructObject());
    }
    public void Select()
    {
    }

    public void UnSelect()
    {
    }

    public void InteractSelect()
    {

        _AnimatedOutline.GetComponent<Animator>().SetTrigger("ShowOutline");
    }

    public void InteractWithObject(ISelectable selectableObject)
    {
    }

    public void InteractWithLocation(Vector3 location)
    {
    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }

    public void TakeDamage(int damage)
    {
        builtStagePercentage += damage;
    }
    void Update()
    {
        if(builtStagePercentage >= 5)
        {
            stages++;
            builtStagePercentage = 0;

            transform.GetChild(stages).gameObject.SetActive(true);
            if(stages - 1 != 0)
                transform.GetChild(stages -1).gameObject.SetActive(false);


            if (stages >= transform.childCount -1)
            {
                Destroy(GetComponent<BoxCollider>());
               // Destroy(GetComponent<NavMeshObstacle>());
                Destroy(GetComponent<WorldResource>());
                Destroy(this);
            }
        }
    }
}
