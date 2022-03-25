using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveElementsWithTag : MonoBehaviour
{
    public void RemoveElements(string tag)
    {
        GameObject[] taggedElements = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject targetElement in taggedElements)
            Destroy(targetElement);
    }
}
