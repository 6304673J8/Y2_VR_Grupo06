using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;

    [Tooltip("If nonInteractable stops hands movement")]
    public Renderer nonPhysicalHand;
    [Tooltip("Distance required for nonPhysicalHand to render")]
    public float showNonPhysicalHandDistance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Compare the distance of our hands and the physical hand
        float distance = Vector3.Distance(transform.position, target.position);

        // If Distance exceeds our threshold ...  
        if (distance > showNonPhysicalHandDistance)
        {
            nonPhysicalHand.enabled = true;
        }
        else {
            nonPhysicalHand.enabled = false;
        }
    }


    // Called every fixed frame-rate frame, 0.01 (100 calls per second)
    void FixedUpdate()
    {
        // Calculating that the Rigidbody properly follows our Target

        // Changes the velocity of the Object to go with the Target pace.
        rb.velocity = (target.position - transform.position) 
                                                        / Time.fixedDeltaTime;

        // Gets the difference in the rotation of the Object and our Target
        Quaternion rotationDifference = target.rotation * 
                                        Quaternion.Inverse(transform.rotation);

        // We get the angle in order to calculate the Rotation Difference
        rotationDifference.ToAngleAxis(out float angleInDegree, 
                                                    out Vector3 rotationAxis);
        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

        // Angular velocity is computed, the required data needs to be Radian
        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad 
                                                        / Time.fixedDeltaTime);
    }
}
