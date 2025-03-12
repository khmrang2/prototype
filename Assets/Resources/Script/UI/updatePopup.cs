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

    public GameObject popUpUI; // �˾� ui

    public Button yesButton; // yes ��ư
    public Button noButton;  // no ��ư
    // �̹� �� �������� SlotInven.cs�� item�� ����.

    private Equipment equip;
    private int amount;
    
    // ���� ������ ���̵�� ������ �����ɴϴ�. 
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
        // ����ٰ� �ʿ� �ý��� �ֱ�.
        // amount �����ؼ� ���� ����.
    }
    public void noClicked()
    {
        if (popUpUI == null) return;

        // �˾��� ��� ��Ȱ��ȭ
        popUpUI.SetActive(false);
    }
}
