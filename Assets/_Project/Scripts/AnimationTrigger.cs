using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{

    private ParticleSystem _ParticleSystem;
    private AudioClip _AudioClip;

    public void SetReferences()
    {
        if (_ParticleSystem == null)
        {
            _ParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
        }
    }
   
    public void TriggerSound()
    {
        _ParticleSystem.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.4f);
        _ParticleSystem.GetComponent<AudioSource>().Play();
        
    }
    public void TriggerParticles()
    {
        if (_ParticleSystem != null)
        {
            _ParticleSystem.Emit(10);
        }
        
    }
}
