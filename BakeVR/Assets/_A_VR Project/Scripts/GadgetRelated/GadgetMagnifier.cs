using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetMagnifier : MonoBehaviour
{
    [SerializeField] private GameObject lensCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        lensCamera.SetActive(true);
    }

    public void TurnOff()
    {
        lensCamera.SetActive(false);
    }
}