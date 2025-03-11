using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotInven : MonoBehaviour
{
    
    public Item item;              // 팝업에 띄우기 위한 아이템 데이터.

    public GameObject popUpPanel; // 활성화할 프리팹
    public updatePopup popUpScript;

    public Image backgroundImage;

    public Sprite commonSprite;    // rarity 0
    public Sprite uncommonSprite;  // rarity 1
    public Sprite rareSprite;      // rarity 2
    public Sprite epicSprite;      // rarity 3
    public Sprite legendarySprite; // rarity 4

    private void Awake()
    {
        if(item != null)
        {
            SetRarity(item.Rarity);
        }
    }
    /// <summary>
    /// 팝업을 띄운다.
    /// </summary>
    public void showPopup()
    {
        if (popUpPanel == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        //popUpPanel.

        popUpPanel.SetActive(true);
    }

    /// <summary>
    /// rarity 값에 따라 배경 이미지를 설정합니다.
    /// </summary>
    public void SetRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                backgroundImage.sprite = commonSprite;
                break;
            case Rarity.Uncommon:
                backgroundImage.sprite = uncommonSprite;
                break;
            case Rarity.Rare:
                backgroundImage.sprite = rareSprite;
                break;
            case Rarity.Epic:
                backgroundImage.sprite = epicSprite;
                break;
            case Rarity.Legendary:
                backgroundImage.sprite = legendarySprite;
                break;
            default:
                backgroundImage.sprite = commonSprite;
                break;
        }
    }
}
