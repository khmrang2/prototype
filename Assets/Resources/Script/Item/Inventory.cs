using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class InventoryData
{
    public List<ItemDataForSave> items = new List<ItemDataForSave>();
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;

    // UI 슬롯과 아이템 데이터가 인덱스별로 매칭되는 리스트
    private List<GameObject> slots = new List<GameObject>();
    private List<ItemDataForSave> inventoryItems = new List<ItemDataForSave>();

    private const string SAVE_FILE = "/inventoryData.json";

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
    /// 인벤토리에 아이템을 추가하거나, 이미 존재하면 수량을 업데이트하고 UI를 갱신합니다.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">추가할 수량</param>
    public void AddOrUpdateItem(int id, int amount)
    {
        // 이미 같은 아이템이 있는지 검사
        int index = inventoryItems.FindIndex(x => x.id == id);
        if (index >= 0)
        {
            // 기존 아이템이 있다면 수량 업데이트
            inventoryItems[index].amount += amount;
            UpdateSlotUI(index, inventoryItems[index].amount);
        }
        else
        {
            // 없으면 데이터에 추가하고 UI 슬롯 생성
            inventoryItems.Add(new ItemDataForSave(id, amount));
            CreateItemUI(id, amount);
        }
        RefreshInventoryUI();
    }

    /// <summary>
    /// 여러 아이템을 한 번에 추가 또는 업데이트합니다.
    /// </summary>
    public void AddOrUpdateItems(List<ItemDataForSave> handItems)
    {
        foreach (ItemDataForSave data in handItems)
        {
            AddOrUpdateItem(data.id, data.amount);
        }
    }

    /// <summary>
    /// 새로운 슬롯 UI를 생성하고, SlotInven의 setInit()을 호출하여 아이템 데이터를 적용합니다.
    /// </summary>
    private void CreateItemUI(int id, int amount)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemById(id);
        if (itemToAdd == null)
        {
            Debug.LogWarning("해당 아이템 ID를 찾을 수 없습니다: " + id);
            return;
        }

        GameObject slot = Instantiate(inventorySlotPrefab, slotPanel.transform, false);
        SlotInven slotInven = slot.GetComponent<SlotInven>();
        if (slotInven != null)
        {
            slotInven.setInit(new ItemData(itemToAdd, amount));
        }
        else
        {
            Debug.LogWarning("SlotInven 컴포넌트를 찾을 수 없습니다.");
        }
        slots.Add(slot);
    }

    /// <summary>
    /// 주어진 인덱스의 슬롯 UI에서 수량 텍스트를 업데이트합니다.
    /// </summary>
    private void UpdateSlotUI(int index, int newAmount)
    {
        if (index < 0 || index >= slots.Count)
        {
            Debug.LogWarning("잘못된 슬롯 인덱스: " + index);
            return;
        }
        GameObject slot = slots[index];
        TextMeshProUGUI qtyText = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (qtyText != null)
        {
            qtyText.text = newAmount.ToString();
        }
    }

    /// <summary>
    /// 인벤토리 데이터를 JSON으로 저장합니다.
    /// </summary>
    public void SaveInventory()
    {
        InventoryData data = new InventoryData { items = inventoryItems };
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + SAVE_FILE;
        File.WriteAllText(path, json);
        Debug.Log("인벤토리 저장됨: " + path);
    }

    /// <summary>
    /// JSON 파일에서 인벤토리 데이터를 불러오고 UI를 재구성합니다.
    /// </summary>
    public void LoadInventory()
    {
        string path = Application.persistentDataPath + SAVE_FILE;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            ClearInventoryUI();
            inventoryItems.Clear();

            foreach (ItemDataForSave item in data.items)
            {
                AddOrUpdateItem(item.id, item.amount);
            }
        }
        else
        {
            Debug.Log("저장된 인벤토리 파일이 없습니다.");
        }
    }

    /// <summary>
    /// 모든 UI 슬롯을 삭제하고 slots 리스트를 초기화합니다.
    /// </summary>
    private void ClearInventoryUI()
    {
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();
    }

    /// <summary>
    /// 인벤토리 데이터를 레어리티 순으로 정렬한 후, UI를 갱신합니다.
    /// </summary>
    public void RefreshInventoryUI()
    {
        // inventoryItems 리스트를 정렬: 먼저 레어리티 기준, 그 다음 Equipment 타입 순
        inventoryItems.Sort((a, b) =>
        {
            Item itemA = ItemDatabase.Instance.FetchItemById(a.id);
            Item itemB = ItemDatabase.Instance.FetchItemById(b.id);
            if (itemA == null && itemB == null) return 0;
            if (itemA == null) return 1;
            if (itemB == null) return -1;

            // 1. 레어리티 기준 정렬 (낮은 값이 우선)
            int rarityComparison = itemA.Rarity.CompareTo(itemB.Rarity);
            if (rarityComparison != 0)
                return rarityComparison;

            // 2. 레어리티가 같다면, 두 아이템 모두 Equipment인 경우 EquipmentType 기준 정렬
            Equipment equipA = itemA as Equipment;
            Equipment equipB = itemB as Equipment;
            if (equipA != null && equipB != null)
            {
                return equipA.EquipType.CompareTo(equipB.EquipType);
            }
            // 만약 하나만 Equipment라면, Equipment를 우선 (원하는 순서에 따라 조정 가능)
            if (equipA != null && equipB == null)
                return -1;
            if (equipA == null && equipB != null)
                return 1;

            // 그 외의 경우, 두 아이템이 동일한 것으로 간주
            return 0;
        });

        // 기존 UI 슬롯 삭제
        ClearInventoryUI();
        // 정렬된 순서대로 UI 슬롯 재생성
        foreach (ItemDataForSave data in inventoryItems)
        {
            CreateItemUI(data.id, data.amount);
        }
    }
    public int GetItemAmount(int id)
    {
        int index = inventoryItems.FindIndex(x => x.id == id);
        return index >= 0 ? inventoryItems[index].amount : 0;
    }

    /// <summary>
    /// 인벤토리에서 특정 아이템을 지정한 수량만큼 제거합니다.
    /// 만약 기존 수량보다 작거나 같으면 해당 아이템 항목을 완전히 삭제합니다.
    /// </summary>
    /// <param name="id">제거할 아이템 ID</param>
    /// <param name="amount">제거할 수량</param>
    public void RemoveItem(int id, int amount)
    {
        // 해당 아이템이 있는지 검사
        int index = inventoryItems.FindIndex(x => x.id == id);
        if (index >= 0)
        {
            // 기존 수량이 제거할 수량보다 크면 업데이트
            if (inventoryItems[index].amount > amount)
            {
                inventoryItems[index].amount -= amount;
                UpdateSlotUI(index, inventoryItems[index].amount);
            }
            else
            {
                // 수량이 같거나 적으면 해당 항목을 삭제
                inventoryItems.RemoveAt(index);
                if (index < slots.Count)
                {
                    Destroy(slots[index]);
                    slots.RemoveAt(index);
                }
            }
        }
        else
        {
            Debug.LogWarning("RemoveItem: 해당 아이템이 인벤토리에 없습니다. ID: " + id);
        }
    }
}