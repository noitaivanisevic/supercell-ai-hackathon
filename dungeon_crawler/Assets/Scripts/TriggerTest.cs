using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER HIT by: " + other.gameObject.name + " | Tag: " + other.tag);
    }
}