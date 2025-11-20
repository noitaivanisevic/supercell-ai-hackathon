using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    [Header("Player References")]
    public SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        LoadSelectedCharacter();
    }

    void LoadSelectedCharacter()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.LogWarning("CharacterManager no encontrado!");
            return;
        }

        // Obtener sprite del CharacterManager
        Sprite characterSprite = CharacterManager.Instance.GetSelectedCharacterSprite();

        // Aplicar sprite al jugador
        if (playerSpriteRenderer != null && characterSprite != null)
        {
            playerSpriteRenderer.sprite = characterSprite;
            Debug.Log($"Sprite aplicado: {CharacterManager.Instance.selectedCharacterClass}");
        }
        else
        {
            Debug.LogWarning("SpriteRenderer o sprite no asignado!");
        }

        // Log de información del personaje
        Debug.Log($"Personaje cargado: {CharacterManager.Instance.selectedCharacterClass}");
        Debug.Log($"HP: {CharacterManager.Instance.playerHealth}/{CharacterManager.Instance.playerMaxHealth}");
        Debug.Log($"Daño: {CharacterManager.Instance.playerDamage}");
        Debug.Log($"Velocidad: {CharacterManager.Instance.playerSpeed}");
        Debug.Log($"Defensa: {CharacterManager.Instance.playerDefense}");
    }

    // Método opcional para actualizar el sprite en cualquier momento
    public void RefreshSprite()
    {
        if (CharacterManager.Instance != null && playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = CharacterManager.Instance.GetSelectedCharacterSprite();
        }
    }
}