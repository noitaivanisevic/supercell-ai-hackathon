using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public enum GameState
    {
        TownMap,
        Town1,
        Town2,
        Town3,
        DungeonMap,
        Dungeon1,
        Dungeon2,
        Dungeon3,
        Battle
    }

    [Header("State Objects")]
    public GameObject townMapObjects;
    public GameObject town1Objects;
    public GameObject town2Objects;
    public GameObject town3Objects;
    public GameObject dungeonMapObjects;
    public GameObject dungeon1Objects;
    public GameObject dungeon2Objects;
    public GameObject dungeon3Objects;
    public GameObject battleObjects;

    [Header("Backgrounds")]
    public SpriteRenderer backgroundRenderer;
    public Sprite townMapBackground;
    public Sprite town1Background;
    public Sprite town2Background;
    public Sprite town3Background;
    public Sprite dungeonMapBackground;
    public Sprite dungeon1Background;
    public Sprite dungeon2Background;
    public Sprite dungeon3Background;
    public Sprite battleBackground;

    [Header("Camera")]
    public Camera mainCamera;
    public Vector3 townMapCameraPos = new Vector3(0, 0, -10);
    public Vector3 town1CameraPos = new Vector3(0, 0, -10);
    public Vector3 town2CameraPos = new Vector3(0, 0, -10);
    public Vector3 town3CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeonMapCameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon1CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon2CameraPos = new Vector3(0, 0, -10);
    public Vector3 dungeon3CameraPos = new Vector3(0, 0, -10);
    public Vector3 battleCameraPos = new Vector3(0, 0, -10);

    private GameState currentState;

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
        SwitchState(GameState.TownMap);
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;

        // Disable all states
        if (townMapObjects != null) townMapObjects.SetActive(false);
        if (town1Objects != null) town1Objects.SetActive(false);
        if (town2Objects != null) town2Objects.SetActive(false);
        if (town3Objects != null) town3Objects.SetActive(false);
        if (dungeonMapObjects != null) dungeonMapObjects.SetActive(false);
        if (dungeon1Objects != null) dungeon1Objects.SetActive(false);
        if (dungeon2Objects != null) dungeon2Objects.SetActive(false);
        if (dungeon3Objects != null) dungeon3Objects.SetActive(false);
        if (battleObjects != null) battleObjects.SetActive(false);

        // Enable and setup current state
        switch (currentState)
        {
            case GameState.TownMap:
                if (townMapObjects != null) townMapObjects.SetActive(true);
                if (backgroundRenderer != null && townMapBackground != null)
                    backgroundRenderer.sprite = townMapBackground;
                if (mainCamera != null)
                    mainCamera.transform.position = townMapCameraPos;
                break;

            case GameState.Town1:
                if (town1Objects != null) town1Objects.SetActive(true);
                if (backgroundRenderer != null && town1Background != null)
                    backgroundRenderer.sprite = town1Background;
                if (mainCamera != null)
                    mainCamera.transform.position = town1CameraPos;
                break;

            case GameState.Town2:
                if (town2Objects != null) town2Objects.SetActive(true);
                if (backgroundRenderer != null && town2Background != null)
                    backgroundRenderer.sprite = town2Background;
                if (mainCamera != null)
                    mainCamera.transform.position = town2CameraPos;
                break;

            case GameState.Town3:
                if (town3Objects != null) town3Objects.SetActive(true);
                if (backgroundRenderer != null && town3Background != null)
                    backgroundRenderer.sprite = town3Background;
                if (mainCamera != null)
                    mainCamera.transform.position = town3CameraPos;
                break;

            case GameState.DungeonMap:
                if (dungeonMapObjects != null) dungeonMapObjects.SetActive(true);
                if (backgroundRenderer != null && dungeonMapBackground != null)
                    backgroundRenderer.sprite = dungeonMapBackground;
                if (mainCamera != null)
                    mainCamera.transform.position = dungeonMapCameraPos;
                break;

            case GameState.Dungeon1:
                if (dungeon1Objects != null) dungeon1Objects.SetActive(true);
                if (backgroundRenderer != null && dungeon1Background != null)
                    backgroundRenderer.sprite = dungeon1Background;
                if (mainCamera != null)
                    mainCamera.transform.position = dungeon1CameraPos;
                break;

            case GameState.Dungeon2:
                if (dungeon2Objects != null) dungeon2Objects.SetActive(true);
                if (backgroundRenderer != null && dungeon2Background != null)
                    backgroundRenderer.sprite = dungeon2Background;
                if (mainCamera != null)
                    mainCamera.transform.position = dungeon2CameraPos;
                break;

            case GameState.Dungeon3:
                if (dungeon3Objects != null) dungeon3Objects.SetActive(true);
                if (backgroundRenderer != null && dungeon3Background != null)
                    backgroundRenderer.sprite = dungeon3Background;
                if (mainCamera != null)
                    mainCamera.transform.position = dungeon3CameraPos;
                break;

            case GameState.Battle:
                if (battleObjects != null) battleObjects.SetActive(true);
                if (backgroundRenderer != null && battleBackground != null)
                    backgroundRenderer.sprite = battleBackground;
                if (mainCamera != null)
                    mainCamera.transform.position = battleCameraPos;
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}