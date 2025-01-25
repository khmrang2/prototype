using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string id;       // 아이템 ID
    public string tooltip;  // 툴팁
}

public class ItemDataLoader : MonoBehaviour
{
    private Dictionary<string, ItemData> itemDataDictionary;

    private void Awake()
    {
        LoadItemData("Assets/Resource/Data/itemData.json"); // JSON 경로 설정
    }

    // JSON 파일 로드
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
    // 툴팁 반환
    public string GetItemTooltip(string itemID)
    {
        if (itemDataDictionary.TryGetValue(itemID, out var itemData))
        {
            return itemData.tooltip;
        }

        Debug.LogError($"ItemID '{itemID}'에 해당하는 데이터를 찾을 수 없습니다.");
        return "No tooltip available.";
    }
}
