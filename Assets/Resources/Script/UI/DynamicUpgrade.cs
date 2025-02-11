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
        // �θ� Ŭ������ UpdateGridLayout ����
        base.UpdateGridLayout();

        // �θ� Ŭ�������� ���� cellSize�� �߰� �۾� ����
        additionalUpdate();
    }

    /// <summary>
    /// progressBar�� contens�� ũ�⺯ȯ�� cellsize�� ���߾� height�� �����Ѵ�.
    /// </summary>
    private void additionalUpdate()
    {
        if (progressBar != null)
        {
            // progressBar�� ���̸� cellSize�� ������ŭ ����.
            progressBar.sizeDelta = new Vector2(progressBar.sizeDelta.x, (1 + spacingCoefficient) * cellHeight * (16 + 1));
        }

        if (contents != null)
        {
            // contents�� ���̸� cellSize�� �����ϰ� ����
            contents.sizeDelta = new Vector2(contents.sizeDelta.x, (1 + spacingCoefficient) * cellHeight * (16 + 1));
        }

        //Debug.Log("���׷��̵�� ���̾ƿ� �߰� �۾� ����!");
    }
}
