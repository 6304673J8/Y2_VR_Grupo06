using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class SocketTarget : MonoBehaviour
{
    public string SocketType;
    public SelectEnterEvent SocketedEvent;
    public bool DisableSocketOnSocketed;

    void Awake()
    {
        var interactable = GetComponent<XRBaseInteractable>();

        interactable.selectEntered.AddListener(SelectedSwitch);
    }

    public void SelectedSwitch(SelectEnterEventArgs args)
    {
        var interactor = args.interactor;
        //var interactor = args.interactorObject;
        var socketInteractor = interactor as UniqueSocketInteractor;

        if (socketInteractor == null)
            return;

        if (SocketType != socketInteractor.AcceptedType)
            return;

        if (DisableSocketOnSocketed)
        {
            //TODO : delay loses PRESENCE
            StartCoroutine(DisableSocketDelayed(socketInteractor));
        }

        SocketedEvent.Invoke(args);
    }

    IEnumerator DisableSocketDelayed(UniqueSocketInteractor interactor)
    {
        yield return new WaitForSeconds(0.5f);
        interactor.socketActive = false;
    }
}
