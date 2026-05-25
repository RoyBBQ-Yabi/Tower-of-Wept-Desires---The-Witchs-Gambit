using UnityEngine;
using System.Collections.Generic; //引入集合工具箱幫助管理儲存的資料
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; } // Instance 單例入口 {任何人可讀; 只有自己能改;}

    [Header("地圖配置")] // 設立 Title 標籤
    [Tooltip("地圖大小")] // 用 Tooltip 功能實作介紹提示
    public int gridWidth = 5;
    public int gridHeight = 5;
    public GameObject tilePrefab_White;
    public GameObject tilePrefab_Dark;

    [Tooltip("格子的大小(含間距)")]
    public float tileSize = 1.2f;

    // 建立一個儲存格子和座標的 Dictionary 叫 gridTiles
    private Dictionary<Vector2Int, GameObject> gridTiles = new Dictionary<Vector2Int, GameObject>();

    private void Awake()
    {
        // 確保只會有一個 GridManager (變數名稱 Instance)
        // GridManager 必須最先生成所以用 Awake
        if (Instance != null && Instance != this)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[GridManager] 偵測到重複的GridManager {gameObject.name}，已自動刪除。");
#endif
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GenerateGrid() // 生成棋盤地圖
    {
        ClearGrid(); // 開始前清除先前的地圖

        // 計算格子距中心點移動的格數 ( 間距數 * 格子大小 / 2 = 中心點 )
        float offsetX = (gridWidth - 1) * tileSize / 2f;
        float offsetY = (gridHeight - 1) * tileSize / 2f;

        // 計算中心點與邊界左下角的距離，之後將生成好的地圖往左下移
        Vector3 centerOffset = new Vector3(-offsetX, -offsetY, 0);

        // 格子從第一列依序往上生成 (中心點在左下)
        // 生成邏輯 00,01,02,...,31,32,33
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // 格子座標 = 生成座標 + 往左下偏移量，確定保持中心點不變
                Vector3 localPos = new Vector3(x * tileSize, y * tileSize, 0) + centerOffset;

                // 將座標相加 / 2 判斷生成哪個格子，以達到西洋棋棋盤的效果
                // 條件 ? true ; false (if/else進階寫法)
                GameObject prefabToUse = (x + y) % 2 == 0 ? tilePrefab_White : tilePrefab_Dark;
                GameObject newTile = Instantiate(prefabToUse, transform);

                newTile.transform.localPosition = localPos;
                newTile.name = $"Tile_{x}_{y}";
                gridTiles.Add(new Vector2Int(x, y), newTile);
            }
        }
#if UNITY_EDITOR
        Debug.Log($"[GridManager] 已成功生成 {gridWidth}x{gridHeight} 網格。");
#endif
    }

    public void PlaceActorOnGrid(Transform actor, int x, int y)
    {
        // 將角色放置在指定位置，用 key 去尋找有無這個座標
        Vector2Int targetCoord = new Vector2Int(x, y);
        if (gridTiles.ContainsKey(targetCoord)) // 找 gridTiles 對應的 key
        {
            actor.position = gridTiles[targetCoord].transform.position;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"[GridManager] 找不到座標 ({x}, {y})，檢查是否超出範圍！");
#endif
        }
    }

    private void ClearGrid() // 清除棋盤地圖(進入新關卡會需要)
    {
        foreach (var tile in gridTiles.Values) // 檢查 gridTiles 的 Value 是否都清空
        {
            if (tile != null) Destroy(tile); // 先清物件
        }
        gridTiles.Clear(); // 再清棋盤地圖的資料庫
    }
}
