using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopup : MonoBehaviour
{
    public GameObject popUpPanel; // 활성화할 프리팹
    public updatePopup popUpScript;
    public SlotInven itemSlot;

    // 팝업창을 생성시 자신의 아이콘을 넘겨줘야 함.

    // 버튼 클릭 시 호출되는 함수
    public void ActivatePrefab()
    {
        if (popUpPanel == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }
        //popUpScript.loadTooltip(itemSlot.getItemID());

        popUpPanel.SetActive(true);
    }
}
