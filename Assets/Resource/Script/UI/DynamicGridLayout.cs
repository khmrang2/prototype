using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // �����ϰ� ���� ���� Grid Layout Group�� ����
    [Tooltip("��ũ�� ĵ����")]
    public RectTransform parentRectTransform; // ��ũ�� ������Ʈ(ĵ����)

    private float screenHeight; // ���������� ������ ���� ��ũ�� ũ��
    private float screenWidth; // ���������� ������ ���� ��ũ�� ũ��
    //[SerializeField]
    //private float marginOffset = 10.0f; // ����� spacing�� �ֱ����� ��� offset(�� �������� 10%)
    [SerializeField] 
    private int columnCount = 3; // ������: gridlayoutgroup���� ������. 
    [SerializeField][Tooltip("Cell Size���� �� %�� padding�� �� ������ ���.")]
    protected float paddingCoefficient = 0.1f; // cell size���� �� %�� ������ ���
    [SerializeField][Tooltip("Cell Size���� �� %�� spacing�� �� ������ ���.")]
    protected float spacingCoefficient = 0.07f;

    // �ڽ� Ŭ�������� ������ �� �ֵ��� protected�� ����
    protected float cellSize; // ���� �� ũ�⸦ �����ϴ� ����

    private void Start()
    {
        UpdateGridLayout();
    }

    // ȭ����ȯ�� �̷������ ������Ʈ�� Ȱ��ȭ �Ǿ��� ��, Layout�� UI�� ũ�� ������ �ٽ� ����.
    // Update�� ��� �����ϴ� �ͺ��� Ȯ���� ����.
    private void OnEnable()
    {
        UpdateGridLayout();
    }


    // �κ��丮�� �׸��� ���� �ƿ��� ��ũ��(�κ��丮 ���� ũ��)�� �������� �Ҵ����ִ� Ŭ���� �� �޼ҵ�
    // ȭ�� ũ�⿡ ���߾� �� ������,  �� ����, �� spacing�� �������� �Ҵ����ش�.
    public virtual void UpdateGridLayout()
    {
        // ȭ���� �ʺ� ���� ��������
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;
        

        // 1. ���Ŀ� ����Ͽ� cellSize ���
        // cellSize = minCell / (columnCount + ���Ǽ�*2 * marginCoefficient + ���Ǽ�-1  * spacingCoefficient); �� ������ ����.
        cellSize = screenWidth / (columnCount + (columnCount*2) * paddingCoefficient + (columnCount-1) * spacingCoefficient);

        // 2. cellSize�� screenHeight�� �ʰ��ϸ� ������
        if (cellSize > screenHeight)
        {
            // ȭ�� ���̿� ���߱�
            cellSize = screenHeight;

            // ȭ�� ���̿� ���� cellSize�� padding�� spacing�� �ٽ� ���
            float totalHorizontalSpacing = (columnCount - 1) * spacingCoefficient * cellSize;
            float totalPadding = (columnCount * 2) * paddingCoefficient * cellSize;

            // ȭ�� �ʺ� ���� �ʵ��� cellSize�� �ٽ� ���
            if (totalHorizontalSpacing + totalPadding + columnCount * cellSize > screenWidth)
            {
                cellSize = (screenWidth - totalHorizontalSpacing - totalPadding) / columnCount;
            }
        }

        // 2. margin�� spacing ���
        int padding = Mathf.CeilToInt(paddingCoefficient * cellSize);
        float spacing = spacingCoefficient * cellSize;

        // Grid Layout Group ����
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize); // �� ũ�� (���簢��)
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // ���� ����
        gridLayoutGroup.padding = new RectOffset(padding, padding,0,0 ); // �е� ����
    }

    // ���� ���ۿ��� ���ָ� �Ǵϱ� ���� ���� �ʿ䰡 ����.
    //private void Update()
    //{
    //    // Canvas ũ�Ⱑ �������� ���� �� �ִ� ��� Update���� ��� ����
    //    UpdateGridLayout();
    //}
}
