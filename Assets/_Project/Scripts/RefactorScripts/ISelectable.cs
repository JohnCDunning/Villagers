using UnityEngine;
//For all objects that need to be selected
public interface ISelectable
{
    void Select();
    void UnSelect();
    void InteractSelect();
    void InteractWithObject(ISelectable selectableObject);
    void InteractWithLocation(Vector3 location);
    
    GameObject GetObject();
}