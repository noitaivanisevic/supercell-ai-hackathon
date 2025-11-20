using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ClassIntroController : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject openingScene;
    public GameObject[] characterScenes;
    
    [Header("Opening Scene")]
    public TextMeshProUGUI openingText;
    public TextMeshProUGUI questionText;
    
    [Header("Continue Buttons")]
    public Button[] continueButtons;
    
    [Header("Settings")]
    public float typewriterSpeed = 0.05f;
    public float fadeDuration = 1f;
    public Color flameColor1 = new Color(1f, 0.3f, 0f, 1f);
    public Color flameColor2 = new Color(0.8f, 0.1f, 0f, 1f);
    public bool autoAdvance = false;
    public float autoAdvanceDelay = 4f;
    public KeyCode skipKey = KeyCode.Space;
    
    [Header("Scene Transition")]
    public bool useSceneTransition = false;
    public string nextSceneName = "FirstMap";
    public GameObject selectionCanvas;
    
    private int currentSceneIndex = -1;
    private CanvasGroup openingCanvasGroup;
    private CanvasGroup[] characterCanvasGroups;
    private bool isTransitioning = false;
    private bool waitingForInput = false;
    
    void Start()
    {
        SetupCanvasGroups();
        SetupButtons();
        StartCoroutine(PlayIntroSequence());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(skipKey) && waitingForInput && !isTransitioning)
        {
            OnContinuePressed();
        }
    }
    
    void SetupCanvasGroups()
    {
        openingCanvasGroup = openingScene.GetComponent<CanvasGroup>();
        if (openingCanvasGroup == null)
            openingCanvasGroup = openingScene.AddComponent<CanvasGroup>();
        
        characterCanvasGroups = new CanvasGroup[characterScenes.Length];
        for (int i = 0; i < characterScenes.Length; i++)
        {
            if (characterScenes[i] != null)
            {
                characterCanvasGroups[i] = characterScenes[i].GetComponent<CanvasGroup>();
                if (characterCanvasGroups[i] == null)
                    characterCanvasGroups[i] = characterScenes[i].AddComponent<CanvasGroup>();
                
                characterScenes[i].SetActive(false);
                characterCanvasGroups[i].alpha = 0f;
            }
        }
        
        openingScene.SetActive(true);
        openingCanvasGroup.alpha = 0f;
    }
    
    void SetupButtons()
    {
        for (int i = 0; i < continueButtons.Length; i++)
        {
            if (continueButtons[i] != null)
            {
                continueButtons[i].onClick.AddListener(OnContinuePressed);
                continueButtons[i].gameObject.SetActive(false);
            }
        }
    }
    
    IEnumerator PlayIntroSequence()
    {
        yield return ShowOpeningWithFlameText();
        
        for (int i = 0; i < characterScenes.Length; i++)
        {
            if (characterScenes[i] != null)
            {
                yield return TransitionToCharacter(i);
                
                waitingForInput = true;
                ShowContinueButton(i, true);
                
                if (autoAdvance)
                {
                    yield return new WaitForSeconds(autoAdvanceDelay);
                    waitingForInput = false;
                }
                else
                {
                    while (waitingForInput)
                    {
                        yield return null;
                    }
                }
                
                ShowContinueButton(i, false);
            }
        }
        
        LoadClassSelection();
    }
    
    IEnumerator ShowOpeningWithFlameText()
    {
        isTransitioning = true;
        openingScene.SetActive(true);
        
        string openingFullText = openingText.text;
        string questionFullText = questionText.text;
        openingText.text = "";
        questionText.text = "";
        
        yield return FadeCanvasGroup(openingCanvasGroup, 0f, 1f, fadeDuration);
        
        yield return FlameTypewriter(openingText, openingFullText, typewriterSpeed);
        yield return new WaitForSeconds(0.5f);
        yield return FadeOutText(openingText, 1f);
        yield return FlameTypewriter(questionText, questionFullText, typewriterSpeed);
        
        isTransitioning = false;
    }
    
    IEnumerator FlameTypewriter(TextMeshProUGUI textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        textComponent.color = flameColor1;
        
        Coroutine pulseCoroutine = StartCoroutine(ContinuousPulse(textComponent));
        
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }
        
        StopCoroutine(pulseCoroutine);
    }
    
    IEnumerator ContinuousPulse(TextMeshProUGUI textComponent)
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * 2f, 1f);
            textComponent.color = Color.Lerp(flameColor1, flameColor2, t);
            yield return null;
        }
    }
    
    IEnumerator FadeOutText(TextMeshProUGUI textComponent, float duration)
    {
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textComponent.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        
        textComponent.color = endColor;
        textComponent.text = "";
    }
    
    void ShowContinueButton(int sceneIndex, bool show)
    {
        if (sceneIndex >= 0 && sceneIndex < continueButtons.Length && continueButtons[sceneIndex] != null)
        {
            continueButtons[sceneIndex].gameObject.SetActive(show);
        }
    }
    
    public void OnContinuePressed()
    {
        if (waitingForInput)
        {
            waitingForInput = false;
        }
    }
    
    IEnumerator TransitionToCharacter(int sceneIndex)
    {
        isTransitioning = true;
        
        if (currentSceneIndex == -1)
        {
            yield return FadeCanvasGroup(openingCanvasGroup, 1f, 0f, fadeDuration);
            openingScene.SetActive(false);
        }
        else
        {
            if (currentSceneIndex < characterCanvasGroups.Length && characterCanvasGroups[currentSceneIndex] != null)
            {
                yield return FadeCanvasGroup(characterCanvasGroups[currentSceneIndex], 1f, 0f, fadeDuration);
                characterScenes[currentSceneIndex].SetActive(false);
            }
        }
        
        currentSceneIndex = sceneIndex;
        
        characterScenes[sceneIndex].SetActive(true);
        yield return FadeCanvasGroup(characterCanvasGroups[sceneIndex], 0f, 1f, fadeDuration);
        
        isTransitioning = false;
    }
    
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }
    
    void LoadClassSelection()
    {
        if (useSceneTransition)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            if (selectionCanvas != null)
            {
                openingScene.SetActive(false);
                
                for (int i = 0; i < characterScenes.Length; i++)
                {
                    if (characterScenes[i] != null)
                    {
                        characterScenes[i].SetActive(false);
                    }
                }
                
                selectionCanvas.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Selection Canvas not assigned!");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
