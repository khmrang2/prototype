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
    public float forcedSnapDuration = 0.2f;

    private float[] pagePositions;      // 콘텐츠 위치
    private int pageCount;              // 콘텐츠 개수(스테이지 개수)
    private int targetPage = 0;         // 지금 현재 보여주고 있는 스테이지
    private bool isDragging = false;    // 지금 사용자 입력이 들어오고 있는지
    
    private int dragStartPage;          // 드래그 시작 시 페이지
    private bool isMovingToSafePage = false; // 안전한 페이지로 이동 중인지 여부

    public event System.Action<int> OnStageSelected; // 선택된 스테이지 인덱스 알림 이벤트
    
    // 스테이지의 정보들을 가져와서 변수로 저장.
    void Start()
    {
        InitializePagePositions();
    }

    // 콘텐츠가 동적으로 변경될 때 호출하는 메소드
    public void UpdateContent()
    {
        InitializePagePositions();
        // 현재 페이지가 유효한지 확인하고 조정
        if (targetPage >= pageCount)
        {
            targetPage = Mathf.Max(0, pageCount - 1);
        }
    }

    // 페이지 위치 초기화 로직을 분리한 메소드
    private void InitializePagePositions()
    {
        pageCount = content.childCount;
        pagePositions = new float[pageCount];

        if (pageCount > 1)
        {
            float step = 1f / (pageCount - 1);
            for (int i = 0; i < pageCount; i++)
            {
                pagePositions[i] = step * i;
            }
        }
        else if (pageCount == 1)
        {
            pagePositions[0] = 0f;
        }
    }

    // 사용자 입력
    // 슬라이드를 하다가 놓으면
    // 가장 가까운 콘텐츠의 위치를 Lerp를 이용해서 천천히 이동.
    // 변수 : snapSpeed로 콘텐츠로 이동하는 속도 조절.
    void Update()
    {
        if (!isDragging && pageCount > 0)
        {
            // 잠긴 스테이지로 이동하지 않도록 확인
            if (!isMovingToSafePage && !stageManager.IsStageUnlocked(targetPage))
            {
                MoveToSafePage();
            }
            
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
        dragStartPage = targetPage; // 드래그 시작 페이지 저장
    }

    // 드래그가 끝났는가? - 가장 가까운 콘텐츠의 위치를 저장.
    public void OnEndDrag()
    {
        isDragging = false;

        if (pageCount == 0) return;

        // 가장 가까운 페이지 찾기
        int closestPage = FindClosestPage();
        
        // 선택된 페이지가 잠겨있는 경우
        if (!stageManager.IsStageUnlocked(closestPage))
        {
            HandleLockedPageSelection(closestPage);
        }
        else
        {
            targetPage = closestPage;
            OnStageSelected?.Invoke(targetPage);
        }
    }
    
    // 가장 가까운 페이지 찾기
    private int FindClosestPage()
    {
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
        
        return closestPage;
    }
    
    // 잠긴 페이지 선택 처리
    private void HandleLockedPageSelection(int lockedPage)
    {
        // 가장 가까운 열린 스테이지 찾기
        bool foundUnlockedStage = false;
        
        // 이전 스테이지 중에서 찾기
        for (int i = lockedPage - 1; i >= 0; i--)
        {
            if (stageManager.IsStageUnlocked(i))
            {
                targetPage = i;
                foundUnlockedStage = true;
                StartCoroutine(ForceScrollToPage(targetPage));
                break;
            }
        }
        
        // 이전 스테이지에서 찾지 못했다면 원래 위치로 돌아가기
        if (!foundUnlockedStage)
        {
            targetPage = dragStartPage;
            StartCoroutine(ForceScrollToPage(targetPage));
        }
        
        OnStageSelected?.Invoke(targetPage);
    }
    
    // 안전한 페이지로 이동
    private void MoveToSafePage()
    {
        isMovingToSafePage = true;
        
        // 마지막으로 잠금 해제된 스테이지 찾기
        bool foundUnlockedStage = false;
        for (int i = targetPage - 1; i >= 0; i--)
        {
            if (stageManager.IsStageUnlocked(i))
            {
                targetPage = i;
                OnStageSelected?.Invoke(targetPage);
                foundUnlockedStage = true;
                break;
            }
        }
        
        // 이전 스테이지에서 찾지 못했다면 첫 번째 스테이지로 이동
        if (!foundUnlockedStage && pageCount > 0)
        {
            targetPage = 0;
            OnStageSelected?.Invoke(targetPage);
        }
        
        isMovingToSafePage = false;
    }
    
    // 강제로 특정 페이지로 스크롤
    private IEnumerator ForceScrollToPage(int page)
    {
        float elapsedTime = 0;
        float startPos = scrollRect.verticalNormalizedPosition;
        float targetPos = pagePositions[page];
        
        while (elapsedTime < forcedSnapDuration)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPos, targetPos, elapsedTime / forcedSnapDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        scrollRect.verticalNormalizedPosition = targetPos;
    }

    // 특정 페이지로 직접 이동하는 메소드
    public void SetPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pageCount)
        {
            Debug.LogWarning($"⚠️ 유효하지 않은 페이지 인덱스: {pageIndex}. 범위는 0-{pageCount-1}입니다.");
            return;
        }

        // 잠긴 스테이지인지 확인
        if (!stageManager.IsStageUnlocked(pageIndex))
        {
            // 마지막으로 잠금 해제된 스테이지 찾기
            for (int i = pageIndex - 1; i >= 0; i--)
            {
                if (stageManager.IsStageUnlocked(i))
                {
                    targetPage = i;
                    OnStageSelected?.Invoke(targetPage);
                    return;
                }
            }
        }
        else
        {
            targetPage = pageIndex;
            OnStageSelected?.Invoke(targetPage);
        }
    }
}
