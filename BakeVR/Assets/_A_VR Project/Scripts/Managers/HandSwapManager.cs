using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandSwapManager : MonoBehaviour
{
    [Header ("Hands Controller")]
    [SerializeField] private ActionBasedController leftHandController;
    [SerializeField] private ActionBasedController rightHandController;

    [Header("Hands Model")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    public void SwapHands()
    {
        if (leftHandController.modelParent.transform.childCount > 0)
        {
            Destroy(leftHandController.modelParent.GetChild(0).gameObject);
            Destroy(rightHandController.modelParent.GetChild(0).gameObject);

            Instantiate(leftHand, leftHandController.modelParent);
            Instantiate(rightHand, rightHandController.modelParent);
        }
        else
        {
            Instantiate(leftHand, leftHandController.modelParent);
            Instantiate(rightHand, rightHandController.modelParent);
        }
    }
}
