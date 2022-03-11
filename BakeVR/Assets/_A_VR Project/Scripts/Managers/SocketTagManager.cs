using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTagManager : XRSocketInteractor
{
    //This should be Changed for a Random Code Generator String - Idea: 
    //GameObject in Scene with Script that generates that code
    public string targetTag = string.Empty;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable);
    }

    //Custom Functionalities 
    /*private bool MatchUsingTag(XRBaseInteractable interactable)
    {

    }*/
}
