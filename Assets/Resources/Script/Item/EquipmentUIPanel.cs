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


    /// <summary>
    /// 단일 장비 UI 표시
    /// </summary>
    public void ShowSingleEquipment(Item item)
    {
        if (item.Equals(default(Item))) // ✅ 올바른 Null 체크 방식
        {
            Debug.LogError("🚨 ShowSingleEquipment() - equipmentData가 기본 값(잘못된 데이터)입니다!");
            return;
        }

        ClearItemSlots(); // ✅ 기존 슬롯 삭제

        itemIcon.gameObject.SetActive(true);
		itemName.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(false);

        if (item.Sprite!= null)
        {
            itemIcon.sprite = item.Sprite;
        }
        else
        {
            Debug.LogWarning($"⚠️ {item.ItemName}의 iconSprite가 없습니다.");
        }

        itemName.text = item.ItemName;
    }

    /*/// <summary>
    /// 10개 장비 UI 표시
    /// </summary>
    public void ShowMultipleEquipments(List<Item> equipments)
    {
        Debug.Log($"🛠 10개 뽑기 실행 - 총 {equipments.Count}개 장비");

        if (equipments == null || equipments.Count == 0)
        {
            Debug.LogError("🚨 ShowMultipleEquipments() - equipments 리스트가 비어있습니다!");
            return;
        }

        if (itemSlotPrefab == null || itemSlotParent == null)
        {
            Debug.LogError("🚨 itemSlotPrefab 또는 itemSlotParent가 null입니다! Inspector에서 할당되었는지 확인하세요.");
            return;
        }

        //단일 장비 UI 숨기기 (덮어쓰는 문제 방지)
        itemIcon.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);
        
        ClearItemSlots(); // 기존 아이템 삭제

        //10개 장비 슬롯 추가
        foreach (Item item in equipments)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            Debug.Log($"생성된 프리팹: {slot.name}");

            GameObject itemObj = Instantiate(itemPrefab);
            itemObj.transform.SetParent(slot.transform, false);
            RectTransform rect = itemObj.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;

            //if (iconTransform == null)
            //{
            //    Debug.LogError($"🚨 {item.ItemName}의 프리팹에서 'item_icon'을 찾을 수 없습니다!");
            //    continue;
            //}

            //Image slotIcon = iconTransform.GetComponent<Image>();

            //if (slotIcon == null)
            //{
            //    Debug.LogError($"🚨 {item.ItemName}의 'item_icon'에 UI 컴포넌트가 없습니다!");
            //    continue;
            //}

            //if (item.Sprite == null)
            //{
            //    Debug.LogWarning($"⚠️ {item.ItemName}의 iconSprite가 null입니다. 기본 아이콘을 설정하세요.");
            //    continue;
            //}

            //아이콘 설정 및 강제 업데이트
            itemObj.GetComponent<Image>().sprite = item.Sprite;
            
            // rarity에 따라 슬롯 이미지 변경
            SlotInven slotUI = slot.GetComponent<SlotInven>();
            slotUI.SetRarity(item.Rarity);

            Debug.Log($"🎨 {item.ItemName} 슬롯에 아이콘 설정 완료: {item.ImgPath}");
        }

        Debug.Log($"🎉 10개 뽑기 완료 - 생성된 슬롯 수: {itemSlotParent.childCount}");
    }*/
    
    /// <summary>
/// 10개 장비 UI 표시
/// </summary>
public void ShowMultipleEquipments(List<Item> equipments)
{
    Debug.Log($"🛠 10개 뽑기 실행 - 총 {equipments.Count}개 장비");

    if (equipments == null || equipments.Count == 0)
    {
        Debug.LogError("🚨 ShowMultipleEquipments() - equipments 리스트가 비어있습니다!");
        return;
    }

    if (itemSlotPrefab == null || itemSlotParent == null)
    {
        Debug.LogError("🚨 itemSlotPrefab 또는 itemSlotParent가 null입니다! Inspector에서 할당되었는지 확인하세요.");
        return;
    }

    // 단일 장비 UI 숨기기 (덮어쓰는 문제 방지)
    itemIcon.gameObject.SetActive(false);
    itemName.gameObject.SetActive(false);
    rewardText.gameObject.SetActive(false);
    
    ClearItemSlots(); // 기존 아이템 삭제

    // 10개 장비 슬롯 추가
    foreach (Item item in equipments)
    {
        if (item == null)
        {
            Debug.LogWarning("⚠️ Null 아이템이 발견됨. 이 아이템을 건너뜁니다.");
            continue;
        }

        GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
        if (slot == null)
        {
            Debug.LogError($"🚨 {item.ItemName}의 슬롯 프리팹이 생성되지 않았습니다!");
            continue;
        }
        Debug.Log($"✅ 생성된 프리팹: {slot.name}");

        GameObject itemObj = Instantiate(itemPrefab, slot.transform);
        if (itemObj == null)
        {
            Debug.LogError($"🚨 {item.ItemName}의 아이템 프리팹이 생성되지 않았습니다!");
            continue;
        }

        RectTransform rect = itemObj.GetComponent<RectTransform>();
        if (rect != null)
            rect.anchoredPosition = Vector2.zero;
        else
            Debug.LogWarning($"⚠️ {item.ItemName}의 RectTransform을 찾을 수 없습니다.");

        // 아이콘 설정
        Image itemImage = itemObj.GetComponent<Image>();
        if (itemImage == null)
        {
            Debug.LogError($"🚨 {item.ItemName}의 Image 컴포넌트를 찾을 수 없습니다!");
            continue;
        }

        if (item.Sprite == null)
        {
            Debug.LogWarning($"⚠️ {item.ItemName}의 스프라이트가 null입니다. 기본 아이콘을 설정합니다.");
            itemImage.sprite = Resources.Load<Sprite>("DefaultIcon"); // 기본 아이콘 설정
        }
        else
        {
            itemImage.sprite = item.Sprite;
        }

        // 슬롯의 희귀도 UI 설정
        SlotInven slotUI = slot.GetComponent<SlotInven>();
        if (slotUI != null)
        {
            slotUI.SetRarity(item.Rarity);
        }
        else
        {
            Debug.LogError($"🚨 {item.ItemName}의 SlotInven 컴포넌트가 없습니다!");
        }

        Debug.Log($"🎨 {item.ItemName} 슬롯에 아이콘 설정 완료: {item.ImgPath}");
    }

    Debug.Log($"🎉 10개 뽑기 완료 - 생성된 슬롯 수: {itemSlotParent.childCount}");
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
}
