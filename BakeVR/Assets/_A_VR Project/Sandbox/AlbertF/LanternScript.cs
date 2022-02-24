using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{

    GameObject objectCollided;
    void Start()
    {

       
    }

    void Update()
    {
        

        RaycastHit hit;
        // Si el objeto choca con algo
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            //Aquí hay que hacer el sendMessage para hacer visible el objeto
            if (hit.collider.tag=="invObject")
            {
               objectCollided = hit.collider.gameObject;

                objectCollided.SendMessage("getVisible");


            }

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
