using UnityEngine;

public class ResetPositionOnEnable : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(-15.02002f, -2.168752f, 0f);

    void OnEnable()
    {
        // Every time this object is enabled (area becomes active), reset position
        transform.position = startPosition;
        Debug.Log(gameObject.name + " position reset to: " + startPosition);
    }
}