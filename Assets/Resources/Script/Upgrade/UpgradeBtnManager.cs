using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] UpgradeButtonsList;
    [SerializeField] private ScrollRect scrollRect;

    public void RefreshUpgradeBtn()
    {
        foreach (GameObject btn in UpgradeButtonsList)
        {
            UpgradeBtn upgradeBtn = btn.GetComponent<UpgradeBtn>();
            upgradeBtn.CheckUpgradable();
        }

        MoveToFirstUpgradable();
    }

    private void MoveToFirstUpgradable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        foreach (GameObject btn in UpgradeButtonsList)
        {
            UpgradeBtn upgradeBtn = btn.GetComponent<UpgradeBtn>();

            if (upgradeBtn.isUpgradable)
            {
                RectTransform targetRect = btn.GetComponent<RectTransform>();
                StartCoroutine(SmoothScrollToCenter(targetRect));
                return;
            }
        }

        Debug.Log("MoveToFirstUpgradable: 업그레이드 가능한 버튼이 없습니다.");
    }

    private IEnumerator SmoothScrollToCenter(RectTransform target)
    {
        yield return null; // 프레임 대기: UI 업데이트 이후 계산 정확성을 위해

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = scrollRect.content;
        float contentHeight = contentRect.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        // 타겟 중심을 기준으로 계산
        float targetCenterY = -target.anchoredPosition.y + target.rect.height * 0.5f;
        float desiredScrollY = targetCenterY - viewportHeight * 0.5f;

        float normalizedTargetPos = desiredScrollY / (contentHeight - viewportHeight);
        normalizedTargetPos = Mathf.Clamp01(normalizedTargetPos); // 스크롤 방향 반전

        // 너무 가까우면 스크롤하지 않음
        if (Mathf.Abs(scrollRect.verticalNormalizedPosition - normalizedTargetPos) < 0.01f)
            yield break;

        // 부드러운 스크롤
        float duration = 0.8f;
        float elapsedTime = 0f;
        float startValue = scrollRect.verticalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // 부드러운 가속

            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startValue, normalizedTargetPos, t);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = normalizedTargetPos;
    }
}
