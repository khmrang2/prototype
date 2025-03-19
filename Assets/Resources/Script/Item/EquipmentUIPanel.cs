	using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class EquipmentUIPanel : MonoBehaviour
{
    [Header("Single Equipment UI")]
    //public Image itemIcon;  // 아이콘 이미지 (단일 장비)
    public TMP_Text itemName;   // 장비 이름 (단일 장비)
    public Transform singleItemSlotParent; // 장비 1개의 뽑을 때 슬롯이 될 부모.

    [Header("Multiple Equipment UI")]
    public GameObject itemSlotPrefab; // 장비 10개 뽑을 때 사용
    public Transform itemSlotParent;  // 장비 10개 뽑을 때 슬롯을 추가할 부모
    public GameObject itemPrefab;

    private List<ItemDataForSave> handToInventory = new List<ItemDataForSave>(); // 인벤토리에 넘겨줄 아이템들

    private int earn_gold = 0;

    private int earn_upgrade_stone = 0;

    /// <summary>
    /// 단일 장비 UI 표시
    /// </summary>
    public void ShowSingleEquipment(List<Item> equipments)
    {
        // 초기화
        earn_gold = 0;
        earn_upgrade_stone = 0;
        if (equipments == null || equipments.Count == 0) return;

        // 단일 장비이므로 첫 번째 아이템만 사용
        Item item = equipments[0];

        // 기존 슬롯(다중 UI) 클리어 및 인벤토리용 데이터 초기화
        ClearItemSlots();
        handToInventory.Clear();

        // 단일 장비 UI 활성화
        singleItemSlotParent.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        itemSlotParent.gameObject.SetActive(false);

        GameObject slot = Instantiate(itemSlotPrefab, singleItemSlotParent);
        SlotInven slotUI = slot.GetComponent<SlotInven>();
        
        // 아이템 수량 결정 및 처리
        int amount = 1;
        if (item.Id == ItemDatabase.ID_GOLD_POT)
        {
            // 골드 항아리: 50 ~ 110 사이의 랜덤 수량
            amount = Random.Range(50, 110);
            itemName.text = $"{amount}G를\n획득했습니다.";
            earn_gold += amount;
        }
        else if (item.Id == ItemDatabase.ID_UPGRADE_ITEM)
        {
            itemName.text = $"{item.ItemName}을(를)\n{amount}개 획득했습니다!";
            // 업그레이드 아이템: 예시로 1 ~ 10 사이의 랜덤 수량
            amount = Random.Range(1, 10);
            earn_upgrade_stone += amount;
        }
        else
        {
            // 일반 장비: 예시로 1 ~ 2 사이의 랜덤 수량
            amount = Random.Range(1, 3);
            itemName.text = $"{item.ItemName}을(를)\n{amount}개 획득했습니다.";
            // 일반 장비는 인벤토리에 넘겨줄 데이터에 추가
            handToInventory.Add(new ItemDataForSave(item.Id, amount));
        }

        slotUI.setInit(new ItemData(item, amount));
    }

    
    	/// <summary>
		/// 10개 장비 UI 표시
		/// </summary>
    public void ShowMultipleEquipments(List<Item> equipments){
		earn_gold = 0;
        earn_upgrade_stone = 0;

    	if (equipments == null || equipments.Count == 0) return;
   		if (itemSlotPrefab == null || itemSlotParent == null) return;

        singleItemSlotParent.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        itemSlotParent.gameObject.SetActive(true);

        ClearItemSlots(); // 기존 아이템 삭제
        handToInventory.Clear(); // 인벤토리에 넘겨줄 아이템 리스트 초기화.

        // 10개 장비 슬롯 추가
        foreach (Item item in equipments){
            // 아이템에 이미 코드가 다 있음.
        	if (item == null) continue;
       
        	GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
       		if (slot == null) continue;
            // 슬롯의 희귀도 UI 설정
            SlotInven slotUI = slot.GetComponent<SlotInven>();

            // 아이템 수량 결정 (기본은 장비: 1)
            int amount = 1;

            // 예시 조건: 골드 항아리와 업그레이드 아이템은 ID로 구분
            if (item.Id == ItemDatabase.ID_GOLD_POT)
            {
                // 골드 항아리: 50 ~ 110 사이의 랜덤 수량
                amount = Random.Range(50, 110);
                earn_gold += amount;
                /* 여기에 골드를 넣어줌. */
            }
            else if (item.Id == ItemDatabase.ID_UPGRADE_ITEM)
            {
                // 업그레이드 아이템: 예시로 1 ~ 10 사이의 랜덤 수량
                amount = Random.Range(1, 10);
                earn_upgrade_stone += amount;
                /* 여기에 업그레이드 아이템을 넣어줌. */
            }
            else
            {
                // 장비 아이템: 예시로 1 ~ 5 사이의 랜덤 수량
                amount = Random.Range(1, 3);
                // 인벤토리에 넘길 아이템 데이터 생성 및 저장
                handToInventory.Add(new ItemDataForSave(item.Id, amount));
            }
            if (slotUI != null) slotUI.setInit(new ItemData(item, amount));
        }
        
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
}
