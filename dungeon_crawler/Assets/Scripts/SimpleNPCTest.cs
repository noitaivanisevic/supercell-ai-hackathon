using UnityEngine;

public class SimpleNPCTest : MonoBehaviour
{
    public GameObject chatBubble;

    void Start()
    {
        Debug.Log("SimpleNPCTest is working on: " + gameObject.name);

        if (chatBubble != null)
        {
            chatBubble.SetActive(false);
            Debug.Log("Chat bubble hidden at start");
        }
    }

    void Update()
    {
        // Test for = key (Equals key)
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Debug.Log("= key detected!");

            if (chatBubble != null)
            {
                chatBubble.SetActive(!chatBubble.activeSelf);
                Debug.Log("Toggled chat bubble to: " + chatBubble.activeSelf);
            }
        }
    }
}