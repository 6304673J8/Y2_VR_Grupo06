using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class TeleportToggle : MonoBehaviour
{
    //Linking our Input Action Through a Serialize Field
    [SerializeField] private InputActionReference teleportToggleBttn;

    //Creating Unity Events To Be Triggered On Button Pressed
    public UnityEvent OnTeleportActivate;
    public UnityEvent OnTeleportCancel;

    #region Input Action Listeners
    private void OnEnable()
    {
        //Basic New Input System Workaround
        teleportToggleBttn.action.performed += ActivateTeleport;
        teleportToggleBttn.action.canceled += DeactivateTeleport;
    }

    private void OnDisable()
    {
        //Basic New Input System Workaround
        teleportToggleBttn.action.performed -= ActivateTeleport;
        teleportToggleBttn.action.canceled -= DeactivateTeleport;
    }
    #endregion

    //Event Triggered By The New Input System - If Proper TP point, Teleport
    private void ActivateTeleport(InputAction.CallbackContext obj) =>
        OnTeleportActivate.Invoke();

    private void DeactivateTeleport(InputAction.CallbackContext obj) => 
        Invoke("TurnOffTeleport", .1f);

    //Event Triggered By The New Input System - If Button Released Cancel
    private void TurnOffTeleport() =>
        OnTeleportCancel.Invoke();
}
