using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [Header("Left Hand TP Controller")]
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider teleportationProvider;

    [SerializeField] private InteractionLayerMask teleportationLayers;

    [SerializeField] private InputActionProperty teleportModeActivate;
    [SerializeField] private InputActionProperty teleportModeCancel;
    [SerializeField] private InputActionProperty thumbStick;
    [SerializeField] private InputActionProperty gripModeActivate;

    private bool _isActive;
    private InteractionLayerMask initialInteractionLayers;
    private List<IXRInteractable> interactables = new List<IXRInteractable>();

    void Start()
    {
        teleportModeActivate.action.Enable();
        teleportModeCancel.action.Enable();
        thumbStick.action.Enable();
        gripModeActivate.action.Enable();

        teleportModeActivate.action.performed += OnTeleportActivate;
        teleportModeCancel.action.performed += OnTeleportCancel;

        initialInteractionLayers = rayInteractor.interactionLayers;
    }

    void Update()
    {
        //Filtering
        if (!_isActive)
            return;
        if (thumbStick.action.triggered)
            return;

        rayInteractor.GetValidTargets(interactables);

        if (interactables.Count == 0)
        {
            TurnOffTeleportation();
            return;
        }

        //Time To Teleport
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        TeleportRequest request = new TeleportRequest();
        if (interactables[0].interactionLayers == 2)
        {
            request.destinationPosition = hit.point;
        }
        else if (interactables[0].interactionLayers == 4)
        {
            request.destinationPosition = hit.transform.GetChild(0).transform.position;
            request.destinationRotation = hit.transform.GetChild(0).transform.rotation;
        }

        teleportationProvider.QueueTeleportRequest(request);
        TurnOffTeleportation();
    }

    private void OnTeleportActivate(InputAction.CallbackContext ctx)
    {
        if (gripModeActivate.action.phase != InputActionPhase.Performed)
        {
            _isActive = true;
            rayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
            rayInteractor.interactionLayers = teleportationLayers;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext ctx)
    {
        TurnOffTeleportation();
    }

    private void TurnOffTeleportation()
    {
        _isActive = false;
        rayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        rayInteractor.interactionLayers = initialInteractionLayers;
    }
}