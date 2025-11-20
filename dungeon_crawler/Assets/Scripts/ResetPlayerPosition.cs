using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    public Transform playerTransform;  // Drag the player here
    public Vector3 startPosition = new Vector3(-15.02002f, -2.168752f, 0f);

    void OnEnable()
    {
        // When this area becomes active, reset player position
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Debug.Log("Reset player to starting position: " + startPosition);
        }
    }
}