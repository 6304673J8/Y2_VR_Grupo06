using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{

    GameObject objectRenderer;
    // Start is called before the first frame update
    void Start()
    {

        objectRenderer.SetActive(false);
        
    }


    void getVisible()
    {
        objectRenderer.SetActive(true);
    }

    void stopVisibility()
    {
        objectRenderer.SetActive(false);
    }
}
