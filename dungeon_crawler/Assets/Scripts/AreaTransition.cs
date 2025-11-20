using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public GameStateManager.GameState targetArea;
    public Vector3 spawnPosition = Vector3.zero;

    private bool isTransitioning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger: " + other.gameObject.name + " with tag: " + other.tag);

        if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            Debug.Log("Player detected! Entering: " + targetArea);

            if (spawnPosition != Vector3.zero)
            {
                other.transform.position = spawnPosition;
            }

            GameStateManager.Instance.SwitchState(targetArea);

            Invoke("ResetTransition", 0.5f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTransitioning = false;
        }
    }

    void ResetTransition()
    {
        isTransitioning = false;
    }
}