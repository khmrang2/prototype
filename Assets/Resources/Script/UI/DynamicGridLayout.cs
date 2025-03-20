using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[ExecuteAlways]
public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // 조절하고 싶은 동적 Grid Layout Group을 연결
    [Tooltip("스크린 캔버스")]
    public RectTransform parentRectTransform; // 스크린 오브젝트(캔버스)

    protected float screenHeight; // 내부적으로 가지고 있을 스크린 크기
    protected float screenWidth; // 내부적으로 가지고 있을 스크린 크기
    //[SerializeField]
    //private float marginOffset = 10.0f; // 여백과 spacing을 주기위한 상수 offset(셀 사이즈의 10%)
    [SerializeField]
    protected int columnCount = 3; // 열개수: gridlayoutgroup에서 가져옴. 
    [SerializeField]
    protected int rowCount = 3; // 행개수: gridlayoutgroup에서 가져옴. 
    [SerializeField]
    [Tooltip("Cell Size에서 몇 %로 padding을 줄 것인지 계수.")]
    protected float paddingCoefficient = 0.1f; // cell size에서 몇 %를 줄지의 계수
    [SerializeField]
    [Tooltip("Cell Size에서 몇 %로 spacing을 줄 것인지 계수.")]
    protected float spacingCoefficient = 0.07f;
    [SerializeField]
    [Tooltip("Cell Size가 정사각형(0 default)으로 줄지 아니면 직사각형(1)으로 줄지")]
    protected int isSquare = 0;

    // 자식 클래스에서 접근할 수 있도록 protected로 선언
    protected float cellWidth; // 계산된 셀 크기를 저장하는 변수
    protected float cellHeight; // 계산된 셀 크기를 저장하는 변수

    private void Start()
    {
        StartCoroutine(WaitForRectTransformAndUpdate());
    }

    //디버깅용
    void OnValidate()
    {
        UpdateGridLayout();
    }

    // 화면전환이 이루어져서 오브젝트가 활성화 되었을 때, Layout의 UI의 크기 조절을 다시 해줌.
    // Update로 계속 연산하는 것보다 확실히 나음.
    private void OnEnable()
    {
        StartCoroutine(WaitForRectTransformAndUpdate());
    }
    /// <summary>
    /// 저는 코루틴에게 패배한 허접입니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForRectTransformAndUpdate()
    {
        yield return new WaitForEndOfFrame(); // 한 프레임 대기 후 실행

        while (parentRectTransform.rect.width == 0 || parentRectTransform.rect.height == 0)
        {
            yield return null; // RectTransform` 크기가 유효할 때까지 반복 대기
        }
        UpdateGridLayout();
    }

    // 인벤토리의 그리드 레이 아웃의 셀크기(인벤토리 슬롯 크기)를 동적으로 할당해주는 클래스 및 메소드
    // 화면 크기에 맞추어 셀 사이즈,  셀 여백, 셀 spacing을 동적으로 할당해준다.
    public virtual void UpdateGridLayout()
    {
        // 화면의 너비 높이 가져오기
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;

        //Debug.Log("screen width : " + screenWidth);

        //Debug.Log("screen hegiht : " + screenHeight);

        // 1. 수식에 기반하여 cellWidth 계산
        // cellWidth = screen / (columnCount + 열의수 * 2 * marginCoefficient + 열의수 - 1  * spacingCoefficient); 의 수식을 따름.
        cellWidth = screenWidth / (columnCount + (columnCount * 2) * paddingCoefficient + (columnCount - 1) * spacingCoefficient);
        cellHeight = screenHeight / (rowCount + (rowCount * 2) * paddingCoefficient + (rowCount - 1) * spacingCoefficient);

        // 2. margin과 spacing 계산
        int padding = Mathf.CeilToInt(paddingCoefficient * cellWidth);
        float spacing = spacingCoefficient * cellWidth;

        layoutUpdate(cellWidth, cellHeight, padding, spacing);
    }

    protected void layoutUpdate(float width, float height, int padding, float spacing)
    {
        if (isSquare == 0)
        {
            // 정사각형
            // Grid Layout Group 설정
            gridLayoutGroup.cellSize = new Vector2(width, width); // 셀 크기 (정사각형)

        }
        else if (isSquare == 1)
        {
            // 직사각형
            // Grid Layout Group 설정
            gridLayoutGroup.cellSize = new Vector2(width, height); // 셀 크기 (직사각형)

        }
        else
        {
            return;
        }
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // 간격 설정
        gridLayoutGroup.padding = new RectOffset(padding, padding, padding, padding); // 패딩 설정
    }

    // 최초 시작에만 해주면 되니까 굳이 해줄 필요가 없다.
    //private void Update()
    //{
    //    // Canvas 크기가 동적으로 변할 수 있는 경우 Update에서 계속 갱신
    //    UpdateGridLayout();
    //}
}