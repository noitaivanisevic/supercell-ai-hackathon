using UnityEngine;
using System.Collections;

public class RandomEncounterSystem : MonoBehaviour
{
    [Header("Encounter Settings")]
    [Range(0f, 100f)]
    public float encounterChance = 20f;
    public float checkInterval = 2f;

    [Header("Requirements")]
    public float minDistanceToTrigger = 1f;

    [Header("BATTLE Enemy Setup (Must have BattleUnit component!)")]
    public GameObject[] battleEnemyPrefabs; // These must have BattleUnit components!
    public int minEnemies = 1;
    public int maxEnemies = 3;

    private Transform player;
    private Vector3 lastPosition;
    private float timeSinceLastCheck = 0f;
    private bool encounterActive = false;

    void Start()
    {
        player = transform;
        lastPosition = player.position;

        // Validate enemy prefabs on start
        ValidateEnemyPrefabs();
    }

    void ValidateEnemyPrefabs()
    {
        if (battleEnemyPrefabs == null || battleEnemyPrefabs.Length == 0)
        {
            Debug.LogError($"[{gameObject.name}] No battle enemy prefabs assigned!");
            return;
        }

        foreach (GameObject enemy in battleEnemyPrefabs)
        {
            if (enemy == null)
            {
                Debug.LogError($"[{gameObject.name}] Null enemy in battleEnemyPrefabs array!");
                continue;
            }

            BattleUnit unit = enemy.GetComponent<BattleUnit>();
            if (unit == null)
            {
                Debug.LogError($"[{gameObject.name}] Enemy '{enemy.name}' is missing BattleUnit component!");
            }
            else
            {
                Debug.Log($"[{gameObject.name}] Enemy '{enemy.name}' validated - HP:{unit.maxHealth}, ATK:{unit.attackPower}");
            }
        }
    }

    void Update()
    {
        // Check if GameStateManager exists
        if (GameStateManager.Instance == null)
        {
            return;
        }

        // Don't check during battles
        if (encounterActive) return;
        if (GameStateManager.Instance.GetCurrentState() == GameStateManager.GameState.Battle) return;

        // Only check in dungeons
        GameStateManager.GameState currentState = GameStateManager.Instance.GetCurrentState();
        if (currentState != GameStateManager.GameState.Dungeon1 &&
            currentState != GameStateManager.GameState.Dungeon2 &&
            currentState != GameStateManager.GameState.Dungeon3)
        {
            return;
        }

        timeSinceLastCheck += Time.deltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            float distanceMoved = Vector3.Distance(player.position, lastPosition);

            if (distanceMoved >= minDistanceToTrigger)
            {
                CheckForEncounter();
            }

            lastPosition = player.position;
            timeSinceLastCheck = 0f;
        }
    }

    void CheckForEncounter()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= encounterChance)
        {
            Debug.Log($"🎲 Random encounter! (Rolled {roll}, needed ≤{encounterChance})");
            TriggerEncounter();
        }
    }

    void TriggerEncounter()
    {
        if (battleEnemyPrefabs == null || battleEnemyPrefabs.Length == 0)
        {
            Debug.LogError("Cannot start battle - no enemy prefabs assigned!");
            return;
        }

        encounterActive = true;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        Debug.Log("⚔️ Battle sequence starting...");

        // Freeze player
        MonoBehaviour[] playerScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this && script != null)
            {
                script.enabled = false;
            }
        }

        yield return new WaitForSeconds(0.3f);

        // Switch to battle state FIRST
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SwitchState(GameStateManager.GameState.Battle);
        }

        yield return new WaitForSeconds(0.5f);

        // NOW spawn enemies
        if (BattleManager.Instance != null)
        {
            int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
            Debug.Log($"📡 Calling BattleManager with {enemyCount} enemies");
            BattleManager.Instance.StartRandomBattle(battleEnemyPrefabs, enemyCount);
        }
        else
        {
            Debug.LogError("BattleManager.Instance is NULL!");
        }
    }

    // This method is called by BattleManager when battle ends
    public void OnBattleEnd()
    {
        Debug.Log("🏁 Battle ended, re-enabling player");
        encounterActive = false;

        // Re-enable player scripts
        MonoBehaviour[] playerScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != this && script != null)
            {
                script.enabled = true;
            }
        }
    }
}