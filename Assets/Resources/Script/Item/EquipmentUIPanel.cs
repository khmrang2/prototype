	using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using System.Collections.Generic;

public class EquipmentUIPanel : MonoBehaviour
{
    public GameObject panel;
    [Header("Single Equipment UI")]
    //public Image itemIcon;  // 아이콘 이미지 (단일 장비)
    public TMP_Text itemName;   // 장비 이름 (단일 장비)
    public Transform singleItemSlotParent; // 장비 1개의 뽑을 때 슬롯이 될 부모.

    [Header("Multiple Equipment UI")]
    public GameObject itemSlotPrefab; // 장비 10개 뽑을 때 사용
    public Transform itemSlotParent;  // 장비 10개 뽑을 때 슬롯을 추가할 부모

    private List<ItemDataForSave> handToInventory = new List<ItemDataForSave>(); // 인벤토리에 넘겨줄 아이템들

    private int earn_gold = 0;
    private int earn_upgrade_stone = 0;

    /// <summary>
    /// 단일 장비 UI 표시
    /// </summary>
    public void ShowSingleEquipment(List<ItemDataForSave> item_idx_list)
    {
        // 초기화
        earn_gold = 0;
        earn_upgrade_stone = 0;
        if (item_idx_list == null || item_idx_list.Count == 0) return;

        // 단일 장비이므로 첫 번째 아이템만 사용
        int ItemId = item_idx_list[0].id;
        int amount = item_idx_list[0].amount;

        // 기존 슬롯(다중 UI) 클리어 및 인벤토리용 데이터 초기화
        ClearItemSlots();
        if(handToInventory != null)
            handToInventory.Clear();

        // 단일 장비 UI 활성화
        singleItemSlotParent.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        itemSlotParent.gameObject.SetActive(false);

        //슬롯 생성
        GameObject slot = Instantiate(itemSlotPrefab, singleItemSlotParent);
        SlotInven slotUI = slot.GetComponent<SlotInven>();

        ItemData itemData = new ItemData(ItemDatabase.Instance.FetchItemById(ItemId), amount);

        slotUI.setInit(itemData);

        if (ItemId == ItemDatabase.ID_GOLD_POT)
        {
            itemName.text = $"{itemData.item.ItemName}을(를)\n{amount}G를획득했습니다.";
            earn_gold += amount;
        }
        else if (ItemId == ItemDatabase.ID_UPGRADE_ITEM)
        {
            itemName.text = $"{itemData.item.ItemName}을(를)\n{amount}개 획득했습니다!";
            earn_upgrade_stone += amount;
        }
        else
        {
            itemName.text = $"{itemData.item.ItemName}을(를)\n{amount}개 획득했습니다.";
            // 일반 장비는 인벤토리에 넘겨줄 데이터에 추가
            handToInventory.Add(item_idx_list[0]);
        }
    }

    
    	/// <summary>
		/// 10개 장비 UI 표시
		/// </summary>
    public bool ShowMultipleEquipments(List<ItemDataForSave> item_idx_list)
    {
		earn_gold = 0;
        earn_upgrade_stone = 0;

        if (item_idx_list == null || item_idx_list.Count == 0) return false;
   		if (itemSlotPrefab == null || itemSlotParent == null) return false;

        singleItemSlotParent.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        itemSlotParent.gameObject.SetActive(true);

        ClearItemSlots(); // 기존 아이템 삭제
        handToInventory.Clear(); // 인벤토리에 넘겨줄 아이템 리스트 초기화.

        Item item = null;
        int ItemId = 0;
        int amount = 0;

        // 10개 장비 슬롯 추가
        foreach (ItemDataForSave itemdataforsave in item_idx_list){
            // 아이템 잠시 저장.
            item = ItemDatabase.Instance.FetchItemById(itemdataforsave.id);
            ItemId = itemdataforsave.id;
            amount = itemdataforsave.amount;

            // 슬롯 생성.
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            if (slot == null)
            {
                Debug.LogError("아이템 슬롯이 생성되지 않음.");
                ItemId = -1;
                continue;
            }

            // 슬롯의 희귀도 UI 설정
            SlotInven slotUI = slot.GetComponent<SlotInven>();
            if (slotUI != null) slotUI.setInit(new ItemData(item, amount));

            // 예시 조건: 골드 항아리와 업그레이드 아이템은 ID로 구분
            if (ItemId == ItemDatabase.ID_GOLD_POT)
            {
                earn_gold += amount;
            }
            else if (ItemId == ItemDatabase.ID_UPGRADE_ITEM)
            {
                earn_upgrade_stone += amount;
            }
            else
            {
                handToInventory.Add(new ItemDataForSave(ItemId, amount));
            }
        }

        return true;
	}


    public List<ItemDataForSave> getItemDatas() { return handToInventory; }

    /// <summary>
    /// 골드 보상 UI 표시
    /// </summary>
    public void ShowGoldReward(int goldAmount)
    {
        ClearItemSlots(); //기존 슬롯 삭제

        //itemIcon.gameObject.SetActive(false);
		itemName.gameObject.SetActive(false);
    }

    /// <summary>
    /// 기존 슬롯 삭제
    /// </summary>
    private void ClearItemSlots()
    {
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in singleItemSlotParent)
        {
            Destroy(child.gameObject);
        }
    }

	public int GetEarnedGold()
	{
    	return earn_gold;
	}

    public int GetEarnedUpgradeStone()
    {
        return earn_upgrade_stone;
    }

    public void ClosePanel()
    {
        this.panel.SetActive(false);
        return;
    }
}
