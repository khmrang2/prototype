using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;
using Newtonsoft.Json.Converters;
using static UnityEditor.Progress;
using System;

/// <summary>
/// 레어리티를 표현하기 위한.
/// </summary>✅ 
public enum Rarity { Common=0, Uncommon=1, Rare=2, Epic=3, Legendary=4 }

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    public const int ID_GOLD_POT = 31;
    public const int ID_UPGRADE_ITEM = 32;
    /** 아이템 색인 0부터 30까지의 id를 가져옴.*/
    public const int RANGE_EQUIPMENT = 30;

    /** 아이템 색인 0부터 31 골드 팟까지의 ID를 가져옴.*/
    public const int RANGE_GOLD_POT = 31;

    /** 아이템 색인 0부터 32까지의 id를 가져옴.*/
    public const int RANGE_COSUMABLE = 32;


    private List<Item> database = new List<Item>();
    private Dictionary<int, Item> itemLookup = new Dictionary<int, Item>();
    private JsonData itemData;
    private const string ITEM_DATA_PATH = "/Resources/Data/itemData.json";


    void Awake()
    {
        // 싱글톤 패턴 적용: 이미 존재하는 인스턴스가 있다면 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

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

    //void ForDebugTest()
    //{
    //    int idx = 0;
    //    foreach(var item in database)
    //    {
    //        idx++;
    //        Debug.LogError($"{idx}번째 id : {item.Id} : {item.ItemName}이 있음.");
    //    }
    //}

    // json 파일에서 데이터를 읽어오는 함수.
    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            var data = itemData[i];
            string itemType = data["type"].ToString().ToLower(); // 예: "equipment", "goldpot", "consumable"
            Item item = null;

            switch (itemType)
            {
                // 장비 아이템
                case "equipment":
                    EquipmentType equipType;
                    // JSON의 "category" 필드를 EquipmentType enum으로 변환
                    if (Enum.TryParse<EquipmentType>(data["category"].ToString(), true, out equipType))
                    {
                        // stats 배열의 인덱스와 의미는 실제 데이터 구조에 맞게 수정 필요
                        item = new Equipment(
                            (int)data["id"],
                            data["itemName"].ToString(),
                            data["tooltip"].ToString(),
                            data["imgpath"].ToString(),
                            (int)data["rarity"],
                            equipType,
                            (int)data["stats"][0]["statValue"],  // 주 스탯 값
                            (int)data["stats"][1]["statValue"]   // 부 스탯 값
                        );
                    }
                    break;
                // 골드 항아리
                case "goldpot":
                    // 예시: 골드 아이템은 "goldAmount" 필드를 포함한다고 가정
                    item = new Goldpot(
                        (int)data["id"],
                        data["itemName"].ToString(),
                        data["tooltip"].ToString(),
                        data["imgpath"].ToString(),
                        (int)data["rarity"]
                    );
                    break;
                    // 업그레이드 아이템
                case "consumable":
                    item = new Consumable(
                        (int)data["id"],
                        data["itemName"].ToString(),
                        data["tooltip"].ToString(),
                        data["imgpath"].ToString(),
                        (int)data["rarity"]
                    );
                    break;

                default:
                    // 예외 처리 또는 기본 Item 객체 생성
                    break;
            }

            if (item != null)
            {
                database.Add(item);
                itemLookup[item.Id] = item;
            }
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
        int rand = Mathf.RoundToInt(UnityEngine.Random.Range(0, database.Count));
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
        return new ItemData(GetRandomItem(), Mathf.RoundToInt(UnityEngine.Random.Range(1, max_amount)));
    }
    public ItemData GetRandomItemWithAmount()
    {
        return new ItemData(GetRandomItem(), Mathf.RoundToInt(UnityEngine.Random.Range(1, 5)));
    }

}

//public struct ItemStat
//{
//    public string stat;
//    public int value;
    
//    public ItemStat(string s, int v){
//        this.stat = s;
//        this.value = v;
//    }
//}
//// 기본 아이템 클래스
//// 생성자와 반환자.
//public class Item
//{
//    // 아이템 식별자.
//    public int Id {  get; set; }        
//    // 아이템 이름.
//    public string ItemName { get; set; }
//    // 아이템 툴팁.
//    public string Tooltip { get; set; } 
//    // 아이템 이미지 경로
//    public string ImgPath { get; set; } 
//    // 아이템 스프라이트
//    public Sprite Sprite { get; set; }  
//    // 아이템 희귀도. (0: 커먼, 1: 언커먼, 2: 레어, 3: 에픽, 4: 레전드리)
//    public Rarity Rarity { get; set; }     

//    public Item(int id, string itemName, string toolTip, string path, int rarity)
//    {
//        this.Id = id;
//        this.ItemName = itemName;
//        this.Tooltip = toolTip;
//        this.ImgPath = path;
//        this.Sprite = Resources.Load<Sprite>("Image/Items/" + path);
//        this.Rarity = (Rarity)rarity;
//    }

//    /// <summary>
//    /// 최초 생성자. 아이템이 생성되지 않았으므로 id = -1
//    /// </summary>
//    public Item()
//    {
//        this.Id = -1;
//    }
//}

/// <summary>
/// 아이템 하위 : 개별 아이템을 불러오기 쉽게 만든 ItemData, amount를 저장.
/// </summary>
[System.Serializable]
public class ItemData
{
    public Item item;   // 아이템 고유 ID
    public int amount;  // 아이템 수량

    public ItemData(Item item, int a)
    {
        this.item = item;
        this.amount = a;
    }
}
/// <summary>
/// 아이템 하위 : 개별 아이템을 id로 저장하면 너무 커지니까, id와 amount만 저장.
/// </summary>
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

/// <summary>
/// 기본 아이템 클래스. 추상클래스임.
/// </summary>
public abstract class Item
{
    // 아이템 식별자.
    public int Id { get; set; }
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

    public Item(int id, string itemName, string toolTip, string path, int rarity)
    {
        this.Id = id;
        this.ItemName = itemName;
        this.Tooltip = toolTip;
        this.ImgPath = path;
        this.Sprite = Resources.Load<Sprite>("Image/Items/" + path);
        this.Rarity = (Rarity)rarity;
    }

    public Item()
    {
        this.Id = -1;
    }
}

// 아이템 하위 Equipment 클래스 (weapon, heart, gear 로 구분)
public class Equipment : Item
{
    public EquipmentType EquipType { get; }
    public int MainStatValue { get; }
    public int SubStatValue { get; }

    public Equipment(int id, string name, string tooltip, string imgPath, int rarity,
                     EquipmentType equipType, int mainStatValue, int subStatValue)
        : base(id, name, tooltip, imgPath, rarity)
    {
        EquipType = equipType;
        MainStatValue = mainStatValue;
        SubStatValue = subStatValue;
    }

}

// Equipment의 범주를 나타내는 열거형
public enum EquipmentType
{
    Weapon,
    Heart,
    Gear
}

// Goldpot 아이템 클래스
public class Goldpot : Item
{
    public int GoldAmount { get; }

    public Goldpot(int id, string name, string tooltip, string imgPath, int rarity)
        : base(id, name, tooltip, imgPath, rarity)
    {

    }
}

// 소모성 아이템 클래스
public class Consumable : Item
{
    // 추가 필드가 있다면 선언
    public Consumable(int id, string name, string tooltip, string imgPath, int rarity)
        : base(id, name, tooltip, imgPath, rarity)
    {

    }
}