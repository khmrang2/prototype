using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class DynamicGridLayoutForStore : DynamicGridLayout
{
    [SerializeField]
    [Tooltip("UI �̹����� �´� ������ �Է�.")]
    public float aspectRatio = 1.0f / 2.0f;

    // �κ��丮�� �׸��� ���� �ƿ��� ��ũ��(�κ��丮 ���� ũ��)�� �������� �Ҵ����ִ� Ŭ���� �� �޼ҵ�
    // ȭ�� ũ�⿡ ���߾� �� ������,  �� ����, �� spacing�� �������� �Ҵ����ش�.
    public override void UpdateGridLayout()
    {
        // ȭ���� �ʺ� ���� ��������
        screenWidth = parentRectTransform.rect.width;
        screenHeight = parentRectTransform.rect.height;

        // ���� �������� �ʺ� ����
        cellWidth = screenWidth / (columnCount + (columnCount * 2) * paddingCoefficient + (columnCount - 1) * spacingCoefficient);
        cellHeight = cellWidth / aspectRatio;

        // ȭ�� ���̸� �ʰ��ϴ� ��� ���̿� ���� ����
        if (cellHeight * rowCount > screenHeight)
        {
            cellHeight = screenHeight / (rowCount + (rowCount * 2) * paddingCoefficient + (rowCount - 1) * spacingCoefficient);
            cellWidth = cellHeight * aspectRatio;
        }

        // 2. margin�� spacing ���
        int padding = Mathf.CeilToInt(paddingCoefficient * cellWidth);
        float spacing = spacingCoefficient * cellWidth;

        layoutUpdate(cellWidth, cellHeight, padding, spacing);
    }

    // ���� ���ۿ��� ���ָ� �Ǵϱ� ���� ���� �ʿ䰡 ����.
    //private void Update()
    //{
    //    // Canvas ũ�Ⱑ �������� ���� �� �ִ� ��� Update���� ��� ����
    //    UpdateGridLayout();
    //}
}
