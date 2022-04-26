using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private string myAnimation = "FlewDoor";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.Play(myAnimation, 0, 0.0f);
        }
    }
}
