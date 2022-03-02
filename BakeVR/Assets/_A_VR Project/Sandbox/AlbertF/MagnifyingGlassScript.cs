using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagnifyingGlassScript : MonoBehaviour
{


    public GameObject cam;
    public GameObject camdisplay;


    private void Start()
    {
        cam.SetActive(false);
        camdisplay.SetActive(false);
    }

    public void activateCam()
    {
        cam.SetActive(true);
        camdisplay.SetActive(true);
    }

    public void deactivateCam()
    {
        cam.SetActive(false);
        camdisplay.SetActive(false);
    }

}
