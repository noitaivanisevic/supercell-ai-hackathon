using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform townMapPlayer;
    public Transform town1Player;
    public Transform town2Player;
    public Transform town3Player;
    public Transform dungeonMapPlayer;
    public Transform dungeon1Player;
    public Transform dungeon2Player;
    public Transform dungeon3Player;

    public float smoothSpeed = 0.125f;
    public Vector2 minBounds = new Vector2(-20, -15);
    public Vector2 maxBounds = new Vector2(20, 15);

    void LateUpdate()
    {
        Transform currentPlayer = null;
        GameStateManager.GameState state = GameStateManager.Instance.GetCurrentState();

        switch (state)
        {
            case GameStateManager.GameState.TownMap:
                currentPlayer = townMapPlayer;
                break;
            case GameStateManager.GameState.Town1:
                currentPlayer = town1Player;
                break;
            case GameStateManager.GameState.Town2:
                currentPlayer = town2Player;
                break;
            case GameStateManager.GameState.Town3:
                currentPlayer = town3Player;
                break;
            case GameStateManager.GameState.DungeonMap:
                currentPlayer = dungeonMapPlayer;
                break;
            case GameStateManager.GameState.Dungeon1:
                currentPlayer = dungeon1Player;
                break;
            case GameStateManager.GameState.Dungeon2:
                currentPlayer = dungeon2Player;
                break;
            case GameStateManager.GameState.Dungeon3:
                currentPlayer = dungeon3Player;
                break;
            default:
                return;
        }

        if (currentPlayer == null) return;

        Vector3 targetPosition = new Vector3(currentPlayer.position.x, currentPlayer.position.y, transform.position.z);
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}