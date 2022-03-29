using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    [Tooltip("The material that's switched to.")]
    public Material otherMaterial = null;

    private bool usingOther = false;
    private MeshRenderer meshRenderer = null;
    private Material originalMaterial = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    public void SetOtherMaterial()
    {
        usingOther = true;
        meshRenderer.material = otherMaterial;
    }

    public void SetOriginalMaterial()
    {
        usingOther = false;
        meshRenderer.material = originalMaterial;
    }

    public void ToggleMaterial()
    {
        usingOther = !usingOther;

        if (usingOther)
        {
            meshRenderer.material = otherMaterial;
        }
        else
        {
            meshRenderer.material = originalMaterial;
        }
    }
}