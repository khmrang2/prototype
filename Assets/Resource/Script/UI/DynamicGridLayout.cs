using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // 조절하고 싶은 동적 Grid Layout Group을 연결
    [Tooltip("스크린 캔버스")]
    public RectTransform parentRectTransform; // 스크린 오브젝트(캔버스)

    private float screenHeight; // 내부적으로 가지고 있을 스크린 크기
    private float screenWidth; // 내부적으로 가지고 있을 스크린 크기
    //[SerializeField]
    //private float marginOffset = 10.0f; // 여백과 spacing을 주기위한 상수 offset(셀 사이즈의 10%)
    [SerializeField] 
    private int columnCount = 3; // 열개수: gridlayoutgroup에서 가져옴. 
    [SerializeField][Tooltip("Cell Size에서 몇 %로 padding을 줄 것인지 계수.")]
    protected float paddingCoefficient = 0.1f; // cell size에서 몇 %를 줄지의 계수
    [SerializeField][Tooltip("Cell Size에서 몇 %로 spacing을 줄 것인지 계수.")]
    protected float spacingCoefficient = 0.07f;

    // 자식 클래스에서 접근할 수 있도록 protected로 선언
    protected float cellSize; // 계산된 셀 크기를 저장하는 변수

    private void Start()
    {
        UpdateGridLayout();
    }

    // 화면전환이 이루어져서 오브젝트가 활성화 되었을 때, Layout의 UI의 크기 조절을 다시 해줌.
    // Update로 계속 연산하는 것보다 확실히 나음.
    private void OnEnable()
    {
        UpdateGridLayout();
    }


    // 인벤토리의 그리드 레이 아웃의 셀크기(인벤토리 슬롯 크기)를 동적으로 할당해주는 클래스 및 메소드
    // 화면 크기에 맞추어 셀 사이즈,  셀 여백, 셀 spacing을 동적으로 할당해준다.
    public virtual void UpdateGridLayout()
    {
        // 화면의 너비 높이 가져오기
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;
        

        // 1. 수식에 기반하여 cellSize 계산
        // cellSize = minCell / (columnCount + 열의수*2 * marginCoefficient + 열의수-1  * spacingCoefficient); 의 수식을 따름.
        cellSize = screenWidth / (columnCount + (columnCount*2) * paddingCoefficient + (columnCount-1) * spacingCoefficient);

        // 2. cellSize가 screenHeight를 초과하면 재조정
        if (cellSize > screenHeight)
        {
            // 화면 높이에 맞추기
            cellSize = screenHeight;

            // 화면 높이에 맞춘 cellSize로 padding과 spacing을 다시 계산
            float totalHorizontalSpacing = (columnCount - 1) * spacingCoefficient * cellSize;
            float totalPadding = (columnCount * 2) * paddingCoefficient * cellSize;

            // 화면 너비를 넘지 않도록 cellSize를 다시 계산
            if (totalHorizontalSpacing + totalPadding + columnCount * cellSize > screenWidth)
            {
                cellSize = (screenWidth - totalHorizontalSpacing - totalPadding) / columnCount;
            }
        }

        // 2. margin과 spacing 계산
        int padding = Mathf.CeilToInt(paddingCoefficient * cellSize);
        float spacing = spacingCoefficient * cellSize;

        // Grid Layout Group 설정
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize); // 셀 크기 (정사각형)
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // 간격 설정
        gridLayoutGroup.padding = new RectOffset(padding, padding,0,0 ); // 패딩 설정
    }

    // 최초 시작에만 해주면 되니까 굳이 해줄 필요가 없다.
    //private void Update()
    //{
    //    // Canvas 크기가 동적으로 변할 수 있는 경우 Update에서 계속 갱신
    //    UpdateGridLayout();
    //}
}
