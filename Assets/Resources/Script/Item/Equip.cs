using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public static Equip Instance { get; private set; }

    [Header("Equipment Slots (SlotInven)")]
    public SlotInven weaponSlot;
    public SlotInven heartSlot;
    public SlotInven gearSlot;

    private List<ItemDataForSave> equipItemData = new List<ItemDataForSave>(); // 저장과 로드를 위한 아이템 리스트.

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        equipItemData = DataControl.LoadInventoryFromPrefs();
        //foreach(var item in inventoryItemData)
        //{
        //    Debug.Log($"{item.id}와 {item.amount}가 로드됨.");
        //}
        // 로드는 잘되는데 ui를 못불러오네?
    }


    /// <summary>
    /// 장비 해제 로직 (해당 슬롯을 초기화)
    /// </summary>
    /// <summary>
    /// 장비 슬롯에서 해당 장비를 해제하고,
    /// 해제된 장비의 전체 수량을 인벤토리에 다시 추가합니다.
    /// </summary>
    public void UnEquipItem(Equipment equip)
    {
        if (equip == null)
        {
            Debug.LogError("UnEquipItem: equip is null");
            return;
        }

        int quantity = 0;
        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null && weaponSlot.itemData != null)
                {
                    quantity = weaponSlot.itemData.amount;
                    // 슬롯은 유지하되, itemData만 초기화
                    weaponSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Weapon slot에 장착된 아이템이 없습니다.");
                    return;
                }
                break;
            case EquipmentType.Heart:
                if (heartSlot != null && heartSlot.itemData != null)
                {
                    quantity = heartSlot.itemData.amount;
                    heartSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Heart slot에 장착된 아이템이 없습니다.");
                    return;
                }
                break;
            case EquipmentType.Gear:
                if (gearSlot != null && gearSlot.itemData != null)
                {
                    quantity = gearSlot.itemData.amount;
                    gearSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Gear slot에 장착된 아이템이 없습니다.");
                    return;
                }
                break;
            default:
                Debug.LogError("알 수 없는 장비 타입: " + equip.EquipType);
                return;
        }

        // 해제된 장비의 수량을 인벤토리에 추가
        if (quantity > 0)
        {
            Inventory.Instance.AddOrUpdateItems(new List<ItemDataForSave> { new ItemDataForSave(equip.Id, quantity) });
        }
        Inventory.Instance.RefreshInventoryUI();
    }

    /// <summary>
    /// 팝업의 Yes 버튼이 눌렸을 때 호출됩니다.
    /// 전달받은 Equipment 객체를 해당 장비 슬롯에 업데이트합니다.
    /// 이미 장착된 장비가 있으면 교체합니다.
    /// </summary>
    /// <param name="equip">장착할 Equipment 객체 (ItemData.item이 Equipment 형식임)</param>
    public void EquipItem(Equipment equip, int amount)
    {
        // 장착할 데이터 생성 (displayData로 슬롯 UI 갱신)
        ItemData displayData = new ItemData(equip, amount);

        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null)
                {
                    weaponSlot.setInit(displayData);
                    // DataControl의 equipmentItems 업데이트
                }
                else
                    Debug.LogError("weaponSlot이 할당되지 않았습니다.");
                break;
            case EquipmentType.Heart:
                if (heartSlot != null)
                {
                    heartSlot.setInit(displayData);
                }
                else
                    Debug.LogError("heartSlot이 할당되지 않았습니다.");
                break;
            case EquipmentType.Gear:
                if (gearSlot != null)
                {
                    gearSlot.setInit(displayData);
                }
                else
                    Debug.LogError("gearSlot이 할당되지 않았습니다.");
                break;
            default:
                Debug.LogError("알 수 없는 장비 타입: " + equip.EquipType);
                return;
        }
        Debug.Log($"아이템 id : {equip.Id}가 인벤토리에서 삭제됨.");
        // 인벤토리에서 해당 장비 아이템의 전체 수량 제거
        Inventory.Instance.RemoveItem(equip.Id, amount);
        Inventory.Instance.RefreshInventoryUI();
    }

    public Equipment GetEquippedItem(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return (weaponSlot != null && weaponSlot.itemData != null) ? (Equipment)weaponSlot.itemData.item : null;
            case EquipmentType.Heart:
                return (heartSlot != null && heartSlot.itemData != null) ? (Equipment)heartSlot.itemData.item : null;
            case EquipmentType.Gear:
                return (gearSlot != null && gearSlot.itemData != null) ? (Equipment)gearSlot.itemData.item : null;
            default:
                return null;
        }
    }

    /// <summary>
    /// 해당 장비 타입에 대해 슬롯에 이미 장착된 아이템이 있는지 확인합니다.
    /// </summary>
    public bool IsEquipped(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return (weaponSlot != null &&
                        weaponSlot.itemData != null &&
                        weaponSlot.itemData.item != null &&
                        weaponSlot.itemData.item.Id > 0);  // Id가 0 이하라면 미장착으로 간주
            case EquipmentType.Heart:
                return (heartSlot != null &&
                        heartSlot.itemData != null &&
                        heartSlot.itemData.item != null &&
                        heartSlot.itemData.item.Id > 0);
            case EquipmentType.Gear:
                return (gearSlot != null &&
                        gearSlot.itemData != null &&
                        gearSlot.itemData.item != null &&
                        gearSlot.itemData.item.Id > 0);
            default:
                return false;
        }
    }
}