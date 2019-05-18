using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource _AudioSource;
    public AudioClip _AudioToPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggerSound()
    {
        _AudioSource.pitch = Random.Range(0.8f, 1.4f);
        _AudioSource.PlayOneShot(_AudioToPlay);
        
    }
}
