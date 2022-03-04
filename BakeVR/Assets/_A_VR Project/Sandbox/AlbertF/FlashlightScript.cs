using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{

    public GameObject bulbLight;
    public GameObject illumCol;
    // Start is called before the first frame update
    void Start()
    {
        bulbLight.SetActive(false);
        illumCol.SetActive(false);
    }


    public void turnOn()
    {
        bulbLight.SetActive(true);
        illumCol.SetActive(true);
    }

    public void turnOff()
    {
        bulbLight.SetActive(false);
        illumCol.SetActive(false);
    }


}
