using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClose : MonoBehaviour
{
    public GameObject popupUI;      // �˾� UI

    public void ClosePopup()
    {
        Debug.Log("���� ���ȴ�. : " + this.name + "\n");
        if (popupUI == null) return;

        // �˾��� ��� ��Ȱ��ȭ
        popupUI.SetActive(false);
    }
}
