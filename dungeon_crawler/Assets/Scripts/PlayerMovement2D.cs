using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool horizontalOnly = false; // Set to true for town/dungeon players

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if GameStateManager exists
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("GameStateManager not found!");
            return;
        }

        // Only move if in an exploration state
        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        if (state != GameStateManager.GameState.TownMap &&
            state != GameStateManager.GameState.Town1 &&
            state != GameStateManager.GameState.Town2 &&
            state != GameStateManager.GameState.Town3 &&
            state != GameStateManager.GameState.DungeonMap &&
            state != GameStateManager.GameState.Dungeon1 &&
            state != GameStateManager.GameState.Dungeon2 &&
            state != GameStateManager.GameState.Dungeon3)
        {
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");

        // Only allow vertical movement if not horizontalOnly
        if (!horizontalOnly)
        {
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement.y = 0; // Lock vertical movement
        }
    }

    void FixedUpdate()
    {
        // Check if GameStateManager exists
        if (GameStateManager.Instance == null) return;

        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        if (state == GameStateManager.GameState.TownMap ||
            state == GameStateManager.GameState.Town1 ||
            state == GameStateManager.GameState.Town2 ||
            state == GameStateManager.GameState.Town3 ||
            state == GameStateManager.GameState.DungeonMap ||
            state == GameStateManager.GameState.Dungeon1 ||
            state == GameStateManager.GameState.Dungeon2 ||
            state == GameStateManager.GameState.Dungeon3)
        {
            if (rb != null)
            {
                rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}