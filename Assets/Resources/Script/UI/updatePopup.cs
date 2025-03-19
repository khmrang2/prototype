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
    public TextMeshProUGUI itemPrimaryStatName;
    public TextMeshProUGUI itemPrimaryStat;
    public TextMeshProUGUI itemSecondaryStatName;
    public TextMeshProUGUI itemSecondaryStat;

    public FontSizeAdjuster fontSizeAdjuster;

    public GameObject popUpUI; // 팝업 ui

    public Button yesButton; // yes 버튼
    public Button noButton;  // no 버튼
    // 이미 이 시점에는 SlotInven.cs에 item이 있음.

    private Equipment equip;
    private int amount;
    private int popupIdentifier; // 장비(0)에서 온건지 슬롯(1)에서 온건지 

    // 받은 아이템 아이디로 툴팁을 가져옵니다. 
    public void loadItem(int popupid, ItemData itemData)
    {
        popupIdentifier = popupid;
        equip = (Equipment)itemData.item;
        amount = itemData.amount;

        slotUI.setInit(itemData);
        loadData(equip);
        updateWithRairity(equip.Rarity);
        fontSizeAdjuster.adjustFont();
    }

    /// <summary>
    /// 
    /// 아이템 이름을 레어리티에 따라 색깔 변경한다.
    /// </summary>
    /// <param name="rarity"></param>
    private void updateWithRairity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                itemName.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case Rarity.Uncommon:
                itemName.color = new Color(0.2f, 0.8f, 0.3f);
                break;
            case Rarity.Rare:
                itemName.color = new Color(0.25f, 0.4f, 1f);
                break;
            case Rarity.Epic:
                itemName.color = new Color(1f, 0.78f, 0.25f);
                break;
            case Rarity.Legendary:
                itemName.color = new Color(1f, 0.3f, 0.3f);
                break;
            default:
                Debug.LogError("[아이템팝업] : 아이템 레어리티 잘못 불러옴.");
                break;
        }
    }

    private void loadData(Equipment equip)
    {
        itemName.text = equip.ItemName;
        itemDescription.text = equip.Tooltip;
        itemPrimaryStat.text = equip.MainStatValue.ToString();
        itemSecondaryStat.text = equip.SubStatValue.ToString();
        // 쓰레기코드라고생각함.. itemdata.json 구조 변경이 필요함.
        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                itemPrimaryStatName.text = "공격력";
                itemPrimaryStatName.text = "공 개수";
                break;
            case EquipmentType.Heart:
                itemPrimaryStatName.text = "체력";
                itemSecondaryStatName.text = "공격력";
                break;
            case EquipmentType.Gear:
                itemPrimaryStatName.text = "핀 체력";
                itemSecondaryStatName.text = "공 개수";
                break;
            default:
                Debug.LogError("[아이템팝업] : 아이템 스텟 네임 벨류 잘못 불러옴.");
                break;
        }

        if (popupIdentifier == 1)
        {
            // 장비 슬롯이 눌렸을 때
            //Debug.LogError("장비 - 해제밖에 없음.");
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "해제";
        }
        else if (popupIdentifier == 0)
        {
            // 인벤토리에서 호출된 경우:
            // 장비 타입에 해당하는 슬롯에 이미 장착되어 있다면 "교체", 아니면 "장착"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                //Debug.LogError("인벤토리 - 장착된 상태");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "교체";

            }
            else
            {
                //Debug.LogError("인벤토리 - X한 상태");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
            }
        }
    }

    public void yesClicked()
    {
        popUpUI.SetActive(false);
        if (popupIdentifier == 1)
        {
            // 장비에서 호출 된 경우 :
            // 바로 해제
            Equip.Instance.UnEquipItem(equip);
        }
        else if(popupIdentifier == 0)
        {
            Equipment currentEquipped = Equip.Instance.GetEquippedItem(equip.EquipType);
            // 인벤토리에서 호출된 경우:
            // 장비 타입에 해당하는 슬롯에 이미 장착되어 있다면 "교체", 아니면 "장착"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                Equip.Instance.UnEquipItem(currentEquipped);
            }
            // 장비 장착 로직 실행
            Equip.Instance.EquipItem(equip);
        }
    }
    public void noClicked()
    {
        if (popUpUI == null) return;

        // 팝업과 배경 비활성화
        popUpUI.SetActive(false);
    }


}
