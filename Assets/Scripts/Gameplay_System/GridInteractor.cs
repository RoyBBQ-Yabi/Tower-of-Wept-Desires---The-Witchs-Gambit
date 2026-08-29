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
        if (Input.mousePosition != lastMousePosition) // 如果滑鼠位置有變化跳到 HandleHover() 去
        {
            lastMousePosition = Input.mousePosition;
            HandleHover();
        }

        if (Input.GetMouseButtonDown(0)) // 滑鼠點擊跳到 HandleClick() 去
        {
            HandleClick();
        }
    }
    private RaycastHit2D GetMouseRaycast()  // 取得滑鼠位置的射線結果
    {
        // 將滑鼠指向的座標轉換成【遊戲世界】的相對座標
        //將相機看相的視角的滑鼠指向位置轉換成世界的相對位置

        /* 用意：沒轉換的時候抓螢幕位置左下為 0,0 但無法去定義右上的位置
           因為，如果你假設玩家螢幕為 192*1080 右上就是這個，
           但如果有玩家用 2K 螢幕那在他的畫面右上就是 2580*1440。*/
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // RaycastHit2D 儲存射線，Physics2D.Raycast 發射射線偵測(滑鼠座標, 偵測點)
        // Vector2.zero 不給方向，射線不會往前只偵測滑鼠碰到的位置
        return Physics2D.Raycast(mouseWorldPos, Vector2.zero);
    }
    private bool IsPlayerTurn() // 檢查 GameStateManager 是否以初始化 && 為玩家回合
    {
        return GameStateManager.Instance != null &&
               GameStateManager.Instance.CurrentState == GameState.PlayerTurn;
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
        if (!IsPlayerTurn()) // 如果不是該狀態將特效關閉，格子儲存內容設為 null
        {
            SetHoverOverlay(currentHoveredTile, false);
            currentHoveredTile = null;
            return;
        }
        // 獲取滑鼠射線
        RaycastHit2D hit = GetMouseRaycast();

        if (hit.collider != null && hit.collider.CompareTag("Tile")) // 如果射線有打到東西 && 是格子
        {
            GameObject hitTile = hit.collider.gameObject; // 將接觸到的格子存入 hitTile

            // 如果格子與先前的不同將特效關閉換新的再開啟
            if (hitTile != currentHoveredTile)
            {
                SetHoverOverlay(currentHoveredTile, false);
                currentHoveredTile = hitTile;
                SetHoverOverlay(currentHoveredTile, true);
            }
        }
        else // 沒打到東西將特效關閉，當前格子設為 null
        {
            SetHoverOverlay(currentHoveredTile, false);
            currentHoveredTile = null;
        }
    }
    private void HandleClick() // 滑鼠點擊邏輯
    {
        if (!IsPlayerTurn()) return; // 若沒有初始化、玩家回合跳出

        RaycastHit2D hit = GetMouseRaycast(); // 取得滑鼠射線

        if (hit.collider == null) return; // 若沒有打到東西
        if (!hit.collider.CompareTag("Tile")) return; // 如果打到的不是格子

        // 將打到的東西去 TileData 比對資料
        TileData tileData = hit.collider.GetComponent<TileData>();
        if (tileData == null) return; // 如果 TileData 裡面沒有對應資料跳出

#if UNITY_EDITOR
        Debug.Log($"[GridInteractor] 點擊座標: ({tileData.x}, {tileData.y})");
#endif
    }
}