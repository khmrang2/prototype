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

        // ��� ������Ʈ Ǯ ����
        for (int i = 0; i < poolSize; i++)
        {
            var bg = Instantiate(bgPrefab, bgParent);
            bg.anchoredPosition = new Vector2(0, i * bgHeight);
            bg.gameObject.SetActive(true);  // ó������ ��� Ȱ��ȭ
            bgPool.Add(bg);
        }
    }

    void OnScroll()
    {
        float contentY = contents.anchoredPosition.y;

        // ��� ��ġ ����
        foreach (var bg in bgPool)
        {
            float bgY = bg.anchoredPosition.y;
            float distance = Mathf.Abs(contentY - bgY);

            // ������ �þ߿��� ��� ����� ��Ȱ��ȭ
            bool isVisible = distance < bgHeight + visibilityOffset;
            bg.gameObject.SetActive(isVisible);
        }

        // ��ũ�� ���⿡ ���� ��� ����
        if (contentY > lastContentY) // �Ʒ��� ��ũ��
        {
            RecycleBackground();
        }
        lastContentY = contentY;
    }

    void RecycleBackground()
    {
        RectTransform firstBg = bgPool[0];
        RectTransform lastBg = bgPool[^1];

        // ù ��° ����� ȭ�� ���� ����ٸ� ����
        if (firstBg.anchoredPosition.y + bgHeight < contents.anchoredPosition.y - visibilityOffset)
        {
            firstBg.anchoredPosition = new Vector2(0, lastBg.anchoredPosition.y + bgHeight);
            bgPool.RemoveAt(0);
            bgPool.Add(firstBg);
        }
    }
}
