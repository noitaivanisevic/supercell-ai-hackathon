using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("Character Display")]
    public Image characterPreviewImage;
    public Text characterNameText;
    public Text healthText;
    public Text damageText;
    public Text speedText;
    public Text defenseText;

    [Header("Selection")]
    public int currentCharacterIndex = 0;

    // Temporary preview data (NOT saved)
    private string[] characterNames = { "Fighter", "Knight", "Thief", "Beast Class", "Vampire", "Archer" };
    private int[] characterHealth = { 100, 120, 80, 150, 90, 75 };
    private int[] characterDamage = { 15, 18, 20, 12, 22, 19 };
    private int[] characterSpeed = { 2, 2, 4, 3, 3, 3 };
    private int[] characterDefense = { 5, 8, 3, 6, 4, 4 };

    void Start()
    {
        UpdateCharacterDisplay();
    }

    public void NextCharacter()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex > 5)
        {
            currentCharacterIndex = 0;
        }
        UpdateCharacterDisplay();
    }

    public void PreviousCharacter()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = 5;
        }
        UpdateCharacterDisplay();
    }

    public void ConfirmSelection()
    {
        if (CharacterManager.Instance != null)
        {
            // ONLY save when user confirms!
            CharacterManager.Instance.SelectCharacter(currentCharacterIndex);
            Debug.Log($"✅ CONFIRMED character index {currentCharacterIndex}: {CharacterManager.Instance.selectedCharacterClass}");

            // Start the game - check if GameStateManager exists
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SwitchState(GameStateManager.GameState.TownMap);
            }
            else
            {
                Debug.LogWarning("GameStateManager not found - it should be created when entering the game scene");
            }
        }
        else
        {
            Debug.LogError("CharacterManager not found!");
        }
    }

    void UpdateCharacterDisplay()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager not found!");
            return;
        }

        // Just PREVIEW - don't save!
        if (characterPreviewImage != null)
        {
            characterPreviewImage.sprite = GetSpriteForIndex(currentCharacterIndex);
        }

        if (characterNameText != null)
        {
            characterNameText.text = characterNames[currentCharacterIndex];
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {characterHealth[currentCharacterIndex]}";
        }

        if (damageText != null)
        {
            damageText.text = $"ATK: {characterDamage[currentCharacterIndex]}";
        }

        if (speedText != null)
        {
            speedText.text = $"SPD: {characterSpeed[currentCharacterIndex]}";
        }

        if (defenseText != null)
        {
            defenseText.text = $"DEF: {characterDefense[currentCharacterIndex]}";
        }

        Debug.Log($"👁️ Previewing character index {currentCharacterIndex}: {characterNames[currentCharacterIndex]}");
    }

    Sprite GetSpriteForIndex(int index)
    {
        if (CharacterManager.Instance == null) return null;

        switch (index)
        {
            case 0: return CharacterManager.Instance.fighter;
            case 1: return CharacterManager.Instance.knight;
            case 2: return CharacterManager.Instance.thief;
            case 3: return CharacterManager.Instance.beastClass;
            case 4: return CharacterManager.Instance.vampire;
            case 5: return CharacterManager.Instance.archer;
            default: return CharacterManager.Instance.fighter;
        }
    }
}