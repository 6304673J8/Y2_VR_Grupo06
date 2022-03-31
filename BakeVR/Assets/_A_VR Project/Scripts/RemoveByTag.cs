using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Removes all objects in a scene using a particular tag
public class RemoveByTag : MonoBehaviour
{
    public void RemoveObjects(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetObject in taggedObjects)
            Destroy(targetObject);
    }
}
