using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearableObj : MonoBehaviour
{
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Flashlight"))
        {
            mr.enabled = true;
        }


        
    }
}
