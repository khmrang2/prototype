using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUpgradeGridLayout : DynamicGridLayout
{
    public RectTransform progressBar;
    public RectTransform contents;

    /// <summary>
    /// override.
    /// </summary>
    public override void UpdateGridLayout()
    {
        // 부모 클래스의 UpdateGridLayout 실행
        base.UpdateGridLayout();

        // 부모 클래스에서 계산된 cellSize로 추가 작업 수행
        additionalUpdate();
    }

    /// <summary>
    /// progressBar와 contens를 크기변환된 cellsize에 맞추어 height를 변경한다.
    /// </summary>
    private void additionalUpdate()
    {
        if (progressBar != null)
        {
            // progressBar의 높이를 cellSize의 개수만큼 설정.
            progressBar.sizeDelta = new Vector2(progressBar.sizeDelta.x, (1 + spacingCoefficient) * cellHeight * (16 + 1));
        }

        if (contents != null)
        {
            // contents의 높이를 cellSize와 동일하게 설정
            contents.sizeDelta = new Vector2(contents.sizeDelta.x, (1 + spacingCoefficient) * cellHeight * (16 + 1));
        }

        //Debug.Log("업그레이드된 레이아웃 추가 작업 실행!");
    }
}
