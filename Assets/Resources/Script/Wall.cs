using UnityEngine;

public class Wall : MonoBehaviour
{
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;

    private Camera mainCamera;
    private int lastScreenWidth, lastScreenHeight;

    /// <summary>
    /// ���� ���� �ÿ� ȭ�� ������ ���� ���� ��ġ�� ũ�⸦ �����մϴ�.
    /// </summary>
    void Awake()
    {
        mainCamera = Camera.main;
        UpdateWallPositions();
    }

    /// <summary>
    /// �� �����Ӹ��� ȭ�� ũ�Ⱑ ����Ǿ����� üũ�Ͽ�, ���� �� UpdateWallPositions() �Լ��� ȣ���մϴ�.
    /// </summary>
    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateWallPositions();
        }
    }

    /// <summary>
    /// ȭ�� ������ ���缭 ���� ��ġ�� ũ�⸦ �����մϴ�.
    /// - ���� ���̴� ��ü ȭ�� ������ 60%�� �˴ϴ�.
    /// - ���� �߽��� ��ũ�� �ϴܿ��� 25% ������ ��ġ�մϴ�.
    /// </summary>
    void UpdateWallPositions()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        Vector2 screenBottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector2 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        float wallWidth = 0.5f; // ���� �β�
        // ��ü ȭ�� ������ 60%�� ���� ���̷� ���
        float wallHeight = (screenTopRight.y - screenBottomLeft.y) * 0.8f;
        // ��ũ�� �ϴܿ��� 25% ������ ���� y�� �߽����� ����
        float wallCenterY = screenBottomLeft.y + (screenTopRight.y - screenBottomLeft.y) * 0.40f;

        // ���� �� ���� (x���� ���� �β� ���ݸ�ŭ ������)
        leftWall.size = new Vector2(wallWidth, wallHeight);
        leftWall.transform.position = new Vector2(screenBottomLeft.x + wallWidth / 3, wallCenterY);

        // ������ �� ���� (x���� ���� �β� ���ݸ�ŭ ������)
        rightWall.size = new Vector2(wallWidth, wallHeight);
        rightWall.transform.position = new Vector2(screenTopRight.x - wallWidth / 3, wallCenterY);
    }
}
