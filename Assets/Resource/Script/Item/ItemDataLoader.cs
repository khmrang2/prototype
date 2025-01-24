using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string id;       // ������ ID
    public string tooltip;  // ����
}

public class ItemDataLoader : MonoBehaviour
{
    private Dictionary<string, ItemData> itemDataDictionary;

    private void Awake()
    {
        LoadItemData("Assets/Resource/Data/itemData.json"); // JSON ��� ����
    }

    // JSON ���� �ε�
    private void LoadItemData(string filePath)
    {
        string json = File.ReadAllText(filePath);
        List<ItemData> itemList = JsonConvert.DeserializeObject<List<ItemData>>(json);

        itemDataDictionary = new Dictionary<string, ItemData>();
        foreach (var item in itemList)
        {
            itemDataDictionary[item.id] = item;
        }

        Debug.Log("ItemData loaded successfully.");
    }
    // ���� ��ȯ
    public string GetItemTooltip(string itemID)
    {
        if (itemDataDictionary.TryGetValue(itemID, out var itemData))
        {
            return itemData.tooltip;
        }

        Debug.LogError($"ItemID '{itemID}'�� �ش��ϴ� �����͸� ã�� �� �����ϴ�.");
        return "No tooltip available.";
    }
}
