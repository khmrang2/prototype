using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // 조절하고 싶은 동적 Grid Layout Group을 연결
    public RectTransform parentRectTransform; // 스크린 오브젝트(캔버스)
    
    private float parentWidth; // 내부적으로 가지고 있을 스크린 크기\
    [SerializeField]
    private float marginOffset = 10.0f; // 여백과 spacing을 주기위한 상수 offset(셀 사이즈의 10%)

    private void Start()
    {
        UpdateGridLayout();
    }


    // 인벤토리의 그리드 레이 아웃의 셀크기(인벤토리 슬롯 크기)를 동적으로 할당해주는 클래스 및 메소드
    // 화면 크기에 맞추어 셀 사이즈,  셀 여백, 셀 spacing을 동적으로 할당해준다.
    private void UpdateGridLayout()
    {
        // 부모의 너비 가져오기
        float parentWidth = parentRectTransform.rect.width;

        // 열 개수 고정 (4개)
        int columnCount = 4;

        // 셀 간의 간격 (간격 비율을 부모 크기에 따라 조정)
        float spacing = parentWidth * 0.02f; // 2% 간격

        // 패딩 계산 (셀 크기의 10%로 설정)
        float padding = parentWidth * 0.05f; // 5% 패딩
        int paddingInt = Mathf.RoundToInt(padding); // 정수로 변환

        // 셀 크기 계산: 패딩과 간격을 포함하여 너비를 나눔
        float totalSpacing = spacing * (columnCount - 1); // 모든 간격의 합
        float totalPadding = padding * 2; // 좌우 패딩 합
        float cellWidth = (parentWidth - totalSpacing - totalPadding) / columnCount; // 실제 셀 크기

        // Grid Layout Group 설정
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth); // 정사각형 셀
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // 간격 설정
        gridLayoutGroup.padding = new RectOffset(paddingInt, paddingInt, paddingInt, paddingInt); // 패딩 설정
    }

    private void Update()
    {
        // Canvas 크기가 동적으로 변할 수 있는 경우 Update에서 계속 갱신
        UpdateGridLayout();
    }
}
