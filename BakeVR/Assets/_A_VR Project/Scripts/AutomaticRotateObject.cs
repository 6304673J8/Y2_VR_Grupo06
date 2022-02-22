using UnityEngine;

public class AutomaticRotateObject : MonoBehaviour
{
    [Tooltip("The value at which the speed is applied")]
    [Range(0, 1)] public float sensitivity = 1.0f;

    [Tooltip("The max speed of the rotation")]
    public float speed = 10.0f;

    private bool isRotating = false;

    private void Awake()
    {
        isRotating = true;
    }

    private void Update()
    {
        if (isRotating)
            Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(transform.up, (sensitivity * speed) * Time.deltaTime);
    }
}