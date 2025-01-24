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

    // ���� ������ ���̵�� ������ �����ɴϴ�. 
    public void loadTooltip(string itemID)
    {
        tooltip.text = itemDataLoader.GetItemTooltip(itemID);
    }
}
