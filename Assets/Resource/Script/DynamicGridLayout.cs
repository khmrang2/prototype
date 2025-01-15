using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // �����ϰ� ���� ���� Grid Layout Group�� ����
    public RectTransform parentRectTransform; // ��ũ�� ������Ʈ(ĵ����)
    
    private float parentWidth; // ���������� ������ ���� ��ũ�� ũ��\
    [SerializeField]
    private float marginOffset = 10.0f; // ����� spacing�� �ֱ����� ��� offset(�� �������� 10%)

    private void Start()
    {
        UpdateGridLayout();
    }


    // �κ��丮�� �׸��� ���� �ƿ��� ��ũ��(�κ��丮 ���� ũ��)�� �������� �Ҵ����ִ� Ŭ���� �� �޼ҵ�
    // ȭ�� ũ�⿡ ���߾� �� ������,  �� ����, �� spacing�� �������� �Ҵ����ش�.
    private void UpdateGridLayout()
    {
        // �θ��� �ʺ� ��������
        float parentWidth = parentRectTransform.rect.width;

        // �� ���� ���� (4��)
        int columnCount = 4;

        // �� ���� ���� (���� ������ �θ� ũ�⿡ ���� ����)
        float spacing = parentWidth * 0.02f; // 2% ����

        // �е� ��� (�� ũ���� 10%�� ����)
        float padding = parentWidth * 0.05f; // 5% �е�
        int paddingInt = Mathf.RoundToInt(padding); // ������ ��ȯ

        // �� ũ�� ���: �е��� ������ �����Ͽ� �ʺ� ����
        float totalSpacing = spacing * (columnCount - 1); // ��� ������ ��
        float totalPadding = padding * 2; // �¿� �е� ��
        float cellWidth = (parentWidth - totalSpacing - totalPadding) / columnCount; // ���� �� ũ��

        // Grid Layout Group ����
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth); // ���簢�� ��
        gridLayoutGroup.spacing = new Vector2(spacing, spacing); // ���� ����
        gridLayoutGroup.padding = new RectOffset(paddingInt, paddingInt, paddingInt, paddingInt); // �е� ����
    }

    private void Update()
    {
        // Canvas ũ�Ⱑ �������� ���� �� �ִ� ��� Update���� ��� ����
        UpdateGridLayout();
    }
}
