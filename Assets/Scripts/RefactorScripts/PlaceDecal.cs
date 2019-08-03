using UnityEngine;
public class PlaceDecal : ParticleHit
{
    public ParticleSystem _PS;

    private void Awake()
    {
        _PS = GameObject.Find("BloodParticles").GetComponent<ParticleSystem>();
    }
    //Using Particle hit systems to place a drychemdecal.
    public override void DecalPlace()
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = _ParticleHitLoc;
        emitParams.rotation3D = _ParticleRot;

        _PS.Emit(emitParams,1);
        base.DecalPlace();
    }
}
    
  