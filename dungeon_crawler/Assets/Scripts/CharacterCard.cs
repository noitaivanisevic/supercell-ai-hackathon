using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCard : MonoBehaviour
{
    [Header("Character Data")]
    public string characterName = "Fighter";
    public int characterIndex = 0;
    public Sprite characterSprite;
    
    [Header("UI References")]
    public Image cardBackground;
    public Image characterSpriteImage;
    public TextMeshProUGUI characterNameText;
    
    [Header("Visual Settings")]
    public Color normalColor = new Color(0.12f, 0.12f, 0.16f, 1f);
    public Color selectedColor = new Color(0.3f, 0.2f, 0.1f, 1f);
    public Color selectedBorderColor = new Color(1f, 0.84f, 0f, 1f);
    
    private bool isSelected = false;
    private Button button;
    private Outline outline;
    
    void Start()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        
        button.onClick.AddListener(OnCardClicked);
        
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        
        outline.effectColor = selectedBorderColor;
        outline.effectDistance = new Vector2(4, 4);
        outline.enabled = false;
        
        if (cardBackground == null)
        {
            cardBackground = GetComponent<Image>();
        }
        
        if (cardBackground != null)
        {
            cardBackground.color = normalColor;
        }
        
        if (characterNameText != null)
        {
            characterNameText.text = characterName;
        }
        
        if (characterSpriteImage != null)
        {
            characterSpriteImage.sprite = characterSprite;
        }
    }
    
    void OnCardClicked()
    {
        if (!isSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }
    
    public void Select()
    {
        isSelected = true;
        
        if (cardBackground != null)
        {
            cardBackground.color = selectedColor;
        }
        
        if (outline != null)
        {
            outline.enabled = true;
        }
        
        transform.localScale = Vector3.one * 1.05f;
        
        if (CharacterSelectionManager.Instance != null)
        {
            CharacterSelectionManager.Instance.OnCharacterSelected(this);
        }
    }
    
    public void Deselect()
    {
        isSelected = false;
        
        if (cardBackground != null)
        {
            cardBackground.color = normalColor;
        }
        
        if (outline != null)
        {
            outline.enabled = false;
        }
        
        transform.localScale = Vector3.one;
        
        if (CharacterSelectionManager.Instance != null)
        {
            CharacterSelectionManager.Instance.OnCharacterDeselected(this);
        }
    }
}