using System.Collections;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Characters")]
    public GameObject[] characters; // Drag your 6 characters here

    [Header("Particle Effects")]
    public ParticleSystem[] mistEffects; // Drag 6 mist particle systems here

    [Header("Timing")]
    public float sittingTime = 3f; // Time before character vanishes
    public float fadeOutTime = 1.5f; // Disappear duration
    public float fadeInTime = 1.5f; // Appear duration
    public float pauseBetween = 0.5f; // Gap between transitions
    public float groupDisplayTime = 3f; // Time to show all 6 characters together

    private int currentCharacterIndex = 0;
    private CanvasGroup[] canvasGroups;

    void Start()
    {
        // Set up fading for each character
        canvasGroups = new CanvasGroup[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            canvasGroups[i] = characters[i].GetComponent<CanvasGroup>();
            if (canvasGroups[i] == null)
            {
                canvasGroups[i] = characters[i].AddComponent<CanvasGroup>();
            }

            // Start ALL invisible (including first)
            canvasGroups[i].alpha = 0;
            characters[i].SetActive(false);
        }

        // Start the cycle
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (true)
        {
            // Show first character with mist
            characters[0].SetActive(true);
            canvasGroups[0].alpha = 0;
            currentCharacterIndex = 0;

            if (mistEffects[0] != null)
            {
                mistEffects[0].gameObject.SetActive(true);
                mistEffects[0].Play();
            }

            yield return StartCoroutine(FadeIn(0));

            // Cycle through remaining characters (1-5)
            for (int cycle = 0; cycle < characters.Length - 1; cycle++)
            {
                yield return new WaitForSeconds(sittingTime);

                int current = currentCharacterIndex;
                int next = (currentCharacterIndex + 1) % characters.Length;

                // Play mist and start fading out current
                if (mistEffects[current] != null)
                {
                    mistEffects[current].gameObject.SetActive(true);
                    mistEffects[current].Play();
                }

                // Start fading out current (don't wait for it to complete)
                StartCoroutine(FadeOut(current));

                // Wait only half the fade time for overlap effect
                yield return new WaitForSeconds(fadeOutTime * 0.5f);

                // Fade in next character (overlapping with fade out!)
                characters[next].SetActive(true);

                if (mistEffects[next] != null)
                {
                    mistEffects[next].gameObject.SetActive(true);
                    mistEffects[next].Play();
                }

                yield return StartCoroutine(FadeIn(next));

                characters[current].SetActive(false);
                currentCharacterIndex = next;
            }

            // Wait for last character to sit
            yield return new WaitForSeconds(sittingTime);

            // After full cycle, show all 6 characters together
            yield return StartCoroutine(ShowAllCharacters());
        }
    }

    IEnumerator ShowAllCharacters()
    {
        // Fade out last single character
        yield return StartCoroutine(FadeOut(currentCharacterIndex));
        characters[currentCharacterIndex].SetActive(false);

        // Small pause
        yield return new WaitForSeconds(pauseBetween);

        // Activate all characters (invisible at first)
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(true);
            canvasGroups[i].alpha = 0;
        }

        // Fade in all characters together
        float elapsed = 0;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInTime;
            float alpha = Mathf.SmoothStep(0, 1, t);

            for (int i = 0; i < characters.Length; i++)
            {
                canvasGroups[i].alpha = alpha;
            }
            yield return null;
        }

        // Make sure all are fully visible
        for (int i = 0; i < characters.Length; i++)
        {
            canvasGroups[i].alpha = 1;
        }

        // Display all characters for set time
        yield return new WaitForSeconds(groupDisplayTime);

        // Fade out all characters
        elapsed = 0;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutTime;
            float alpha = 1 - (t * t);

            for (int i = 0; i < characters.Length; i++)
            {
                canvasGroups[i].alpha = alpha;
            }
            yield return null;
        }

        // Deactivate all and reset to invisible
        for (int i = 0; i < characters.Length; i++)
        {
            canvasGroups[i].alpha = 0;
            characters[i].SetActive(false);
        }

        // Small pause before loop restarts
        yield return new WaitForSeconds(pauseBetween);
    }

    IEnumerator FadeOut(int index)
    {
        float elapsed = 0;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutTime;
            // Smooth ease out curve
            canvasGroups[index].alpha = 1 - (t * t);
            yield return null;
        }
        canvasGroups[index].alpha = 0;
    }

    IEnumerator FadeIn(int index)
    {
        float elapsed = 0;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInTime;
            // Smooth ease in curve
            canvasGroups[index].alpha = Mathf.SmoothStep(0, 1, t);
            yield return null;
        }
        canvasGroups[index].alpha = 1;
    }
}