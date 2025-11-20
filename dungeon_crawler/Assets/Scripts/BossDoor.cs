using UnityEngine;
using System.Collections;

public class BossDoor : MonoBehaviour
{
    [Header("Earthquake Settings")]
    public float earthquakeDuration = 3f;
    public float earthquakeMagnitude = 0.3f;

    [Header("Timing")]
    public float doorOpenDisplayTime = 2f;

    [Header("Audio (Optional)")]
    public AudioSource earthquakeSound;
    public AudioSource doorOpenSound;

    [Header("Background Sprites")]
    public Sprite dungeon3OpenDoorSprite;  // Assign your open door background here

    private bool bossSequenceStarted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bossSequenceStarted)
        {
            bossSequenceStarted = true;
            StartCoroutine(BossBattleSequence());
        }
    }

    IEnumerator BossBattleSequence()
    {
        Debug.Log("Player approached the boss door!");

        // Find player and disable ALL MonoBehaviour components (movement scripts)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        MonoBehaviour[] playerScripts = null;

        if (player != null)
        {
            // Get all scripts on the player
            playerScripts = player.GetComponents<MonoBehaviour>();

            // Disable all movement-related scripts
            foreach (MonoBehaviour script in playerScripts)
            {
                // Don't disable this script or certain system scripts
                if (script != null && script.GetType().Name != "Transform")
                {
                    script.enabled = false;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Step 1: EARTHQUAKE (player stays visible)
        Debug.Log("Earthquake starting!");

        if (earthquakeSound != null)
        {
            earthquakeSound.Play();
        }

        if (CameraShake.Instance != null)
        {
            yield return StartCoroutine(CameraShake.Instance.Shake(earthquakeDuration, earthquakeMagnitude));
        }

        yield return new WaitForSeconds(0.3f);

        // Step 2: Change background sprite to show open door
        Debug.Log("Door opened!");

        if (GameStateManager.Instance.backgroundRenderer != null && dungeon3OpenDoorSprite != null)
        {
            GameStateManager.Instance.backgroundRenderer.sprite = dungeon3OpenDoorSprite;
        }

        if (doorOpenSound != null)
        {
            doorOpenSound.Play();
        }

        // Optional: Make player walk through door
        if (player != null)
        {
            Vector3 startPos = player.transform.position;
            Vector3 targetPos = startPos + new Vector3(3f, 0, 0);  // Walk 3 units forward
            float walkTime = doorOpenDisplayTime;
            float elapsed = 0f;

            while (elapsed < walkTime)
            {
                player.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / walkTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(doorOpenDisplayTime);
        }

        // Step 3: Switch to Battle!
        Debug.Log("Boss battle begins!");
        GameStateManager.Instance.SwitchState(GameStateManager.GameState.Battle);

        yield return new WaitForSeconds(0.5f);

        // Step 4: Spawn the 3 Hydras
        //if (HydraBattleManager.Instance != null)
        //{
         //   HydraBattleManager.Instance.StartHydraBattle(3);
        //}
    }
}