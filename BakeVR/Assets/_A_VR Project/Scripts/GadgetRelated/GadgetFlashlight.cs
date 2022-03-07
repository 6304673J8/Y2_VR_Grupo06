using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetFlashlight : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameObject lightCollider;
    
    // Start is called before the first frame update
    private void Awake()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        spotLight.SetActive(true);
        lightCollider.SetActive(true);
    }

    public void TurnOff()
    {
        spotLight.SetActive(false);
        lightCollider.SetActive(false);
    }       
}