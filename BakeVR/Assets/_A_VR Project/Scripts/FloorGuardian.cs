using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorGuardian : MonoBehaviour
{
    public UnityEvent OnTriggerFloorGuard;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FloorGuardian"))
        {
            //The object has touched the floor 
            Debug.Log("OJITOOOO");
            OnTriggerFloorGuard?.Invoke();
        }
    }
}
