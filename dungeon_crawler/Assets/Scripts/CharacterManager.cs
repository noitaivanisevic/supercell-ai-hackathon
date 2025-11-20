using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    private bool hasInitialized = false;

    [Header("Available Character Sprites")]
    public Sprite fighter;      // Index 0
    public Sprite knight;       // Index 1
    public Sprite thief;        // Index 2
    public Sprite beastClass;   // Index 3
    public Sprite vampire;      // Index 4
    public Sprite archer;       // Index 5

    [Header("Currently Selected Character")]
    public string selectedCharacterClass = "";
    public int selectedCharacterIndex = -1;

    [Header("Character Stats")]
    public int playerHealth = 100;
    public int playerMaxHealth = 100;
    public int playerDamage = 15;
    public int playerSpeed = 2;
    public int playerDefense = 5;

    void Awake()
    {
        // FORCE reset to prevent Inspector values from interfering
        selectedCharacterIndex = -1;
        selectedCharacterClass = "";

        Debug.Log($"=== CharacterManager Awake START === Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CharacterManager created and set to DontDestroyOnLoad");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate CharacterManager destroyed");
            return;
        }

        Debug.Log($"=== CharacterManager Awake END === Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}");
    }

    void Start()
    {
        Debug.Log($"=== CharacterManager Start BEGIN === Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}, hasInitialized: {hasInitialized}");

        if (!hasInitialized)
        {
            Debug.Log("First initialization - about to load saved character...");
            LoadSavedCharacter();
            hasInitialized = true;
            Debug.Log($"After LoadSavedCharacter - Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}");
        }
        else
        {
            Debug.Log("Already initialized - skipping LoadSavedCharacter");
        }

        Debug.Log($"=== CharacterManager Start END === Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}");

        // Tell all ApplyCharacterSkin scripts to apply now
        StartCoroutine(NotifySkinsToApply());
    }

    IEnumerator NotifySkinsToApply()
    {
        yield return null; // Wait one frame

        Debug.Log("Broadcasting to all ApplyCharacterSkin scripts...");
        ApplyCharacterSkin[] skins = FindObjectsByType<ApplyCharacterSkin>(FindObjectsSortMode.None);
        foreach (var skin in skins)
        {
            skin.ApplySkin();
        }
    }

    void Update()
    {
        // Press 'P' to print what's in PlayerPrefs
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("=== PLAYERPREFS DEBUG ===");
            Debug.Log($"Saved Index: {PlayerPrefs.GetInt("SelectedCharacterIndex", -999)}");
            Debug.Log($"Saved Class: {PlayerPrefs.GetString("SelectedCharacter", "NONE")}");
            Debug.Log($"Current Index: {selectedCharacterIndex}");
            Debug.Log($"Current Class: {selectedCharacterClass}");
        }

        // Press 'R' to force refresh the sprite
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Force refreshing all player sprites...");
            ApplyCharacterSkin[] skinAppliers = FindObjectsByType<ApplyCharacterSkin>(FindObjectsSortMode.None);
            foreach (var applier in skinAppliers)
            {
                applier.ApplySkin();
            }
        }
    }

    public void SelectCharacter(int characterIndex)
    {
        Debug.Log($"=== SelectCharacter CALLED === Changing from Index {selectedCharacterIndex} to {characterIndex}");

        selectedCharacterIndex = characterIndex;

        switch (characterIndex)
        {
            case 0: // Fighter
                ApplyCharacterStats("Fighter", 100, 15, 2, 5);
                break;

            case 1: // Knight
                ApplyCharacterStats("Knight", 120, 18, 2, 8);
                break;

            case 2: // Thief
                ApplyCharacterStats("Thief", 80, 20, 4, 3);
                break;

            case 3: // Beast Class
                ApplyCharacterStats("Beast Class", 150, 12, 3, 6);
                break;

            case 4: // Vampire
                ApplyCharacterStats("Vampire", 90, 22, 3, 4);
                break;

            case 5: // Archer
                ApplyCharacterStats("Archer", 75, 19, 3, 4);
                break;

            default:
                ApplyCharacterStats("Fighter", 100, 15, 2, 5);
                break;
        }

        SaveCharacterSelection();
        Debug.Log($"=== SelectCharacter COMPLETE === Index {characterIndex} = {selectedCharacterClass}");
    }

    void ApplyCharacterStats(string className, int health, int damage, int speed, int defense)
    {
        Debug.Log($"ApplyCharacterStats called: {className}");
        selectedCharacterClass = className;
        playerMaxHealth = health;
        playerHealth = health;
        playerDamage = damage;
        playerSpeed = speed;
        playerDefense = defense;
    }

    public Sprite GetSelectedCharacterSprite()
    {
        Debug.Log($"GetSelectedCharacterSprite called - Index: {selectedCharacterIndex}, Class: {selectedCharacterClass}");

        switch (selectedCharacterIndex)
        {
            case 0:
                Debug.Log("Returning fighter sprite");
                return fighter;
            case 1:
                Debug.Log("Returning knight sprite");
                return knight;
            case 2:
                Debug.Log("Returning thief sprite");
                return thief;
            case 3:
                Debug.Log("Returning beastClass sprite");
                return beastClass;
            case 4:
                Debug.Log("Returning vampire sprite");
                return vampire;
            case 5:
                Debug.Log("Returning archer sprite");
                return archer;
            default:
                Debug.LogWarning($"Unknown character index: {selectedCharacterIndex}, returning fighter");
                return fighter;
        }
    }

    public void SaveCharacterSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacterClass);
        PlayerPrefs.SetInt("PlayerMaxHealth", playerMaxHealth);
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.SetInt("PlayerDamage", playerDamage);
        PlayerPrefs.SetInt("PlayerSpeed", playerSpeed);
        PlayerPrefs.SetInt("PlayerDefense", playerDefense);
        PlayerPrefs.Save();

        Debug.Log($"💾 SAVED to PlayerPrefs: {selectedCharacterClass} (Index: {selectedCharacterIndex})");
    }

    void LoadSavedCharacter()
    {
        Debug.Log("=== LoadSavedCharacter START ===");

        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            int loadedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
            string loadedClass = PlayerPrefs.GetString("SelectedCharacter", "Fighter");

            Debug.Log($"Found saved data in PlayerPrefs - Index: {loadedIndex}, Class: {loadedClass}");

            selectedCharacterIndex = loadedIndex;
            selectedCharacterClass = loadedClass;
            playerMaxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
            playerHealth = PlayerPrefs.GetInt("PlayerHealth", 100);
            playerDamage = PlayerPrefs.GetInt("PlayerDamage", 15);
            playerSpeed = PlayerPrefs.GetInt("PlayerSpeed", 2);
            playerDefense = PlayerPrefs.GetInt("PlayerDefense", 5);

            Debug.Log($"✅ Loaded saved character: {selectedCharacterClass} (Index: {selectedCharacterIndex})");
        }
        else
        {
            Debug.Log("⚠️ No saved character found, defaulting to Fighter");
            selectedCharacterIndex = 0;
            selectedCharacterClass = "Fighter";
            playerMaxHealth = 100;
            playerHealth = 100;
            playerDamage = 15;
            playerSpeed = 2;
            playerDefense = 5;
        }

        Debug.Log("=== LoadSavedCharacter END ===");
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(damage - playerDefense, 0);
        playerHealth -= actualDamage;
        playerHealth = Mathf.Max(playerHealth, 0);
        SaveCharacterSelection();
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Min(playerHealth, playerMaxHealth);
        SaveCharacterSelection();
    }

    public bool IsDead()
    {
        return playerHealth <= 0;
    }

    public void RestoreHealth()
    {
        playerHealth = playerMaxHealth;
        SaveCharacterSelection();
    }
}