using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private CameraFollowPlayer cameraFollow;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cameraFollow = GetComponent<CameraFollowPlayer>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        // Temporarily disable camera follow during shake
        bool wasFollowEnabled = false;
        if (cameraFollow != null)
        {
            wasFollowEnabled = cameraFollow.enabled;
            cameraFollow.enabled = false;
        }

        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        // Re-enable camera follow
        if (cameraFollow != null)
        {
            cameraFollow.enabled = wasFollowEnabled;
        }
    }
}