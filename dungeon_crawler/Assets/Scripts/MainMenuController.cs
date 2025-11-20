using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Call this when Play button is clicked
    public void PlayGame()
    {
        // Load the game scene (make sure it's added to Build Settings)
        SceneManager.LoadScene("InitialScene"); // Replace with your actual game scene name

        // Or load by index: SceneManager.LoadScene(1);
    }

    // Call this when Options button is clicked
    public void OpenOptions()
    {
        // Load options scene or open options panel
        Debug.Log("Options button clicked");
        // SceneManager.LoadScene("OptionsScene");
    }

    // Call this when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        // Note: Application.Quit() doesn't work in the Unity Editor
        // Use this for testing in editor:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Optional: Load a specific scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}