using UnityEngine;

public class GridInteractor : MonoBehaviour
{
    private Camera mainCamera; // 快取相機，避免每幀數都做尋找增加耗能
    private Vector3 lastMousePosition; // 滑鼠最後接觸的位置
    private GameObject CurrentHoveredTile; //紀錄當前滑鼠指向的格子

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

    private void HandleHover() // 滑鼠懸停邏輯
    {

    }
    private void HandleClick() // 滑鼠點擊邏輯
    {

    }
}
