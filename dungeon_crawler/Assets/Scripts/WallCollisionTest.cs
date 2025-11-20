using UnityEngine;

public class WallCollisionTest : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("WALL HIT by: " + collision.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("WALL TRIGGER by: " + other.gameObject.name);
    }
}