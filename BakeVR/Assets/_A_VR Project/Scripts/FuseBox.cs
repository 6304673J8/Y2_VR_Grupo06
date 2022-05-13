using UnityEngine;

public class FuseBox : MonoBehaviour
{
    public ParticleSystem[] SparkleFuseVFX;
    public ParticleSystem[] SwitchedOnVFX;
    public ParticleSystem[] SwitchedOffVFX;
    [SerializeField] private string AudioName;

    bool b_FusePresent = false;

    public void Switched(int step)
    {
        if (!b_FusePresent)
            return;

        if (step == 0)
        {
            foreach (var s in SwitchedOffVFX)
            {
                s.Play();
            }
        }
        else
        {
            foreach (var s in SwitchedOnVFX)
            {
                s.Play();
            }
        }
    }

    public void FuseSocketed(bool socketed)
    {
        b_FusePresent = socketed;

        if (b_FusePresent)
        {
            FindObjectOfType<AudioManager>().Play(AudioName);
            foreach (var s in SparkleFuseVFX)
            {
                s.Play();
            }
        }
    }
}