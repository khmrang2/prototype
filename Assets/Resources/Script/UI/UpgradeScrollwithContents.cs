using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScrollwithContents : MonoBehaviour
{
    public RectTransform bgParent;  // viewport 배경
    public RectTransform contents;  // ScrollView의 contents
    public ScrollRect scrollRect;   // ScrollRect 컴포넌트
    public RectTransform bgPrefab;  // 생성할 배경 프리팹
    private float lastContentY;  // 마지막 콘텐츠

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
