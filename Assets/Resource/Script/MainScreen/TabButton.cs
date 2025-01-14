using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] private RectTransform icon;            // ������ �̹���

    [SerializeField] private Image background;              // �⺻ ��� �̹���
    [SerializeField] private Sprite active_background;       // Ȱ��ȭ ��� �̹���
    [SerializeField] private Sprite deactive_background;     // ��Ȱ��ȭ ��� �̹���

    [SerializeField] private Vector3 activePositionOffset;  // Ȱ��ȭ ��ġ �̵���.
    private Vector3 originalPosition;                       // �ʱ� ������ ��ġ

    private void Start()
    {
        // �ʱ� ��ġ ����
        if (icon != null) {
            Debug.Log("original is not set. : local position is : " + icon.localPosition);
            originalPosition = icon.anchoredPosition;
        }
    }

    // Activate() :
    // ��ư�� Ȱ��ȭ �Ǿ��� ��
    // 1. -> local position ã�ư���.
    // 2. �� ��� ���� -> activated����.
    public void Activate()
    {
        if (background != null)
        {
            // ���� ����
            background.sprite = active_background;
            // icon ��ġ ����
            icon.anchoredPosition = originalPosition + activePositionOffset;
        }
    }

    // ��Ȱ��ȭ �Ǿ�����
    // 1. -> local position ã�ư���.
    // 2. �� ��� ���� -> deactivated����.
    public void Deactivate()
    {
        if (background != null)
        {
            // ���� ����
            background.sprite = deactive_background;
            // icon ��ġ ����
            icon.anchoredPosition = originalPosition;
        }
    }
}
