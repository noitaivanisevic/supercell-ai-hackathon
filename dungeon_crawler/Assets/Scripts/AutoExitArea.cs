using UnityEngine;
using System.Collections;

public class AutoExitArea : MonoBehaviour
{
    public float stayDuration = 2f;
    public GameStateManager.GameState exitToArea = GameStateManager.GameState.TownMap;
    public Vector3 spawnPositionOnExit = Vector3.zero;

    private bool hasEntered = false;

    void OnEnable()
    {
        if (!hasEntered)
        {
            hasEntered = true;
            StartCoroutine(AutoExitAfterDelay());
        }
    }

    void OnDisable()
    {
        hasEntered = false;
        StopAllCoroutines();
    }

    IEnumerator AutoExitAfterDelay()
    {
        Debug.Log("Entered restricted area. Auto-exit in " + stayDuration + " seconds...");

        float remaining = stayDuration;
        while (remaining > 0)
        {
            Debug.Log("Leaving in: " + Mathf.Ceil(remaining) + " seconds");
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
        }

        Debug.Log("Time's up! Returning to " + exitToArea);

        if (spawnPositionOnExit != Vector3.zero)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player != null)
            {
                player.position = spawnPositionOnExit;
            }
        }

        GameStateManager.Instance.SwitchState(exitToArea);
    }
}