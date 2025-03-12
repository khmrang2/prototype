using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopup : MonoBehaviour
{
    public GameObject popUpPrefab; // Ȱ��ȭ�� ������ �˾�
    public updatePopup popUpScript; // Ȱ��ȭ�� �˾� �ڵ�
    public GameObject canvas; // �˾�â�� �׸� ĵ����.

    private GameObject popUpInstance; // Ȱ��ȭ�� ������ �˾�.

    // �˾�â�� ������ �ڽ��� �������� �Ѱ���� ��.

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ActivatePrefab()
    {
        if (popUpInstance == null)
        {
            // �˾� ĵ������ �θ�� �Ͽ� �˾��� ����, �������� RectTransform ������ ������.
            popUpInstance = Instantiate(popUpPrefab, canvas.transform, false);
        }
        else
        {
            popUpInstance.SetActive(true);
        }
        popUpScript = popUpPrefab.GetComponent<updatePopup>();
    }
}
