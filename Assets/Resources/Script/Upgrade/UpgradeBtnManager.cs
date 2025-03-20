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

            if (upgradeBtn.isUpgradable) // 업그레이드 가능한 버튼 찾기
            {
                RectTransform targetRect = btn.GetComponent<RectTransform>();

                StartCoroutine(SmoothScrollTo(targetRect));
                return;
            }
        }

        Debug.Log("MoveToFirstUpgradable: 업그레이드 가능한 버튼이 없습니다.");
    }


    private IEnumerator SmoothScrollTo(RectTransform target)
    {
        yield return null;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect = scrollRect.content;
        float contentHeight = contentRect.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float targetY = target.anchoredPosition.y;
        float clampedY = Mathf.Clamp(-targetY, 0, contentHeight - viewportHeight);
        float normalizedTargetPos = clampedY * 0.9f / (contentHeight - viewportHeight);
        normalizedTargetPos = Mathf.Clamp(normalizedTargetPos, 0.1f, 0.95f);
        
        if (Mathf.Abs(scrollRect.verticalNormalizedPosition - normalizedTargetPos) < 0.01f)
        {
            yield break;
        }
        
        float duration = 0.8f;
        float elapsedTime = 0f;
        float startValue = scrollRect.verticalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / duration;
            t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);

            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startValue, normalizedTargetPos, t);
            yield return null;
        }
        
        scrollRect.verticalNormalizedPosition = normalizedTargetPos;
    }
}
