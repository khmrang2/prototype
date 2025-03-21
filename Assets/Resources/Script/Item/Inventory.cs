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

    // UI 슬롯과 아이템 데이터가 인덱스별로 매칭되는 리스트
    private List<GameObject> slots = new List<GameObject>();
    private List<ItemDataForSave> inventoryItemData = new List<ItemDataForSave>(); // 저장과 로드를 위한 아이템 리스트. 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void loadInv()
    {
        inventoryItemData = DataControl.LoadItemDataFromPrefs("PlayerInventory");
        RefreshInventoryUI();

        foreach(var item in inventoryItemData)
        {
            Debug.Log($"{item.id}와 {item.amount}가 로드됨.");
        }
    }

    private void Start()
    {
        loadInv();
        RefreshInventoryUI();
        //foreach(var item in inventoryItemData)
        //{
        //    Debug.Log($"{item.id}와 {item.amount}가 로드됨.");
        //}
        // 로드는 잘되는데 ui를 못불러오네?
    }

    /// <summary>
    /// 1. 인벤토리에 아이템을 추가하거나, 
    /// 이미 존재하면 수량을 업데이트하고 
    /// UI를 갱신합니다.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">추가할 수량</param>
    public void AddOrUpdateItem(int id, int amount)
    {
        int index = inventoryItemData.FindIndex(x => x.id == id);
        if (index >= 0)
        {
            // 이미 아이템을 가지고 있는 경우.
            inventoryItemData[index].amount += amount;
        }
        else
        {   // 아이템을 새로 획득한 경우.
            inventoryItemData.Add(new ItemDataForSave(id, amount));
        }

        RefreshInventoryUI(); // 정렬 및 UI 업데이트는 여기에서 수행
    }

    /// <summary>
    /// 1b. 여러 아이템을 한 번에 추가 또는 업데이트합니다.
    /// </summary>
    public void AddOrUpdateItems(List<ItemDataForSave> handItems, bool save = false)
    {
        foreach (ItemDataForSave data in handItems)
        {
            AddOrUpdateItem(data.id, data.amount);
        }
        if (save)
            DataControl.SaveItemDataToPrefs("PlayerInventory", inventoryItemData);
    }

    /// <summary>
    /// 2. 인벤토리 데이터를 레어리티 순으로 정렬한 후, UI를 갱신합니다.
    /// </summary>
    /// <summary>
    /// 인벤토리 데이터를 레어리티 순으로 정렬한 후, UI를 갱신합니다.
    /// </summary>
    public void RefreshInventoryUI()
    {
        // 🔹 inventoryItems 리스트를 정렬: 먼저 레어리티 기준, 그 다음 Equipment 타입 순
        inventoryItemData.Sort((a, b) =>
        {
            Item itemA = ItemDatabase.Instance.FetchItemById(a.id);
            Item itemB = ItemDatabase.Instance.FetchItemById(b.id);
            if (itemA == null && itemB == null) return 0;
            if (itemA == null) return 1;
            if (itemB == null) return -1;

            int rarityComparison = itemB.Rarity.CompareTo(itemA.Rarity);
            if (rarityComparison != 0) return rarityComparison;

            Equipment equipA = itemA as Equipment;
            Equipment equipB = itemB as Equipment;
            if (equipA != null && equipB != null)
            {
                return equipA.EquipType.CompareTo(equipB.EquipType);
            }
            if (equipA != null) return -1;
            if (equipB != null) return 1;

            return 0;
        });

        ClearInventoryUI();

        foreach (ItemDataForSave data in inventoryItemData)
        {
            CreateItemUI(data.id, data.amount);
        }
    }

    /// <summary>
    /// 3새로운 슬롯 UI를 생성하고, SlotInven의 setInit()을 호출하여 아이템 데이터를 적용합니다.
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
    /// 인벤토리에서 특정 아이템을 지정한 수량만큼 제거합니다.
    /// 만약 기존 수량보다 작거나 같으면 해당 아이템 항목을 완전히 삭제합니다.
    /// 
    /// 인벤창에서 장비창으로 아이템을 옮기기 위해 만든 코드.
    /// </summary>
    /// <param name="id">제거할 아이템 ID</param>
    /// <param name="amount">제거할 수량</param>
    /// <summary>
    /// 인벤토리에서 특정 아이템을 지정한 수량만큼 제거합니다.
    /// 만약 기존 수량보다 작거나 같으면 해당 아이템 항목을 완전히 삭제합니다.
    /// </summary>
    public void RemoveItem(int id, int amount)
    {
        int index = inventoryItemData.FindIndex(x => x.id == id);
        if (index >= 0)
        {
            if (inventoryItemData[index].amount > amount)
            {
                inventoryItemData[index].amount -= amount;
            }
            else
            {
                inventoryItemData.RemoveAt(index);
            }
            Debug.Log($"{id}의 아이템이 {amount}만큼 삭제됨.");
        }
        else
        {
            Debug.LogWarning("RemoveItem: 해당 아이템이 인벤토리에 없습니다. ID: " + id);
        }
        // 인벤토리에서 사라졌으니 삭제함을 json에 저장.
        DataControl.SaveItemDataToPrefs("PlayerInventory", inventoryItemData);

        RefreshInventoryUI(); // 🔹 UI를 다시 정렬하고 갱신
    }
}