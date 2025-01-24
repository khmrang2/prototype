using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopup : MonoBehaviour
{
    public GameObject popUpPanel; // Ȱ��ȭ�� ������
    public updatePopup popUpScript;
    private GameObject spawnedPopUp; // ������ ������Ʈ�� ������ ����

    [SerializeField] public string itemID = "1"; // ������ �ĺ���

    [SerializeField] public ItemDataLoader itemDataLoader;

    // �˾�â�� ������ �ڽ��� �������� �Ѱ���� ��.

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ActivatePrefab()
    {
        if (popUpPanel == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }
        popUpScript.loadTooltip(itemID);

        popUpPanel.SetActive(true);
    }
}
