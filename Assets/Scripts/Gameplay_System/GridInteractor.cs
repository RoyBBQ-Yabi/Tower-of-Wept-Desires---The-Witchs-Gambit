using UnityEngine;

public class GridInteractor : MonoBehaviour
{
    private Camera mainCamera; // 快取相機，避免每幀數都做尋找增加耗能
    private Vector3 lastMousePosition; // 滑鼠最後接觸的位置
    private GameObject currentHoveredTile; //紀錄當前滑鼠指向的格子

    void Start() // 偵測機初始化
    {
        mainCamera = Camera.main; // 找到場景裡的 mainCamera
        lastMousePosition = Input.mousePosition; // 紀錄開始時滑鼠的位置
    }
    void Update()
    {
        if (Input.mousePosition != lastMousePosition) // 如果滑鼠位置有變化
        {
            lastMousePosition = Input.mousePosition;
            HandleHover();
        }

        if (Input.GetMouseButtonDown(0)) // 滑鼠點擊
        {
            HandleClick();
        }
    }

    private void SetHoverOverlay(GameObject tile, bool isActive) // 設定滑鼠指向時的特效開關
    {
        if (tile == null) // 沒到格子就不動作
        {
            return;
        }

        // 用 transform.Find() 來找當下物件的子物件
        // 在當下格子的子物件裡搜尋 HoverOverlay 
        Transform overlay = tile.transform.Find("HoverOverlay");

        if (overlay != null) // 有找到 HoverOverlay 就設定開關
        {
            overlay.gameObject.SetActive(isActive);
        }
    }
    private void HandleHover() // 滑鼠懸停邏輯
    {
        // 如果 GameStateManager 還沒初始化 or 非玩家回合就將格子特效關閉
        // 前者防止 Bug，後者非玩家回合格子不應該有滑鼠互動特效
        if (GameStateManager.Instance == null || GameStateManager.Instance.CurrentState != GameState.PlayerTurn)
        {
            SetHoverOverlay(currentHoveredTile, false);
            currentHoveredTile = null; // 清空指向的格子紀錄，防止格子殘留卡記憶體
            return;
        }
    }
    private void HandleClick() // 滑鼠點擊邏輯
    {

    }
}
