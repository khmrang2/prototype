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

    private List<ItemDataForSave> equipItemData = new List<ItemDataForSave>(); // ����� �ε带 ���� ������ ����Ʈ.

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
        for (int i = 0; i < equipItemData.Count; i++)
        {
            Item equipItem = ItemDatabase.Instance.FetchItemById(equipItemData[i].id);
            if (equipItem == null)
            {
                continue;
            }

            Equipment equip = equipItem as Equipment;
            if (equip == null)
            {
                continue;
            }

            // ���� �����͸� UI ������Ʈ�� �����ϵ��� updateData�� false�� �����մϴ�.
            EquipItem(equip, equipItemData[i].amount, false, false);
        }
    }

    public void clearslot()
    {
        weaponSlot.ClearSlot();
        heartSlot.ClearSlot();
        gearSlot.ClearSlot();
    }

    /// <summary>
    /// ��� ���Կ��� �ش� ��� �����ϰ�,
    /// ������ ����� ��ü ������ �κ��丮�� �ٽ� �߰��մϴ�.
    /// </summary>
    public void UnEquipItem(Equipment equip, bool save = false)
    {
        if (equip == null)
        {
            return;
        }

        equipItemData.RemoveAll(item => item.id == equip.Id);

        int quantity = 0;
        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null && weaponSlot.itemData != null)
                {
                    quantity = weaponSlot.itemData.amount;
                    // ������ �����ϵ�, itemData�� �ʱ�ȭ
                    weaponSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Weapon slot�� ������ �������� �����ϴ�.");
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
                    Debug.LogWarning("Heart slot�� ������ �������� �����ϴ�.");
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
                    Debug.LogWarning("Gear slot�� ������ �������� �����ϴ�.");
                    return;
                }
                break;
            default:
                Debug.LogError("�� �� ���� ��� Ÿ��: " + equip.EquipType);
                return;
        }

        // ������ ����� ������ �κ��丮�� �߰�
        if (quantity > 0)
        {
            Inventory.Instance.AddOrUpdateItems(new List<ItemDataForSave> { new ItemDataForSave(equip.Id, quantity) }, true);
        }

        DataControl.SaveItemDataToPrefs("PlayerEquip", equipItemData);
    }

    /// <summary>
    /// �˾��� Yes ��ư�� ������ �� ȣ��˴ϴ�.
    /// ���޹��� Equipment ��ü�� �ش� ��� ���Կ� ������Ʈ�մϴ�.
    /// �̹� ������ ��� ������ ��ü�մϴ�.
    /// </summary>
    /// <param name="equip">������ Equipment ��ü (ItemData.item�� Equipment ������)</param>
    /// <summary>
    /// �˾��� Yes ��ư�� ������ �� ȣ��˴ϴ�.
    /// ���޹��� Equipment ��ü�� �ش� ��� ���Կ� ������Ʈ�մϴ�.
    /// �̹� ������ ��� ������ ��ü�մϴ�.
    /// </summary>
    /// <param name="equip">������ Equipment ��ü (ItemData.item�� Equipment ������)</param>
    /// <param name="amount">������ �������� ����</param>
    /// <param name="fromInv">�κ��丮���� �����ϴ� ��� true</param>
    /// <param name="updateData">���� �����͸� ������Ʈ���� ���� (�ε� �ÿ��� false)</param>
    public void EquipItem(Equipment equip, int amount, bool fromInv = false, bool updateData = true)
    {
        // ������ ������ ���� (displayData�� ���� UI ����)
        ItemData equipItem = new ItemData(equip, amount);

        if (updateData)
        {
            equipItemData.Add(new ItemDataForSave(equip.Id, amount));
        }

        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null)
                {
                    weaponSlot.setInit(equipItem);
                }
                else
                    Debug.LogError("weaponSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                break;
            case EquipmentType.Heart:
                if (heartSlot != null)
                {
                    heartSlot.setInit(equipItem);
                }
                else
                    Debug.LogError("heartSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                break;
            case EquipmentType.Gear:
                if (gearSlot != null)
                {
                    gearSlot.setInit(equipItem);
                }
                else
                    Debug.LogError("gearSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                break;
            default:
                Debug.LogError("�� �� ���� ��� Ÿ��: " + equip.EquipType);
                return;
        }

        if (updateData)
        {
            DataControl.SaveItemDataToPrefs("PlayerEquip", equipItemData); // ������ ����
        }
        if (fromInv)
        {
            // �κ��丮���� �ش� ��� �������� ��ü ���� ����
            Inventory.Instance.RemoveItem(equip.Id, amount);
            Inventory.Instance.RefreshInventoryUI();
        }
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
    /// �ش� ��� Ÿ�Կ� ���� ���Կ� �̹� ������ �������� �ִ��� Ȯ���մϴ�.
    /// </summary>
    public bool IsEquipped(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return (weaponSlot != null &&
                        weaponSlot.itemData != null &&
                        weaponSlot.itemData.item != null &&
                        weaponSlot.itemData.item.Id > 0);  // Id�� 0 ���϶�� ���������� ����
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