using UnityEngine;

public class DebugCharacterManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== CHARACTER MANAGER DEBUG ===");

        if (CharacterManager.Instance != null)
        {
            Debug.Log("✅ CharacterManager EXISTS!");
            Debug.Log($"Character: {CharacterManager.Instance.selectedCharacterClass}");
            Debug.Log($"Index: {CharacterManager.Instance.selectedCharacterIndex}");
            Debug.Log($"Health: {CharacterManager.Instance.playerHealth}");
        }
        else
        {
            Debug.LogError("❌ CharacterManager is NULL!");
        }
    }

    void Update()
    {
        // Press 'T' to test at any time
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (CharacterManager.Instance != null)
            {
                Debug.Log($"✅ Still exists! Character: {CharacterManager.Instance.selectedCharacterClass}");
            }
            else
            {
                Debug.LogError("❌ CharacterManager disappeared!");
            }
        }
    }
}