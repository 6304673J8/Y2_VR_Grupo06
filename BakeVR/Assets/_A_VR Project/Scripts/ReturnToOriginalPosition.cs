using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToOriginalPosition : MonoBehaviour
{
    [Tooltip("The Transform that the object will return to")]
    [SerializeField] Vector3 returnToPosition;
    public bool shouldReturnHome { get; set; } = false;
    public bool actionedSocketTrigger { get; set; } = false;
    public bool returnedSocketTrigger { get; set; } = false;

    void Awake()
    {
        returnToPosition = this.transform.position;
        shouldReturnHome = true;

        actionedSocketTrigger = false;
        returnedSocketTrigger = false;
    }

    public void ReturnHome()
    {
        if (shouldReturnHome)
            transform.position = returnToPosition;
    }
}
