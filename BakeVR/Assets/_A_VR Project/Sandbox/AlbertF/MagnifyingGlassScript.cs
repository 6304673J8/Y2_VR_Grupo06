using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagnifyingGlassScript : MonoBehaviour
{

    public GameObject cam;
    
    public void activateCam()
    {
        cam.SetActive(true);
    }

    public void deactivateCam()
    {
        cam.SetActive(false);
    }

}
