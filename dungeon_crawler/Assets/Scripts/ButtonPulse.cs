using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Makes a UI button pulse/breathe to draw attention
/// Attach this to your Continue button GameObject
/// </summary>
public class ButtonPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private bool pulseScale = true;
    [SerializeField] private bool pulseColor = true;
    
    [Header("Scale Pulse")]
    [SerializeField] private float scaleMin = 1f;
    [SerializeField] private float scaleMax = 1.05f;
    [SerializeField] private float scaleSpeed = 0.6f;
    
    [Header("Color Pulse")]
    [SerializeField] private Color normalColor = new Color(0.16f, 0.16f, 0.24f, 0.86f); // Dark blue-gray
    [SerializeField] private Color highlightColor = new Color(0.31f, 0.24f, 0.16f, 1f); // Warm brown-gold
    
    private Image buttonImage;
    private Vector3 originalScale;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalScale = transform.localScale;
        
        // If no image component, try to get it from child
        if (buttonImage == null)
        {
            buttonImage = GetComponentInChildren<Image>();
        }
    }
    
    void Update()
    {
        float pulse = Mathf.PingPong(Time.time * scaleSpeed, 1f);
        
        // Pulse scale (breathing effect)
        if (pulseScale)
        {
            float scale = Mathf.Lerp(scaleMin, scaleMax, pulse);
            transform.localScale = originalScale * scale;
        }
        
        // Pulse color (glowing effect)
        if (pulseColor && buttonImage != null)
        {
            buttonImage.color = Color.Lerp(normalColor, highlightColor, pulse);
        }
    }
    
    void OnDisable()
    {
        // Reset to original state when disabled
        if (pulseScale)
        {
            transform.localScale = originalScale;
        }
        
        if (pulseColor && buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
}