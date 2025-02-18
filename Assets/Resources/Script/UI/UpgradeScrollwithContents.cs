using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScrollwithContents : MonoBehaviour
{
    public RectTransform bgParent;  // 배경을 담을 부모 (Viewport 하위)
    public RectTransform contents;  // ScrollView의 contents
    public ScrollRect scrollRect;   // ScrollRect 컴포넌트
    public RectTransform bgPrefab;  // 배경 프리팹
    public int poolSize = 5;        // 배경 오브젝트 풀 크기
    public float bgHeight = 1280f;  // 배경 한 장의 높이
    public float visibilityOffset = 500f; // 화면 밖에서 비활성화할 거리

    private readonly List<RectTransform> bgPool = new(); // 배경 오브젝트 풀
    private float lastContentY;  // 마지막으로 확인한 스크롤 위치

    void Start()
    {
        scrollRect.onValueChanged.AddListener(_ => OnScroll());

        // 배경 오브젝트 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            var bg = Instantiate(bgPrefab, bgParent);
            bg.anchoredPosition = new Vector2(0, i * bgHeight);
            bg.gameObject.SetActive(true);  // 처음에는 모두 활성화
            bgPool.Add(bg);
        }
    }

    void OnScroll()
    {
        float contentY = contents.anchoredPosition.y;

        // 배경 위치 갱신
        foreach (var bg in bgPool)
        {
            float bgY = bg.anchoredPosition.y;
            float distance = Mathf.Abs(contentY - bgY);

            // 유저의 시야에서 벗어난 배경을 비활성화
            bool isVisible = distance < bgHeight + visibilityOffset;
            bg.gameObject.SetActive(isVisible);
        }

        // 스크롤 방향에 따라 배경 재사용
        if (contentY > lastContentY) // 아래로 스크롤
        {
            RecycleBackground();
        }
        lastContentY = contentY;
    }

    void RecycleBackground()
    {
        RectTransform firstBg = bgPool[0];
        RectTransform lastBg = bgPool[^1];

        // 첫 번째 배경이 화면 위로 벗어났다면 재사용
        if (firstBg.anchoredPosition.y + bgHeight < contents.anchoredPosition.y - visibilityOffset)
        {
            firstBg.anchoredPosition = new Vector2(0, lastBg.anchoredPosition.y + bgHeight);
            bgPool.RemoveAt(0);
            bgPool.Add(firstBg);
        }
    }
}
