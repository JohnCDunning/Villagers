using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    public CollectedResources _CollectedResources;
    public ConstructObject _ConstructObject;
    public HighlightManager _HighlightManager;
    public UpgradeManager _UpgradeManager;
    public RaycastInfo _RaycastManager;
    public FindObjectOfInterest _FindObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
}
