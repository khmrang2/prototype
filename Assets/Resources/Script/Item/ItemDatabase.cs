using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;
using Newtonsoft.Json.Converters;
using static UnityEditor.Progress;

/// <summary>
/// 레어리티를 표현하기 위한.
/// </summary>✅ 
public enum Rarity { Common=0, Uncommon=1, Rare=2, Epic=3, Legendary=4 }

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private Dictionary<int, Item> itemLookup = new Dictionary<int, Item>();
    private JsonData itemData;
    private const string ITEM_DATA_PATH = "/Resources/Data/itemData.json";


    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/itemData");
        if (jsonFile != null)
        {
            itemData = JsonMapper.ToObject(jsonFile.text);
            ConstructItemDatabase();
        }
        else
        {
            Debug.LogError("itemData.json 파일을 찾을 수 없습니다.");
        }
    }

    // json 파일에서 데이터를 읽어오는 함수.
    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            var item = new Item(
                            (int)itemData[i]["id"],                         // id
                            itemData[i]["itemName"].ToString(),             // name
                            itemData[i]["tooltip"].ToString(),              // tooltip
                            itemData[i]["imgpath"].ToString(),              // imgpath
                            (int)itemData[i]["rarity"],                     // rarity
                            itemData[i]["stats"][0]["statName"].ToString(), // 주스텟 이름
                            (int)itemData[i]["stats"][1]["statValue"],      // 주스텟 벨류
                            itemData[i]["stats"][1]["statName"].ToString(), // 부스텟 이름
                            (int)itemData[i]["stats"][1]["statValue"]       // 부스텟 벨류
                        );
            database.Add(item);
            itemLookup[item.Id] = item;
        }
    }

    // 매개변수인 id를 통해서 아이템을 반환하는 아이템 반환자.
    public Item FetchItemById(int id)
    {
        return itemLookup.TryGetValue(id, out var item) ? item : null;
    }

    /// <summary>
    /// 랜덤으로 아이템을 반환하는 아이템 반환자.
    /// </summary>
    /// <returns>Item 랜덤 아이템</returns>
    public Item GetRandomItem()
    {
        int rand = Mathf.RoundToInt(Random.Range(1, database.Count));
        return database[rand];
    }

    /// <summary>
    /// 랜덤으로 ItemData(아이템과 양)을 반환하는 아이템 반환자.
    /// 
    /// param을 넣지 않으면 기본으로 0 ~ 5로 반환합니다.
    /// </summary>
    /// <param name="max_amount">아이템의 최대 수량을 return 합니다.</param>
    /// <returns>ItemData 랜덤 아이템과 양.</returns>
    public ItemData GetRandomItemWithAmount(int max_amount)
    {
        return new ItemData(GetRandomItem(), Mathf.RoundToInt(Random.Range(1, max_amount)));
    }
    public ItemData GetRandomItemWithAmount()
    {
        return new ItemData(GetRandomItem(), Mathf.RoundToInt(Random.Range(1, 5)));
    }

}

public struct ItemStat
{
    public string stat;
    public int value;
    
    public ItemStat(string s, int v){
        this.stat = s;
        this.value = v;
    }
}
// 기본 아이템 클래스
// 생성자와 반환자.
public class Item
{
    // 아이템 식별자.
    public int Id {  get; set; }        
    // 아이템 이름.
    public string ItemName { get; set; }
    // 아이템 툴팁.
    public string Tooltip { get; set; } 
    // 아이템 이미지 경로
    public string ImgPath { get; set; } 
    // 아이템 스프라이트
    public Sprite Sprite { get; set; }  
    // 아이템 희귀도. (0: 커먼, 1: 언커먼, 2: 레어, 3: 에픽, 4: 레전드리)
    public Rarity Rarity { get; set; }     

    public ItemStat PrimaryStat { get; set; }
    public ItemStat SecondStat { get; set; }


    public Item(int id, string itemName, string toolTip, string path, int rarity, string primaryStatName, int primaryStatValue, string secondStatName, int secondStatValue)
    {
        this.Id = id;
        this.ItemName = itemName;
        this.Tooltip = toolTip;
        this.ImgPath = path;
        this.Sprite = Resources.Load<Sprite>("Image/Items/" + path);
        this.Rarity = (Rarity)rarity;
        this.PrimaryStat = new ItemStat(primaryStatName, primaryStatValue);
        this.SecondStat = new ItemStat(secondStatName, secondStatValue);
    }

    /// <summary>
    /// 최초 생성자. 아이템이 생성되지 않았으므로 id = -1
    /// </summary>
    public Item()
    {
        this.Id = -1;
    }
}

/// <summary>
/// 개별 아이템이 저장되기위한 인벤토리 슬롯에 대한 저장구조
/// </summary>
[System.Serializable]
public class ItemData
{
    public Item item;      // 아이템 고유 ID
    public int amount;  // 아이템 수량

    public ItemData(Item item, int a)
    {
        this.item = item;
        this.amount = a;
    }
}
[System.Serializable]
public class ItemDataForSave
{
    public int id;      // 아이템 고유 ID
    public int amount;  // 아이템 수량

    public ItemDataForSave(int i, int a)
    {
        this.id = i;
        this.amount = a;
    }
}