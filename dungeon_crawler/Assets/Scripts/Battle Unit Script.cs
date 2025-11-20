using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [Header("Unit Stats")]
    public string unitName = "Unit";
    public int maxHealth = 100;
    public int currentHealth;
    public int attackPower = 20;
    public int defense = 5;

    [Header("Is This The Player?")]
    public bool isPlayer = false;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    private bool isDefending = false;

    void Start()
    {
        // If this is the player, use CharacterManager stats
        if (isPlayer && CharacterManager.Instance != null)
        {
            unitName = CharacterManager.Instance.selectedCharacterClass;
            maxHealth = CharacterManager.Instance.playerMaxHealth;
            currentHealth = CharacterManager.Instance.playerHealth;
            attackPower = CharacterManager.Instance.playerDamage;
            defense = CharacterManager.Instance.playerDefense;

            // Apply player sprite
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = CharacterManager.Instance.GetSelectedCharacterSprite();
            }

            Debug.Log($"Player battle unit loaded: {unitName} - HP:{currentHealth}/{maxHealth}");
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public int Attack()
    {
        int damage = attackPower + Random.Range(-5, 6);
        return Mathf.Max(damage, 1);
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = damage;

        if (isDefending)
        {
            actualDamage = Mathf.Max(damage - defense * 2, 0);
            Debug.Log($"{unitName} blocked some damage!");
        }
        else
        {
            actualDamage = Mathf.Max(damage - defense, 0);
        }

        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Save player health to CharacterManager
        if (isPlayer && CharacterManager.Instance != null)
        {
            CharacterManager.Instance.playerHealth = currentHealth;
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        Debug.Log($"{unitName} took {actualDamage} damage! HP: {currentHealth}/{maxHealth}");
    }

    public void Defend()
    {
        isDefending = true;
    }

    public void ResetDefense()
    {
        isDefending = false;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Save player health to CharacterManager
        if (isPlayer && CharacterManager.Instance != null)
        {
            CharacterManager.Instance.playerHealth = currentHealth;
        }

        Debug.Log($"{unitName} healed {amount} HP! HP: {currentHealth}/{maxHealth}");
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    System.Collections.IEnumerator FlashRed()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = original;
    }
}