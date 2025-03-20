using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[ExecuteAlways]
public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // �����ϰ� ���� ���� Grid Layout Group�� ����
    [Tooltip("��ũ�� ĵ����")]
    public RectTransform parentRectTransform; // ��ũ�� ������Ʈ(ĵ����)

    protected float screenHeight; // ���������� ������ ���� ��ũ�� ũ��
    protected float screenWidth; // ���������� ������ ���� ��ũ�� ũ��
    //[SerializeField]
    //private float marginOffset = 10.0f; // ����� spacing�� �ֱ����� ��� offset(�� �������� 10%)
    [SerializeField]
    protected int columnCount = 3; // ������: gridlayoutgroup���� ������. 
    [SerializeField]
    protected int rowCount = 3; // �ళ��: gridlayoutgroup���� ������. 
    [SerializeField]
    [Tooltip("Cell Size���� �� %�� padding�� �� ������ ���.")]
    protected float paddingCoefficient = 0.1f; // cell size���� �� %�� ������ ���
    [SerializeField]
    [Tooltip("Cell Size���� �� %�� spacing�� �� ������ ���.")]
    protected float spacingCoefficient = 0.07f;
    [SerializeField]
    [Tooltip("Cell Size�� ���簢��(0 default)���� ���� �ƴϸ� ���簢��(1)���� ����")]
    protected int isSquare = 0;

    // �ڽ� Ŭ�������� ������ �� �ֵ��� protected�� ����
    protected float cellWidth; // ���� �� ũ�⸦ �����ϴ� ����
    protected float cellHeight; // ���� �� ũ�⸦ �����ϴ� ����

    private void Start()
    {
        StartCoroutine(WaitForRectTransformAndUpdate());
    }

    //������
    void OnValidate()
    {
        UpdateGridLayout();
    }

    // ȭ����ȯ�� �̷������ ������Ʈ�� Ȱ��ȭ �Ǿ��� ��, Layout�� UI�� ũ�� ������ �ٽ� ����.
    // Update�� ��� �����ϴ� �ͺ��� Ȯ���� ����.
    private void OnEnable()
    {
        StartCoroutine(WaitForRectTransformAndUpdate());
    }
    /// <summary>
    /// ���� �ڷ�ƾ���� �й��� �����Դϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForRectTransformAndUpdate()
    {
        yield return new WaitForEndOfFrame(); // �� ������ ��� �� ����

        while (parentRectTransform.rect.width == 0 || parentRectTransform.rect.height == 0)
        {
            yield return null; // RectTransform` ũ�Ⱑ ��ȿ�� ������ �ݺ� ���
        }
        UpdateGridLayout();
    }

    // �κ��丮�� �׸��� ���� �ƿ��� ��ũ��(�κ��丮 ���� ũ��)�� �������� �Ҵ����ִ� Ŭ���� �� �޼ҵ�
    // ȭ�� ũ�⿡ ���߾� �� ������,  �� ����, �� spacing�� �������� �Ҵ����ش�.
    public virtual void UpdateGridLayout()
    {
        // ȭ���� �ʺ� ���� ��������
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;

        //Debug.Log("screen width : " + screenWidth);

        //Debug.Log("screen hegiht : " + screenHeight);

        // 1. ���Ŀ� ����Ͽ� cellWidth ���
        // cellWidth = screen / (columnCount + ���Ǽ� * 2 * marginCoefficient + ���Ǽ� - 1  * spacingCoefficient); �� ������ ����.
        cellWidth = screenWidth / (columnCount + (columnCount * 2) * paddingCoefficient + (columnCount - 1) * spacingCoefficient);
        cellHeight = screenHeight / (rowCount + (rowCount * 2) * paddingCoefficient + (rowCount - 1) * spacingCoefficient);

        // 2. margin�� spacing ���
        int padding = Mathf.CeilToInt(paddingCoefficient * cellWidth);
        float spacing = spacingCoefficient * cellWidth;

        layoutUpdate(cellWidth, cellHeight, padding, spacing);
    }

    protected void layoutUpdate(float width, float height, int padding, float spacing)
    {
        if (isSquare == 0)
        {
            // ���簢��
            // Grid Layout Group ����
            gridLayoutGroup.cellSize = new Vector2(width, width); // �� ũ�� (���簢��)

        }
        else if (isSquare == 1)
        {
            // ���簢��
            // Grid Layout Group ����
            gridLayoutGroup.cellSize = new Vector2(width, height); // �� ũ�� (���簢��)

        }
        else
        {
            return;
        }
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // ���� ����
        gridLayoutGroup.padding = new RectOffset(padding, padding, padding, padding); // �е� ����
    }

    // ���� ���ۿ��� ���ָ� �Ǵϱ� ���� ���� �ʿ䰡 ����.
    //private void Update()
    //{
    //    // Canvas ũ�Ⱑ �������� ���� �� �ִ� ��� Update���� ��� ����
    //    UpdateGridLayout();
    //}
}