using UnityEngine;
using System.Collections.Generic; //引入集合工具箱幫助管理儲存的資料
public class GridManager : MonoBehaviour
{
    public static GridManager instance { get; private set; } // 設定誰能 {讀; 寫;}

    [Header("地圖配置")] // 設立 Title 標籤
    [Tooltip("地圖大小")] // 用 Tooltip 功能實作介紹提示
    public int gridWidth = 5;
    public int gridHeight = 5;
}
