using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    private Item item;

    public Button yesButton; // yes 버튼
    public Button noButton;  // no 버튼

    //public ItemDataLoader itemDataLoader;
    [SerializeField]
    public TMP_Text tooltip;

    private string itemCode;

    public void loadTooltip(Item handleItem)
    {
        item = handleItem;

        // 이제 여기서 모두 다 한다.
        // 
    }

    public void yesClicked()
    {

    }
    public void noClicked()
    {
    }
}
