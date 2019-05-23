using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public AudioSource _AudioSource;
    public AudioClip _AudioToPlay;
    public ParticleSystem _ParticleSystem;
  
    public void TriggerSound()
    {
        _AudioSource.pitch = Random.Range(0.8f, 1.4f);
        _AudioSource.PlayOneShot(_AudioToPlay);
        
    }
    public void TriggerParticles()
    {
        _ParticleSystem.Emit(10);
    }
}
