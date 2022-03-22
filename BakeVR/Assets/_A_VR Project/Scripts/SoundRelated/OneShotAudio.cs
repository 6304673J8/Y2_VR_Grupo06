using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    public void InstantiateAudio(AudioClip clip, bool spatial)
    {
        //Basic Spatialization Effect
        audioSource.spatialBlend = spatial ? 1f : 0f;
        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
        Destroy(this.gameObject, clip.length + 0.01f);
    }
}