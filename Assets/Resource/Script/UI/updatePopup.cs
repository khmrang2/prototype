using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updatePopup : MonoBehaviour
{
    public GameObject popUpUI; // �˾� ui

    public Button yesButton; // yes ��ư
    public Button noButton;  // no ��ư

    public ItemDataLoader itemDataLoader;
    [SerializeField]
    public TMP_Text tooltip;

    public EquipManager equipManager;

    private string itemCode;

    // ���� ������ ���̵�� ������ �����ɴϴ�. 
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

        // �˾��� ��� ��Ȱ��ȭ
        popUpUI.SetActive(false);
    }
}
