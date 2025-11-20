using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Battle Setup")]
    public Transform playerBattlePosition;
    public Transform[] enemySpawnPositions;

    [Header("Player Reference")]
    public GameObject playerBattlePrefab;

    [Header("UI References")]
    public GameObject battleUI;
    public UnityEngine.UI.Text playerHealthText;
    public UnityEngine.UI.Text enemyHealthText;
    public UnityEngine.UI.Text battleLogText;

    private GameObject playerBattleInstance;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private BattleUnit playerUnit;
    private BattleUnit currentEnemyTarget;
    private bool battleActive = false;
    private GameStateManager.GameState returnState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (battleUI != null)
        {
            battleUI.SetActive(false);
        }
    }

    void Update()
    {
        // Press B to manually test battle
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("🔧 DEBUG: Manual battle trigger");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                RandomEncounterSystem encounters = player.GetComponent<RandomEncounterSystem>();
                if (encounters != null && encounters.battleEnemyPrefabs != null && encounters.battleEnemyPrefabs.Length > 0)
                {
                    Debug.Log($"✅ Found {encounters.battleEnemyPrefabs.Length} enemy prefabs, starting battle");
                    StartRandomBattle(encounters.battleEnemyPrefabs, 2);
                }
                else
                {
                    Debug.LogError("❌ Player has no RandomEncounterSystem or no enemies assigned!");
                }
            }
            else
            {
                Debug.LogError("❌ No player found with 'Player' tag!");
            }
        }
    }

    public void StartRandomBattle(GameObject[] possibleEnemies, int enemyCount)
    {
        Debug.Log($"🎮 StartRandomBattle called - Enemies: {enemyCount}, Active: {battleActive}");

        if (battleActive)
        {
            Debug.LogWarning("Battle already active!");
            return;
        }

        if (possibleEnemies == null || possibleEnemies.Length == 0)
        {
            Debug.LogError("❌ No enemies provided!");
            return;
        }

        if (playerBattlePrefab == null)
        {
            Debug.LogError("❌ Player battle prefab not assigned in BattleManager!");
            return;
        }

        if (battleUI == null)
        {
            Debug.LogError("❌ Battle UI not assigned in BattleManager!");
            return;
        }

        if (enemySpawnPositions == null || enemySpawnPositions.Length == 0)
        {
            Debug.LogError("❌ No enemy spawn positions assigned in BattleManager!");
            return;
        }

        Debug.Log($"✅ All validations passed, starting battle setup");

        battleActive = true;
        returnState = GameStateManager.Instance.GetCurrentState();
        StartCoroutine(SetupBattle(possibleEnemies, enemyCount));
    }

    public void StartHydraBattle(GameObject hydraPrefab, int hydraCount)
    {
        if (battleActive) return;

        battleActive = true;
        returnState = GameStateManager.GameState.Dungeon3;

        GameObject[] hydras = new GameObject[hydraCount];
        for (int i = 0; i < hydraCount; i++)
        {
            hydras[i] = hydraPrefab;
        }

        StartCoroutine(SetupBattle(hydras, hydraCount));
    }

    IEnumerator SetupBattle(GameObject[] possibleEnemies, int enemyCount)
    {
        Debug.Log($"🔧 SetupBattle starting - Will spawn {enemyCount} enemies from {possibleEnemies.Length} possible types");

        // Spawn player
        if (playerBattlePrefab != null && playerBattlePosition != null)
        {
            Debug.Log("👤 Spawning player...");
            playerBattleInstance = Instantiate(playerBattlePrefab, playerBattlePosition.position, Quaternion.identity);
            playerBattleInstance.transform.SetParent(transform);
            playerUnit = playerBattleInstance.GetComponent<BattleUnit>();

            if (playerUnit == null)
            {
                Debug.LogError("❌ Player battle instance has no BattleUnit component!");
            }
            else
            {
                Debug.Log($"✅ Player spawned - HP: {playerUnit.currentHealth}/{playerUnit.maxHealth}");
            }
        }

        // Spawn enemies
        for (int i = 0; i < enemyCount && i < enemySpawnPositions.Length; i++)
        {
            if (possibleEnemies.Length > 0)
            {
                GameObject enemyPrefab = possibleEnemies[Random.Range(0, possibleEnemies.Length)];

                if (enemyPrefab == null)
                {
                    Debug.LogError($"❌ Enemy prefab at index is null!");
                    continue;
                }

                Debug.Log($"👹 Spawning enemy {i + 1}: {enemyPrefab.name}");
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPositions[i].position, Quaternion.identity);
                enemy.transform.SetParent(transform);
                activeEnemies.Add(enemy);

                BattleUnit enemyUnit = enemy.GetComponent<BattleUnit>();
                if (enemyUnit == null)
                {
                    Debug.LogError($"❌ Enemy '{enemy.name}' has no BattleUnit component!");
                }
                else
                {
                    Debug.Log($"✅ Enemy spawned - {enemyUnit.unitName} HP: {enemyUnit.currentHealth}");
                }

                yield return new WaitForSeconds(0.3f);
            }
        }

        Debug.Log($"✅ Battle setup complete - {activeEnemies.Count} enemies spawned");

        // Show battle UI
        if (battleUI != null)
        {
            battleUI.SetActive(true);
            Debug.Log("✅ Battle UI enabled!");
        }
        else
        {
            Debug.LogError("❌ Battle UI is null!");
        }

        // Set first target
        if (activeEnemies.Count > 0)
        {
            currentEnemyTarget = activeEnemies[0].GetComponent<BattleUnit>();
            if (currentEnemyTarget != null)
            {
                Debug.Log($"🎯 Target set to: {currentEnemyTarget.unitName}");
            }
        }
        else
        {
            Debug.LogError("❌ No enemies spawned!");
        }

        UpdateUI();
        AddBattleLog("Battle Start!");
    }

    public void PlayerAttack()
    {
        if (!battleActive || playerUnit == null || currentEnemyTarget == null) return;

        int damage = playerUnit.Attack();
        currentEnemyTarget.TakeDamage(damage);

        AddBattleLog($"You dealt {damage} damage!");
        UpdateUI();

        if (currentEnemyTarget.IsDead())
        {
            StartCoroutine(OnEnemyDefeated());
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    public void PlayerDefend()
    {
        if (!battleActive || playerUnit == null) return;

        playerUnit.Defend();
        AddBattleLog("You brace for impact!");
        UpdateUI();

        StartCoroutine(EnemyTurn());
    }

    public void PlayerUseItem()
    {
        if (!battleActive || playerUnit == null) return;

        int healAmount = 30;
        playerUnit.Heal(healAmount);
        AddBattleLog($"You healed {healAmount} HP!");
        UpdateUI();

        StartCoroutine(EnemyTurn());
    }

    public void PlayerFlee()
    {
        float fleeChance = Random.Range(0f, 100f);

        if (fleeChance > 50f)
        {
            AddBattleLog("You escaped!");
            StartCoroutine(EndBattle(true, true));
        }
        else
        {
            AddBattleLog("Can't escape!");
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        foreach (GameObject enemyObj in activeEnemies)
        {
            BattleUnit enemy = enemyObj.GetComponent<BattleUnit>();
            if (enemy != null && !enemy.IsDead())
            {
                int damage = enemy.Attack();
                playerUnit.TakeDamage(damage);

                AddBattleLog($"{enemy.unitName} dealt {damage} damage!");
                UpdateUI();

                yield return new WaitForSeconds(0.5f);

                if (playerUnit.IsDead())
                {
                    StartCoroutine(OnPlayerDefeated());
                    yield break;
                }
            }
        }

        playerUnit.ResetDefense();
    }

    IEnumerator OnEnemyDefeated()
    {
        AddBattleLog($"{currentEnemyTarget.unitName} defeated!");

        activeEnemies.Remove(currentEnemyTarget.gameObject);
        Destroy(currentEnemyTarget.gameObject);

        yield return new WaitForSeconds(1f);

        if (activeEnemies.Count == 0)
        {
            AddBattleLog("Victory!");
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EndBattle(true, false));
        }
        else
        {
            currentEnemyTarget = activeEnemies[0].GetComponent<BattleUnit>();
            UpdateUI();
        }
    }

    IEnumerator OnPlayerDefeated()
    {
        AddBattleLog("You were defeated!");
        yield return new WaitForSeconds(2f);

        StartCoroutine(EndBattle(false, false));
    }

    IEnumerator EndBattle(bool playerWon, bool fled)
    {
        battleActive = false;

        if (playerBattleInstance != null)
        {
            Destroy(playerBattleInstance);
        }

        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }
        activeEnemies.Clear();

        if (battleUI != null)
        {
            battleUI.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        GameStateManager.Instance.SwitchState(returnState);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            RandomEncounterSystem encounterSystem = player.GetComponent<RandomEncounterSystem>();
            if (encounterSystem != null)
            {
                encounterSystem.OnBattleEnd();
            }
        }
    }

    void UpdateUI()
    {
        if (playerUnit != null && playerHealthText != null)
        {
            playerHealthText.text = $"HP: {playerUnit.currentHealth}/{playerUnit.maxHealth}";
        }

        if (currentEnemyTarget != null && enemyHealthText != null)
        {
            enemyHealthText.text = $"{currentEnemyTarget.unitName}\nHP: {currentEnemyTarget.currentHealth}/{currentEnemyTarget.maxHealth}";
        }
    }

    void AddBattleLog(string message)
    {
        if (battleLogText != null)
        {
            battleLogText.text = message + "\n" + battleLogText.text;

            string[] lines = battleLogText.text.Split('\n');
            if (lines.Length > 5)
            {
                battleLogText.text = string.Join("\n", lines, 0, 5);
            }
        }

        Debug.Log($"[Battle] {message}");
    }
}