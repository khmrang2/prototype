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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    /// <summary>
    /// ��� ���� ���� (�ش� ������ �ʱ�ȭ)
    /// </summary>
    /// <summary>
    /// ��� ���Կ��� �ش� ��� �����ϰ�,
    /// ������ ����� ��ü ������ �κ��丮�� �ٽ� �߰��մϴ�.
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
            Inventory.Instance.AddOrUpdateItem(equip.Id, quantity);
        }
        Inventory.Instance.RefreshInventoryUI();
    }

    /// <summary>
    /// �˾��� Yes ��ư�� ������ �� ȣ��˴ϴ�.
    /// ���޹��� Equipment ��ü�� �ش� ��� ���Կ� ������Ʈ�մϴ�.
    /// �̹� ������ ��� ������ ��ü�մϴ�.
    /// </summary>
    /// <param name="equip">������ Equipment ��ü (ItemData.item�� Equipment ������)</param>
    public void EquipItem(Equipment equip)
    {
        // �κ��丮���� �ش� ����� ��ü ������ ������
        int quantity = Inventory.Instance.GetItemAmount(equip.Id);

        // ������ ������ ���� (displayData�� ���� UI ����)
        ItemData displayData = new ItemData(equip, quantity);

        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                if (weaponSlot != null)
                {
                    weaponSlot.setInit(displayData);
                    // DataControl�� equipmentItems ������Ʈ
                }
                else
                {
                    Debug.LogError("weaponSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                    return;
                }
                break;
            case EquipmentType.Heart:
                if (heartSlot != null)
                {
                    heartSlot.setInit(displayData);
                }
                else
                {
                    Debug.LogError("heartSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                    return;
                }
                break;
            case EquipmentType.Gear:
                if (gearSlot != null)
                {
                    gearSlot.setInit(displayData);
                }
                else
                {
                    Debug.LogError("gearSlot�� �Ҵ���� �ʾҽ��ϴ�.");
                    return;
                }
                break;
            default:
                Debug.LogError("�� �� ���� ��� Ÿ��: " + equip.EquipType);
                return;
        }

        // �κ��丮���� �ش� ��� �������� ��ü ���� ����
        Inventory.Instance.RemoveItem(equip.Id, quantity);
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
}