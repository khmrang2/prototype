using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updatePopup : MonoBehaviour
{
    [Header("Handling UI for update")]
    public SlotInven slotUI;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrimaryStat;
    public TextMeshProUGUI itemSecondaryStat;

    public FontSizeAdjuster fontSizeAdjuster;

    public GameObject popUpUI; // 팝업 ui

    public Button yesButton; // yes 버튼
    public Button noButton;  // no 버튼
    // 이미 이 시점에는 SlotInven.cs에 item이 있음.

    private Equipment equip;
    private int amount;
    
    // 받은 아이템 아이디로 툴팁을 가져옵니다. 
    public void loadItem(ItemData itemData)
    {
        equip = (Equipment)itemData.item;
        amount = itemData.amount;

        slotUI.setInit(itemData);
        loadData(equip);
        fontSizeAdjuster.adjustFont();
    }

    public void loadData(Equipment equip)
    {
        itemName.text = equip.ItemName;
        itemDescription.text = equip.Tooltip;
        itemPrimaryStat.text = equip.MainStatValue.ToString();
        itemSecondaryStat.text = equip.SubStatValue.ToString();
    }

    public void yesClicked()
    {
        popUpUI.SetActive(false);
        // 여기다가 초월 시스템 넣기.
        // amount 관련해서 넣을 예정.
    }
    public void noClicked()
    {
        if (popUpUI == null) return;

        // 팝업과 배경 비활성화
        popUpUI.SetActive(false);
    }
}
