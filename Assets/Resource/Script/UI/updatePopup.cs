using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updatePopup : MonoBehaviour
{
    public GameObject popUpUI; // 팝업 ui

    public Button yesButton; // yes 버튼
    public Button noButton;  // no 버튼

    public ItemDataLoader itemDataLoader;
    [SerializeField]
    public TMP_Text tooltip;

    public EquipManager equipManager;

    private string itemCode;

    // 받은 아이템 아이디로 툴팁을 가져옵니다. 
    public void loadTooltip(string itemID)
    {
        itemCode = itemID;
        tooltip.text = itemDataLoader.GetItemTooltip(itemID);
    }

    public void yesClicked()
    {
        
    }
    public void noClicked()
    {
        if (popUpUI == null) return;

        // 팝업과 배경 비활성화
        popUpUI.SetActive(false);
    }
}
