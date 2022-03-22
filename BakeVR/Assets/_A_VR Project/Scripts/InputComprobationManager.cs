using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputComprobationManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private InputActionProperty leftTriggerActivate;
    [SerializeField] private InputActionProperty rightTriggerActivate;
    [SerializeField] private InputActionProperty leftGripActivate;
    [SerializeField] private InputActionProperty rightGripActivate;
    [Space(10)]
    [Header("Left Buttons")]
    [SerializeField] private InputActionReference L_ControllerActionTrigger;
    [SerializeField] private InputActionReference L_ControllerActionGrip;
    public bool b_lTriggerIsActive;
    public bool b_lGripIsActive;
    [Space(5)]
    [Header("Right Buttons")]
    [SerializeField] private InputActionReference R_ControllerActionTrigger;
    [SerializeField] private InputActionReference R_ControllerActionGrip;
    public bool b_rTriggerIsActive;
    public bool b_rGripIsActive;

    private void Awake()
    {
        //Left
        L_ControllerActionTrigger.action.performed += LeftTriggerPress;
        L_ControllerActionGrip.action.performed += LeftGripPress;
        //Right
        R_ControllerActionTrigger.action.performed += RightTriggerPress;
        R_ControllerActionGrip.action.performed += RightGripPress;
    }

    private void LeftTriggerPress(InputAction.CallbackContext ctx)
    {
        if (leftTriggerActivate.action.phase == InputActionPhase.Performed)
        {
            b_lTriggerIsActive = true;
            Debug.Log("L Triggered");
        }
    }
    private void LeftGripPress(InputAction.CallbackContext ctx)
    {
        if (leftGripActivate.action.phase == InputActionPhase.Performed)
        {
            b_lGripIsActive = true;
            Debug.Log("L Gripped");
        }
    }
    private void RightTriggerPress(InputAction.CallbackContext ctx)
    {
        if (rightTriggerActivate.action.phase == InputActionPhase.Performed)
        {
            b_rTriggerIsActive = true;
            Debug.Log("R Triggered");
        }
    }
    private void RightGripPress(InputAction.CallbackContext ctx)
    {
        if (rightGripActivate.action.phase == InputActionPhase.Performed)
        {
            b_rGripIsActive = true;
            Debug.Log("R Gripped");
        }
    }

}