using UnityEngine;

public class BattleController : MonoBehaviour
{
    void Update()
    {
        // Press ESC to return to town map
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.SwitchState(GameStateManager.GameState.TownMap);
        }
    }
}