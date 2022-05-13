using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AlienHandAnimator : MonoBehaviour
{

    [SerializeField] private InputActionReference controllerActionGrip;
    [SerializeField] private InputActionReference controllerActionTrigger;
    [SerializeField] private InputActionReference controllerActionPrimary;

    private string animatorGripParam = "Grip";
    private string animatorTriggetParam = "Trigger";
    private string animatorThumbParam = "Thumb";

    private float animationTarget;

    private Animator handAnimator;


    private void OnEnable()
    {
        controllerActionGrip.action.performed += GripActionPerformed;
        controllerActionGrip.action.canceled += GripActionCancel;

        controllerActionTrigger.action.performed += TriggerActionPerformed;
        controllerActionTrigger.action.canceled += TriggerActionCanceled;

        controllerActionPrimary.action.performed += PrimaryActionPerformed;
        controllerActionPrimary.action.canceled += PrimaryActionCanceled;
    }

    private void OnDisable()
    {
        controllerActionGrip.action.performed -= GripActionPerformed;
        controllerActionGrip.action.canceled -= GripActionCancel;

        controllerActionTrigger.action.performed -= TriggerActionPerformed;
        controllerActionTrigger.action.canceled -= TriggerActionCanceled;

        controllerActionPrimary.action.performed -= PrimaryActionPerformed;
        controllerActionPrimary.action.canceled -= PrimaryActionCanceled;
    }

    void Start()
    {
        this.handAnimator = GetComponent<Animator>();
    }

    private void GripActionPerformed(InputAction.CallbackContext obj)
    {
        animationTarget = 1.0f;
        SetFingerAnimationValues(animatorGripParam, animationTarget);
    }

    private void GripActionCancel(InputAction.CallbackContext obj)
    {
        animationTarget = 0.0f;
        SetFingerAnimationValues(animatorGripParam, animationTarget);
    }

    private void TriggerActionPerformed(InputAction.CallbackContext obj)
    {
        animationTarget = 1.0f;
        SetFingerAnimationValues(animatorTriggetParam, animationTarget);
    }
    private void TriggerActionCanceled(InputAction.CallbackContext obj)
    {
        animationTarget = 0.0f;
        SetFingerAnimationValues(animatorTriggetParam, animationTarget);
    }
    private void PrimaryActionPerformed(InputAction.CallbackContext obj)
    {
        animationTarget = 1.0f;
        SetFingerAnimationValues(animatorThumbParam, animationTarget);
    }

    private void PrimaryActionCanceled(InputAction.CallbackContext obj)
    {
        animationTarget = 0.0f;
        SetFingerAnimationValues(animatorThumbParam, animationTarget);
    }
    private void SetFingerAnimationValues(string animatorParam, float gripTarget)
    {
        handAnimator.SetFloat(animatorParam, gripTarget);
    }


}
