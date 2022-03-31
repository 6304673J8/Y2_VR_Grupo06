using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    [Tooltip("Time before destroying in seconds")]
    public float lifeTime = 5.0f;

    public void DestroyAfterSeconds()
    {
        Destroy(gameObject, lifeTime);
    }
}
