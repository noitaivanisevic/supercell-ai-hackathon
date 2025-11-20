using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneTransition : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"=== SCENE LOADED: {scene.name} ===");

        if (CharacterManager.Instance != null)
        {
            Debug.Log($"✅ CharacterManager still exists!");
            Debug.Log($"Index: {CharacterManager.Instance.selectedCharacterIndex}");
            Debug.Log($"Class: {CharacterManager.Instance.selectedCharacterClass}");
        }
        else
        {
            Debug.LogError($"❌ CharacterManager is NULL in scene {scene.name}!");
        }
    }
}