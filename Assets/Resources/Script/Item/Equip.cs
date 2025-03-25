using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Equip : MonoBehaviour
{
    public static Equip Instance { get; private set; }

    [Header("Equipment Slots (SlotInven)")]
    public SlotInven weaponSlot;
    public SlotInven heartSlot;
    public SlotInven gearSlot;

    private int equip_hp;
    private int equip_attack;
    private int equip_ball_num;
    private int equip_pin_hp;


    private InventoryData equipItemData = new InventoryData(); // 저장과 로드를 위한 아이템 리스트.

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
        LoadEquipData();
    }

    public void LoadEquipData()
    {
        equipItemData = DataControl.LoadItemDataFromPrefs("PlayerEquip");
        if (equipItemData == null) return;
        for (int i = 0; i < equipItemData.items.Count; i++)
        {
            Item equipItem = ItemDatabase.Instance.FetchItemById(equipItemData.items[i].id);
            if (equipItem == null)
            {
                continue;
            }

            Equipment equip = equipItem as Equipment;
            if (equip == null)
            {
                continue;
            }

            // 저장 데이터를 UI 업데이트만 수행하도록 updateData를 false로 전달합니다.
            EquipItem(equip, equipItemData.items[i].amount, false, false);
        }
    }

    public void clearslot()
    {
        weaponSlot.ClearSlot();
        heartSlot.ClearSlot();
        gearSlot.ClearSlot();
    }

    /// <summary>
    /// 장비 슬롯에서 해당 장비를 해제하고,
    /// 해제된 장비의 전체 수량을 인벤토리에 다시 추가합니다.
    /// </summary>
    public void UnEquipItem(Equipment equip, bool save = false)
    {
        Debug.LogWarning($"{equip.ItemName}을 해제함.");
        if (equip == null)
        {
            return;
        }

        equipItemData.items.RemoveAll(item => item.id == equip.Id);

        int quantity = 0;
        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null && weaponSlot.itemData != null)
                {
                    equip_attack -= equip.MainStatValue;
                    equip_ball_num -= equip.SubStatValue;
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
                    equip_hp -= equip.MainStatValue;
                    equip_attack -= equip.SubStatValue;
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
                    equip_pin_hp -= equip.MainStatValue;
                    equip_ball_num -= equip.SubStatValue;
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
            Inventory.Instance.AddOrUpdateItems(new List<ItemDataForSave> { new ItemDataForSave(equip.Id, quantity) }, true);
        }
        AddItemStat();
        DataControl.SaveItemDataToPrefs("PlayerEquip", equipItemData);
    }

    /// <summary>
    /// 팝업의 Yes 버튼이 눌렸을 때 호출됩니다.
    /// 전달받은 Equipment 객체를 해당 장비 슬롯에 업데이트합니다.
    /// 이미 장착된 장비가 있으면 교체합니다.
    /// </summary>
    /// <param name="equip">장착할 Equipment 객체 (ItemData.item이 Equipment 형식임)</param>
    /// <summary>
    /// 팝업의 Yes 버튼이 눌렸을 때 호출됩니다.
    /// 전달받은 Equipment 객체를 해당 장비 슬롯에 업데이트합니다.
    /// 이미 장착된 장비가 있으면 교체합니다.
    /// </summary>
    /// <param name="equip">장착할 Equipment 객체 (ItemData.item이 Equipment 형식임)</param>
    /// <param name="amount">장착할 아이템의 수량</param>
    /// <param name="fromInv">인벤토리에서 장착하는 경우 true</param>
    /// <param name="updateData">저장 데이터를 업데이트할지 여부 (로드 시에는 false)</param>
    public void EquipItem(Equipment equip, int amount, bool fromInv = false, bool updateData = true)
    {
        Debug.LogWarning($"{equip.ItemName}을 장착함.");
        // 장착할 데이터 생성 (displayData로 슬롯 UI 갱신)
        ItemData equipItem = new ItemData(equip, amount);

        if (updateData)
        {
            equipItemData.items.Add(new ItemDataForSave(equip.Id, amount));
        }

        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null)
                {
                    weaponSlot.setInit(equipItem);
                    if (updateData)
                    {
                        equip_attack += equip.MainStatValue;
                        equip_ball_num += equip.SubStatValue;
                    }
                }
                else
                    Debug.LogError("weaponSlot이 할당되지 않았습니다.");
                break;
            case EquipmentType.Heart:
                if (heartSlot != null)
                {
                    heartSlot.setInit(equipItem);
                    if (updateData)
                    {
                        equip_hp += equip.MainStatValue;
                        equip_attack += equip.SubStatValue;
                    }
                }
                else
                    Debug.LogError("heartSlot이 할당되지 않았습니다.");
                break;
            case EquipmentType.Gear:
                if (gearSlot != null)
                {
                    gearSlot.setInit(equipItem);
                    if (updateData)
                    {
                        equip_pin_hp += equip.MainStatValue;
                        equip_ball_num += equip.SubStatValue;
                    }
                }
                else
                    Debug.LogError("gearSlot이 할당되지 않았습니다.");
                break;
            default:
                Debug.LogError("알 수 없는 장비 타입: " + equip.EquipType);
                return;
        }
        AddItemStat();
        if (updateData)
        {
            DataControl.SaveItemDataToPrefs("PlayerEquip", equipItemData); // 데이터 저장
        }
        if (fromInv)
        {
            // 인벤토리에서 해당 장비 아이템의 전체 수량 제거
            Inventory.Instance.RemoveItem(equip.Id, amount);
            Inventory.Instance.RefreshInventoryUI();
        }
    }

    private void AddItemStat()
    {
        Debug.LogWarning($"{equip_attack}, {equip_hp}, {equip_ball_num}, {equip_pin_hp}");

        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_HP", (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP")) + equip_hp).ToString());
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_ATK", (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK")) + equip_attack).ToString());
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_BALLCOUNT", (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT")) + equip_ball_num).ToString());
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_PINHP", (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP")) + equip_pin_hp).ToString());
        equip_hp =0; equip_attack =0; equip_ball_num =0 ; equip_pin_hp = 0;
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