using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentUIPanel : MonoBehaviour
{
    public Image itemIcon;  // 아이콘 이미지 (단일 장비)
    public TMP_Text itemName;   // 장비 이름 (단일 장비)
    public TMP_Text rewardText; // 골드 보상 텍스트

    public GameObject itemSlotPrefab; // 장비 10개 뽑을 때 사용
    public Transform itemSlotParent;  // 장비 10개 뽑을 때 슬롯을 추가할 부모

    /// <summary>
    /// 단일 장비 UI 표시
    /// </summary>
    public void ShowSingleEquipment(EquipmentData equipmentData)
    {
        if (equipmentData.Equals(default(EquipmentData))) // ✅ 올바른 Null 체크 방식
        {
            Debug.LogError("🚨 ShowSingleEquipment() - equipmentData가 기본 값(잘못된 데이터)입니다!");
            return;
        }

        ClearItemSlots(); // ✅ 기존 슬롯 삭제

        itemIcon.gameObject.SetActive(true);
		itemName.gameObject.SetActive(true);
        rewardText.gameObject.SetActive(false);

        if (equipmentData.iconSprite != null)
        {
            itemIcon.sprite = equipmentData.iconSprite;
        }
        else
        {
            Debug.LogWarning($"⚠️ {equipmentData.name}의 iconSprite가 없습니다.");
        }

        itemName.text = equipmentData.name;
    }

    /// <summary>
    /// 10개 장비 UI 표시
    /// </summary>
    public void ShowMultipleEquipments(List<EquipmentData> equipments)
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

        // ✅ 단일 장비 UI 숨기기 (덮어쓰는 문제 방지)
        itemIcon.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);
        
        ClearItemSlots(); // 기존 아이템 삭제

        // ✅ 10개 장비 슬롯 추가
        foreach (var equipment in equipments)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            Debug.Log($"✅ 생성된 프리팹: {slot.name}");

            Transform iconTransform = slot.transform.Find("item_icon");

            if (iconTransform == null)
            {
                Debug.LogError($"🚨 {equipment.name}의 프리팹에서 'item_icon'을 찾을 수 없습니다!");
                continue;
            }

            Image slotIcon = iconTransform.GetComponent<Image>();

            if (slotIcon == null)
            {
                Debug.LogError($"🚨 {equipment.name}의 'item_icon'에 UI 컴포넌트가 없습니다!");
                continue;
            }

            if (equipment.iconSprite == null)
            {
                Debug.LogWarning($"⚠️ {equipment.name}의 iconSprite가 null입니다. 기본 아이콘을 설정하세요.");
                continue;
            }

            // ✅ 아이콘 설정 및 강제 업데이트
            slotIcon.sprite = equipment.iconSprite;
            slotIcon.enabled = false;
            slotIcon.enabled = true;

            Debug.Log($"🎨 {equipment.name} 슬롯에 아이콘 설정 완료: {equipment.iconSprite.name}");
        }

        Debug.Log($"🎉 10개 뽑기 완료 - 생성된 슬롯 수: {itemSlotParent.childCount}");
    }

    /// <summary>
    /// 골드 보상 UI 표시
    /// </summary>
    public void ShowGoldReward(int goldAmount)
    {
        ClearItemSlots(); // ✅ 기존 슬롯 삭제

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
        Debug.Log($"🗑 기존 슬롯 삭제 - {itemSlotParent.childCount}개 삭제됨");

        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("🗑 모든 슬롯 삭제 완료");
    }
}
