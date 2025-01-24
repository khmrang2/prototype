using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updatePopup : MonoBehaviour
{
    public ItemDataLoader itemDataLoader;
    [SerializeField]
    public TMP_Text tooltip;

    // 받은 아이템 아이디로 툴팁을 가져옵니다. 
    public void loadTooltip(string itemID)
    {
        tooltip.text = itemDataLoader.GetItemTooltip(itemID);
    }
}
