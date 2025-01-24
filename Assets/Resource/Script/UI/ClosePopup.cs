using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject popupUI;      // �˾� UI

    public void closePopup()
    {
        if (popupUI == null) return;

        // �˾��� ��� ��Ȱ��ȭ
        popupUI.SetActive(false);
    }
}
