using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject selectionPanel;
    public Button startButton;
    public TextMeshProUGUI feedbackText;
    
    [Header("Settings")]
    public string nextSceneName = "FirstMap";
    
    public static string SelectedCharacterName { get; private set; }
    public static int SelectedCharacterIndex { get; private set; } = -1;
    public static Sprite SelectedCharacterSprite { get; private set; }
    
    private CharacterCard currentlySelected = null;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (startButton != null)
        {
            startButton.interactable = false;
        }
        
        UpdateFeedback();
    }
    
    public void OnCharacterSelected(CharacterCard card)
    {
        if (currentlySelected != null && currentlySelected != card)
        {
            currentlySelected.Deselect();
        }
        
        currentlySelected = card;
        SelectedCharacterName = card.characterName;
        SelectedCharacterIndex = card.characterIndex;
        SelectedCharacterSprite = card.characterSprite;
        
        if (startButton != null)
        {
            startButton.interactable = true;
        }
        
        UpdateFeedback();
        
        Debug.Log($"Selected: {SelectedCharacterName} (Index: {SelectedCharacterIndex})");
    }
    
    public void OnCharacterDeselected(CharacterCard card)
    {
        if (currentlySelected == card)
        {
            currentlySelected = null;
            SelectedCharacterName = null;
            SelectedCharacterIndex = -1;
            SelectedCharacterSprite = null;
            
            if (startButton != null)
            {
                startButton.interactable = false;
            }
            
            UpdateFeedback();
        }
    }
    
    void UpdateFeedback()
    {
        if (feedbackText != null)
        {
            if (SelectedCharacterIndex == -1)
            {
                feedbackText.text = "Select your character to continue";
                feedbackText.color = new Color(1f, 0.7f, 0.7f);
            }
            else
            {
                feedbackText.text = $"{SelectedCharacterName} selected!";
                feedbackText.color = new Color(0.7f, 1f, 0.7f);
            }
        }
    }
    
    public void OnStartButtonPressed()
    {
        if (SelectedCharacterIndex != -1)
        {
            PlayerPrefs.SetString("SelectedCharacter", SelectedCharacterName);
            PlayerPrefs.SetInt("SelectedCharacterIndex", SelectedCharacterIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"Saved character: {SelectedCharacterName} (Index: {SelectedCharacterIndex})");
            
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No character selected!");
        }
    }
    
    public static bool HasSelection()
    {
        return SelectedCharacterIndex != -1;
    }
}