using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject popupUI;      // 팝업 UI

    public void closePopup()
    {
        if (popupUI == null) return;

        // 팝업과 배경 비활성화
        popupUI.SetActive(false);
    }
}
