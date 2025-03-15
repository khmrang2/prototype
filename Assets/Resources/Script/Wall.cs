using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("벽")]
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;

    [Header("밑에 공 사라지게 하는 오브젝트")]
    public BoxCollider2D destroyArea;

    private Camera mainCamera;
    private int lastScreenWidth, lastScreenHeight;

    /// <summary>
    /// 최초 실행 시에 화면 비율에 맞춰 벽의 위치와 크기를 설정합니다.
    /// </summary>
    void Awake()
    {
        mainCamera = Camera.main;
        UpdateObjectPositions();
    }

    /// <summary>
    /// 매 프레임마다 화면 크기가 변경되었는지 체크하여, 변경 시 UpdateWallPositions() 함수를 호출합니다.
    /// </summary>
    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateObjectPositions();
        }
    }

    void UpdateObjectPositions()
    {
        UpdateWallPositions();
        UpdateDestroyAreaPosition();
    }

    /// <summary>
    /// 화면 비율에 맞춰서 벽의 위치와 크기를 변경합니다.
    /// - 벽의 높이는 전체 화면 높이의 60%가 됩니다.
    /// - 벽의 중심은 스크린 하단에서 25% 지점에 위치합니다.
    /// </summary>
    void UpdateWallPositions()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        Vector2 screenBottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector2 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        float wallWidth = 0.5f; // 벽의 두께
        // 전체 화면 높이의 60%를 벽의 높이로 사용
        float wallHeight = (screenTopRight.y - screenBottomLeft.y) * 0.8f;
        // 스크린 하단에서 25% 지점을 벽의 y축 중심으로 설정
        float wallCenterY = screenBottomLeft.y + (screenTopRight.y - screenBottomLeft.y) * 0.40f;

        // 왼쪽 벽 설정 (x축은 벽의 두께 절반만큼 오프셋)
        leftWall.size = new Vector2(wallWidth, wallHeight);
        leftWall.transform.position = new Vector2(screenBottomLeft.x + wallWidth / 3, wallCenterY);

        // 오른쪽 벽 설정 (x축은 벽의 두께 절반만큼 오프셋)
        rightWall.size = new Vector2(wallWidth, wallHeight);
        rightWall.transform.position = new Vector2(screenTopRight.x - wallWidth / 3, wallCenterY);
    }

    void UpdateDestroyAreaPosition()
    {
        // 메인 카메라를 기준으로 화면 하단의 좌표를 가져옴
        Vector3 bottomCenterScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));

        // Destroy Zone의 위치를 카메라 하단부로 설정
        destroyArea.transform.position = new Vector3(bottomCenterScreen.x, bottomCenterScreen.y - 1.0f, 0); // Y좌표를 살짝 낮춰 카메라에 보이지 않도록 설정

        destroyArea.size = new Vector2(Camera.main.orthographicSize * 3 * Camera.main.aspect, 1); // 화면 너비에 맞춤
    }
}