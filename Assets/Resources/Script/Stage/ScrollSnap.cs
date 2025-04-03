using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    // 스크롤을 중간을 보여주는게 아니라,
    // 딱딱 콘텐츠(스테이지)만 보여주게 만드는 코드
    public StageManager stageManager;

    public ScrollRect scrollRect;       // 스크롤 rect
    public RectTransform content;       // 스크롤 rect - 콘텐츠
    public float snapSpeed = 10f;       // 

    private float[] pagePositions;      // 콘텐츠 위치
    private int pageCount;              // 콘텐츠 개수(스테이지 개수)
    private int targetPage = 0;         // 지금 현재 보여주고 있는 스테이지
    private bool isDragging = false;    // 지금 사용자 입력이 들어오고 있는지

    public event System.Action<int> OnStageSelected; // 선택된 스테이지 인덱스 알림 이벤트
    // 스테이지의 정보들을 가져와서 변수로 저장.
    void Start()
    {
        pageCount = content.childCount;
        pagePositions = new float[pageCount];

        float step = 1f / (pageCount - 1);
        for (int i = 0; i < pageCount; i++)
        {
            pagePositions[i] = step * i;
        }
    }

    // 사용자 입력
    // 슬라이드를 하다가 놓으면
    // 가장 가까운 콘텐츠의 위치를 Lerp를 이용해서 천천히 이동.
    // 변수 : snapSpeed로 콘텐츠로 이동하는 속도 조절.
    void Update()
    {
        if (!isDragging)
        {
            float targetPos = pagePositions[targetPage];
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                targetPos,
                Time.deltaTime * snapSpeed
            );
        }
    }

    // 드래그 중인가? - scrollrect의 이벤트 직접 할당
    public void OnBeginDrag()
    {
        isDragging = true;
    }

    // 드래그가 끝났는가? - 가장 가까운 콘텐츠의 위치를 저장.
    public void OnEndDrag()
    {
        isDragging = false;

        float closest = float.MaxValue;
        int closestPage = 0;

        for (int i = 0; i < pageCount; i++)
        {
            float dist = Mathf.Abs(scrollRect.verticalNormalizedPosition - pagePositions[i]);
            if (dist < closest)
            {
                closest = dist;
                closestPage = i;
            }
        }

        // 선택된 페이지가 잠겨있는 경우
        if (!stageManager.IsStageUnlocked(closestPage + 1)) // StageManager는 1부터
        {
            //Debug.Log($"⛔ 스테이지 {closestPage + 1}은 잠겨있음. 가장 가까운 열린 스테이지로 이동");

            // 가장 가까운 열린 스테이지 찾기 (1부터 비교)
            for (int i = closestPage; i >= 0; i--)
            {
                if (stageManager.IsStageUnlocked(i + 1)) // 여기서도 +1
                {
                    targetPage = i;
                    break;
                }
            }
        }
        else
        {
            targetPage = closestPage;
        }

        //Debug.Log($"📌 최종 보여줄 스테이지: {targetPage}");
        // ⭐ 여기서 이벤트 호출
        OnStageSelected?.Invoke(targetPage);
    }

    public int getCurStageUI()
    {
        return targetPage + 1; // index는 0부터 시작이니까용?
    }
}
