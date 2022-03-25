using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
    //public TutorialManager tutorialManager;
    public GameObject oneShotAudioPrefab;
    public AudioClip hitClip;
    public ParticleSystem hitParticles;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] objects = Physics.OverlapSphere(this.transform.position, 1f);
        for (int i = 0; i < objects.Length; i++)
        {
            Rigidbody rb = objects[i].attachedRigidbody;
            if (rb != null)
                rb.AddForce((rb.position - this.transform.position) * 40f, ForceMode.Impulse);
        }
        hitParticles.Play();
        Instantiate(oneShotAudioPrefab, transform.position, Quaternion.identity).GetComponent<OneShotAudio>().InstantiateAudio(hitClip, true);
        this.gameObject.SetActive(false);
        Invoke("ResetTarget", 2f);
    }

    void ResetTarget()
    {
        this.gameObject.SetActive(true);
    }
}
