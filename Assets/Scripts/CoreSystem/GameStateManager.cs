using UnityEngine;
public enum GameState // 定義遊戲可能出現的所有狀態
{
    Initialize,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; } // Instance 單例入口 {任何人可讀; 只有自己能改;}
    [Header("角色引用 (過渡期代碼，3.1階段將移除)")]
    public Transform player;
    public Transform witch;
    public Transform enemy;

    [Header("當前遊戲狀態")]
    [SerializeField] private GameState currentState; // 當前遊戲狀態
    public GameState CurrentState => currentState; // 其他類別可讀取但無法修改

    private void Awake() // 單例檢查
    {
        if (Instance != null && Instance != this)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[GameStateManager] 偵測到重複的GameStateManager {gameObject.name}，已自動刪除。");
#endif
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Initialize); // 遊戲開始時進入初始化狀態
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState; // 更新當前狀態
#if UNITY_EDITOR
        Debug.Log($"[GameStateManager] 狀態切換至: {currentState}");
#endif
        switch (currentState)
        {
            case GameState.Initialize:
                HandleInitialize(); // 處理初始化
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void HandleInitialize()
    {
        if (GridManager.Instance != null)
        {
            GridManager.Instance.GenerateGrid(); // 生成棋盤

            // 3.1階段過渡期代碼，3.1階段將移除
            GridManager.Instance.PlaceActorOnGrid(player, 0, 0);
            GridManager.Instance.PlaceActorOnGrid(witch, 7, 7);
            GridManager.Instance.PlaceActorOnGrid(enemy, 5, 4);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("[GameStateManager] 找不到 GridManager，無法初始化地圖！");
#endif
            return;
        }
        ChangeState(GameState.PlayerTurn); // 初始化完成後切換到玩家回合
    }
}