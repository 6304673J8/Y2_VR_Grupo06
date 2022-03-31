using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UniqueSocketInteractor : XRSocketInteractor
{
    public string AcceptedType;

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        SocketTarget socketTarget = interactable.GetComponent<SocketTarget>();

        if (socketTarget == null)
            return false;

        return base.CanSelect(interactable) && (socketTarget.SocketType == AcceptedType);
    }

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return CanSelect(interactable);
    }
}
