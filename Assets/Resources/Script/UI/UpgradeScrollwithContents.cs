using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScrollwithContents : MonoBehaviour
{
    public RectTransform bgParent;  // ����� ���� �θ� (Viewport ����)
    public RectTransform contents;  // ScrollView�� contents
    public ScrollRect scrollRect;   // ScrollRect ������Ʈ
    public RectTransform bgPrefab;  // ��� ������
    public int poolSize = 5;        // ��� ������Ʈ Ǯ ũ��
    public float bgHeight = 1280f;  // ��� �� ���� ����
    public float visibilityOffset = 500f; // ȭ�� �ۿ��� ��Ȱ��ȭ�� �Ÿ�

    private readonly List<RectTransform> bgPool = new(); // ��� ������Ʈ Ǯ
    private float lastContentY;  // ���������� Ȯ���� ��ũ�� ��ġ

    void Start()
    {
        scrollRect.onValueChanged.AddListener(_ => OnScroll());
    }

    void OnScroll()
    {
        float contentY = contents.anchoredPosition.y;

        lastContentY = contentY;
    }
}
