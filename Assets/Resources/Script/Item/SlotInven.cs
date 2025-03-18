using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotInven : MonoBehaviour
{
    public ItemData itemData;
    public int id = 0;

    [Header("Popup Comoponent")]
    public GameObject popUpPanel; // 활성화할 프리팹
    public updatePopup popUpScript;

    public Image backgroundImage;

    [Header("Set Rairity")]
    public Sprite commonSprite;    // rarity 0
    public Sprite uncommonSprite;  // rarity 1
    public Sprite rareSprite;      // rarity 2
    public Sprite epicSprite;      // rarity 3
    public Sprite legendarySprite; // rarity 4
    public Sprite usableSprite; // rarity 5

    [Header("Set Item Image.")]
    public GameObject inventoryItemPrefab;  // 생성할 아이템 프리팹.
    private GameObject showingItemObject;


    /// <summary>
    /// 팝업을 띄운다.
    /// </summary>
    public void showPopup()
    {
        if (itemData.item.Rarity != Rarity.Usuable)
        {
            PopUpManager.Instance.ShowPopup(id, itemData);
        }
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
            case Rarity.Usuable:
                backgroundImage.sprite = usableSprite;
                break;
            default:
                backgroundImage.sprite = commonSprite;
                break;
        }
    }

    public void ClearSlot()
    {
        // 아이템 데이터만 삭제
        itemData = null;

        // 배경 이미지를 기본 스프라이트로 설정 (여기서는 commonSprite를 기본값으로 사용)
        if (backgroundImage != null)
        {
            backgroundImage.sprite = commonSprite;
        }

        // 슬롯의 자식 오브젝트들 (예: 아이템 이미지, 수량 텍스트 등)을 기본 상태로 초기화
        // 여기서는 단순히 활성화를 꺼서 빈 슬롯처럼 보이게 합니다.
        foreach (Transform child in transform)
        {
            // 만약 새 아이템이 추가되면, 해당 오브젝트들을 다시 활성화하고 업데이트할 것입니다.
            child.gameObject.SetActive(false);
        }
    }

    public void setInit(ItemData itemdata)
    {
        this.itemData = itemdata;

        setItem(itemdata);
        SetRarity(itemdata.item.Rarity);

        foreach (Transform child in transform)
        {
            // 만약 새 아이템이 추가되면, 해당 오브젝트들을 다시 활성화하고 업데이트할 것입니다.
            child.gameObject.SetActive(true);
        }
    }

    private void setItem(ItemData itemdata)
    {
        Item item = itemdata.item;
        int a = itemdata.amount;


        // 쇼잉아이템 처음 호출되면 인스턴스 생성
        if (showingItemObject == null)
        {
            // false 옵션으로 Instantiate하면, 프리팹의 로컬 RectTransform 설정(앵커, 피벗 등)이 유지됩니다.
            showingItemObject = Instantiate(inventoryItemPrefab, this.transform, false);
        }
        else
        {
            // 이미 존재하면 활성화
            showingItemObject.GetComponent<Image>().sprite = item.Sprite;
        }
        showingItemObject.transform.SetParent(this.transform, false);

        RectTransform rect = showingItemObject.GetComponent<RectTransform>();
        if (rect != null)
            rect.anchoredPosition = Vector2.zero;

        Image itemImage = showingItemObject.GetComponent<Image>();
        itemImage.sprite = item.Sprite;
        showingItemObject.name = item.ItemName;

        // 자식 텍스트에 수량 표시 (수량 표시용 TextMeshProUGUI는 inventoryItemPrefab의 첫 번째 자식이라고 가정)
        TextMeshProUGUI qtyText = showingItemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        qtyText.text = a.ToString();
    }
}
