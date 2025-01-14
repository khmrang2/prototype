using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPopup : MonoBehaviour
{
    public GameObject popUpPanel; // Ȱ��ȭ�� ������
    private GameObject spawnedPopUp; // ������ ������Ʈ�� ������ ����

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ActivatePrefab()
    {
        if (popUpPanel == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        popUpPanel.SetActive(true);
    }
}
