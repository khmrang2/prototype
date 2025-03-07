	using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class EquipmentUIPanel : MonoBehaviour
{
    public Image itemIcon;  // 아이콘 이미지 (단일 장비)
    public TMP_Text itemName;   // 장비 이름 (단일 장비)
    public TMP_Text rewardText; // 골드 보상 텍스트

    public GameObject itemSlotPrefab; // 장비 10개 뽑을 때 사용
    public Transform itemSlotParent;  // 장비 10개 뽑을 때 슬롯을 추가할 부모

    public GameObject itemPrefab;
	
	private int earn_gold = 0;

    /// <summary>
    /// 단일 장비 UI 표시
    /// </summary>
    public void ShowSingleEquipment(List<Item> equipments)
    {
		if (equipments == null || equipments.Count == 0) return;

        Item item = equipments[0];
		earn_gold = 0;

        ClearItemSlots(); // ✅ 기존 슬롯 삭제

        itemIcon.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(false);

        if (item.Sprite != null)
        {
            itemIcon.sprite = item.Sprite;
        }
        else
        {
            Debug.LogWarning($"⚠️ {item.ItemName}의 iconSprite가 없습니다.");
        }

        itemName.text = item.ItemName;

        // ✅ 골드 아이템일 경우 (ID == 30) 랜덤 골드 지급
        if (item.Id == 30)
        {
            int randomValue = Random.Range(50, 110);
            rewardText.gameObject.SetActive(true);
            rewardText.text = $"{randomValue}G earned!";
            earn_gold += randomValue; // 골드 합산
        }
    }

    
    	/// <summary>
		/// 10개 장비 UI 표시
		/// </summary>
    public void ShowMultipleEquipments(List<Item> equipments){
		earn_gold = 0;

    	if (equipments == null || equipments.Count == 0) return;
   		if (itemSlotPrefab == null || itemSlotParent == null) return;

    		// 단일 장비 UI 숨기기 (덮어쓰는 문제 방지)
    	itemIcon.gameObject.SetActive(false);
    	itemName.gameObject.SetActive(false);
    	rewardText.gameObject.SetActive(false);
    
    	ClearItemSlots(); // 기존 아이템 삭제

    	// 10개 장비 슬롯 추가
    	foreach (Item item in equipments){
        	if (item == null) continue;
       
        	GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
       		if (slot == null) continue;
        
        	GameObject itemObj = Instantiate(itemPrefab, slot.transform);
        	if (itemObj == null) continue;

        	RectTransform rect = itemObj.GetComponent<RectTransform>();
        
			if (rect != null) rect.anchoredPosition = Vector2.zero;

       		// 아이콘 설정
        	Image itemImage = itemObj.GetComponent<Image>();
        	if (itemImage == null) continue;

        	if (item.Sprite == null) itemImage.sprite = Resources.Load<Sprite>("DefaultIcon"); // 기본 아이콘 설정
        	else itemImage.sprite = item.Sprite;
        	

        	// 슬롯의 희귀도 UI 설정
        	SlotInven slotUI = slot.GetComponent<SlotInven>();
        	if (slotUI != null) slotUI.SetRarity(item.Rarity);	

			if (item.Id == 30){
        		TextMeshProUGUI textComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
        		if (textComponent != null){
        			   	int randomValue = Random.Range(50, 110); // 50 ~ 100 사이의 랜덤 숫자
        			    textComponent.text = randomValue.ToString();
        	    		earn_gold += randomValue;
        		}
			}
		}
	}

	
    /// <summary>
    /// 골드 보상 UI 표시
    /// </summary>
    public void ShowGoldReward(int goldAmount)
    {
        ClearItemSlots(); //기존 슬롯 삭제

        itemIcon.gameObject.SetActive(false);
		itemName.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(true);

        rewardText.text = $"{goldAmount}G earned!";
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
    }

	public int GetEarnedGold()
	{
    	return earn_gold;
	}

}
