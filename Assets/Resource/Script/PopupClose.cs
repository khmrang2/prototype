using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClose : MonoBehaviour
{
    public GameObject popupUI;      // 팝업 UI

    public void ClosePopup()
    {
        Debug.Log("내가 눌렸다. : " + this.name + "\n");
        if (popupUI == null) return;

        // 팝업과 배경 비활성화
        popupUI.SetActive(false);
    }
}
