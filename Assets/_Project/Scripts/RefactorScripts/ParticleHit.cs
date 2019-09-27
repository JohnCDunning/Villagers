using UnityEngine;
using System.Linq;

public class ParticleHit : MonoBehaviour {
    
    public Vector3 _ParticleHitLoc;
    public Vector3 _ParticleRot;

    public string[] _Tags;

    private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[200];
    void OnParticleCollision(GameObject other)
    {
       
        int safeLength = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
        if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleCollisionEvent[safeLength];
        int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
        int i = 0;
        while (i < numCollisionEvents)
        {
            //Particle Position
            _ParticleHitLoc = collisionEvents[i].intersection;
            //Particle Rotation
            _ParticleRot = Quaternion.LookRotation(-collisionEvents[i].normal).eulerAngles;
            //Collision
            if (_Tags.Contains(other.tag) == true)
            {
                
                DecalPlace();
            }
            i++;
        }
    }
    public virtual void DecalPlace()
    {        
    }
}
