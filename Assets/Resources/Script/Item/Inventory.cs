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
    /// 인벤토리에 아이템을 추가하거나, 이미 존재하는 경우 수량을 업데이트합니다.
    /// </summary>
    /// <param name="id">아이템 ID</param>
    /// <param name="amount">추가할 수량</param>
    public void AddOrUpdateItem(int id, int amount)
    {
        // 이미 같은 아이템이 인벤토리에 있는지 검색
        int index = inventoryItems.FindIndex(x => x.id == id);
        if (index >= 0)
        {
            // 이미 있으면 수량 업데이트
            inventoryItems[index].amount += amount;
            // UI 슬롯 업데이트 (슬롯 인덱스와 데이터 인덱스가 일치한다고 가정)
            if (index < slots.Count)
            {
                GameObject slot = slots[index];
                TextMeshProUGUI qtyText = slot.GetComponentInChildren<TextMeshProUGUI>();
                if (qtyText != null)
                {
                    qtyText.text = inventoryItems[index].amount.ToString();
                }
            }
        }
        else
        {
            // 없으면 새로 추가
            CreateItemUI(id, amount);
            inventoryItems.Add(new ItemDataForSave(id, amount));
        }
    }

    /// <summary>
    /// 외부에서 상점 등에서 전달받은 아이템 리스트를 인벤토리에 추가합니다.
    /// </summary>
    public void AddOrUpdateItems(List<ItemDataForSave> handItems)
    {
        foreach (ItemDataForSave data in handItems)
        {
            AddOrUpdateItem(data.id, data.amount);
        }
    }

    /// <summary>
    /// 새 슬롯과 아이템 UI를 생성합니다.
    /// </summary>
    private void CreateItemUI(int id, int amount)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemById(id);
        if (itemToAdd == null)
        {
            Debug.LogWarning("해당 아이템 ID를 찾을 수 없습니다: " + id);
            return;
        }

        GameObject slot = Instantiate(inventorySlotPrefab);
        slot.transform.SetParent(slotPanel.transform, false);
        SlotInven slotInven = slot.GetComponent<SlotInven>();
        slotInven.setInit(new ItemData(itemToAdd, amount));
        slots.Add(slot);

    }

    #region JSON 저장/로드
    public void SaveInventory()
    {
        InventoryData data = new InventoryData { items = inventoryItems };
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + SAVE_FILE;
        File.WriteAllText(path, json);
        Debug.Log("인벤토리 저장됨: " + path);
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + SAVE_FILE;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            // 초기화: 기존 슬롯과 데이터 삭제
            foreach (GameObject slot in slots)
                Destroy(slot);
            slots.Clear();
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
    #endregion

    public void fordebugingRandomItemAdd()
    {
        AddOrUpdateItem(Mathf.RoundToInt(Random.Range(1, 30)), Mathf.RoundToInt(Random.Range(1, 100)));
    }
    public void fordebugingSave()
    {
        SaveInventory();
    }
    public void fordebugingLoad()
    {
        LoadInventory();
    }
}
