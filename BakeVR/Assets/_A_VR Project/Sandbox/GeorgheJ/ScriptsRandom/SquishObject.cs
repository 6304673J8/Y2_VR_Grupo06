using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishObject : MonoBehaviour
{
    public float squishForce = 1;

    public GameObject fx;


    public AudioClip squishSound;
    public float squishVolume = 1f;

    public void DelayedSmash(float delay)
    {
        Invoke("DoSmash", delay);
    }

    public void DoSmash()
    {
        //var grabbable = GetComponent<Grabbable>();
        var particles = Instantiate(fx, this.transform.position, this.transform.rotation).GetComponent<ParticleSystem>();

        //ParticleSystem.VelocityOverLifetimeModule module = fx.velocityOverLifetime;
        var rb = GetComponent<Rigidbody>();
       // module.x = rb.velocity.x;
        //module.y = rb.velocity.y;
       // module.z = rb.velocity.z;

        //grabbable.ForceHandsRelease();

        //Play the audio sound
        if (squishSound)
            AudioSource.PlayClipAtPoint(squishSound, transform.position, squishVolume);

        Destroy(gameObject);
    }
}
