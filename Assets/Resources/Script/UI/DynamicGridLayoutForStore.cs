using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class DynamicGridLayoutForStore : DynamicGridLayout
{
    [SerializeField]
    [Tooltip("UI 이미지에 맞는 비율을 입력.")]
    public float aspectRatio = 1.0f / 2.0f;

    // 인벤토리의 그리드 레이 아웃의 셀크기(인벤토리 슬롯 크기)를 동적으로 할당해주는 클래스 및 메소드
    // 화면 크기에 맞추어 셀 사이즈,  셀 여백, 셀 spacing을 동적으로 할당해준다.
    public override void UpdateGridLayout()
    {
        // 화면의 너비 높이 가져오기
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;

        // 가로 기준으로 너비 설정
        cellWidth = screenWidth / (columnCount + (columnCount * 2) * paddingCoefficient + (columnCount - 1) * spacingCoefficient);
        cellHeight = cellWidth / aspectRatio;

        // 화면 높이를 초과하는 경우 높이에 맞춰 조정
        if (cellHeight * rowCount > screenHeight)
        {
            cellHeight = screenHeight / (rowCount + (rowCount * 2) * paddingCoefficient + (rowCount - 1) * spacingCoefficient);
            cellWidth = cellHeight * aspectRatio;
        }

        // 2. margin과 spacing 계산
        int padding = Mathf.CeilToInt(paddingCoefficient * cellWidth);
        float spacing = spacingCoefficient * cellWidth;

        layoutUpdate(cellWidth, cellHeight, padding, spacing);
    }

    // 최초 시작에만 해주면 되니까 굳이 해줄 필요가 없다.
    //private void Update()
    //{
    //    // Canvas 크기가 동적으로 변할 수 있는 경우 Update에서 계속 갱신
    //    UpdateGridLayout();
    //}
}
