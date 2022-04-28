using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionTag : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("PURIFICATE");
        if (other.tag == "Purificator")
        {
            //Destroy(this.gameObject);
            gameObject.SetActive(false);
        }
    }
}
