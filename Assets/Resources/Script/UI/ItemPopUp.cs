using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    private Item item;

    public Button yesButton; // yes ��ư
    public Button noButton;  // no ��ư

    //public ItemDataLoader itemDataLoader;
    [SerializeField]
    public TMP_Text tooltip;

    private string itemCode;

    public void loadTooltip(Item handleItem)
    {
        item = handleItem;

        // ���� ���⼭ ��� �� �Ѵ�.
        // 
    }

    public void yesClicked()
    {

    }
    public void noClicked()
    {
    }
}
