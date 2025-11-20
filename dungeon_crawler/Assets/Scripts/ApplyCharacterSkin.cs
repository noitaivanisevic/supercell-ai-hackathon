using UnityEngine;

public class ApplyCharacterSkin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        Debug.Log($"ApplyCharacterSkin Start() on {gameObject.name}");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on " + gameObject.name);
            return;
        }

        boxCollider = GetComponent<BoxCollider2D>();

        Debug.Log($"{gameObject.name} is ready and waiting for CharacterManager...");
    }

    void OnEnable()
    {
        if (CharacterManager.Instance != null && spriteRenderer != null)
        {
            Invoke("ApplySkin", 0.1f);
        }
    }

    public void ApplySkin()
    {
        Debug.Log($"ApplySkin() called on {gameObject.name}");

        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager not found!");
            return;
        }

        Debug.Log($"=== APPLYING SKIN ===");
        Debug.Log($"Index: {CharacterManager.Instance.selectedCharacterIndex}");
        Debug.Log($"Class: {CharacterManager.Instance.selectedCharacterClass}");

        Sprite selectedSprite = CharacterManager.Instance.GetSelectedCharacterSprite();

        if (selectedSprite != null)
        {
            Debug.Log($"Sprite being applied: {selectedSprite.name}");
            spriteRenderer.sprite = selectedSprite;

            // Get parent scale to compensate
            Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;

            if (CharacterManager.Instance.selectedCharacterIndex == 0) // Fighter
            {
                // Original Fighter size: 0.3125
                float targetSize = 0.3125f;
                transform.localScale = new Vector3(
                    targetSize / parentScale.x,
                    targetSize / parentScale.y,
                    5f
                );

                if (boxCollider != null)
                {
                    boxCollider.offset = new Vector2(0f, 0f);
                    boxCollider.size = new Vector2(0.43f, 0.39f);
                }

                Debug.Log($"Applied Fighter with original size compensated. Parent scale: {parentScale}");
            }
            else
            {
                // Original other characters size: 5.0
                float targetSize = 5.0f;
                transform.localScale = new Vector3(
                    targetSize / parentScale.x,
                    targetSize / parentScale.y,
                    5f
                );

                if (boxCollider != null)
                {
                    boxCollider.offset = new Vector2(0f, 0f);
                    boxCollider.size = new Vector2(0.43f, 0.39f);
                }

                Debug.Log($"Applied {CharacterManager.Instance.selectedCharacterClass} with original size compensated. Parent scale: {parentScale}");
            }

            Debug.Log($"✅ SUCCESS! Applied {CharacterManager.Instance.selectedCharacterClass} to {gameObject.name}");
        }
        else
        {
            Debug.LogError("No sprite found for selected character!");
        }
    }
}