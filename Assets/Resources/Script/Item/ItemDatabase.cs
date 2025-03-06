using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;

/// <summary>
/// 레어리티를 표현하기 위한.
/// </summary>✅ 
public enum Rarity { Common=0, Uncommon=1, Rare=2, Epic=3, Legendary=4 }

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;
    private const string ITEM_DATA_PATH = "/Resources/Data/itemData.json";
    // Start is called before the first frame update
    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + ITEM_DATA_PATH));
        ConstructItemDatabase();
    }

    // json 파일에서 데이터를 읽어오는 함수.
    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item(
                            (int)itemData[i]["id"],                         // id
                            itemData[i]["itemName"].ToString(),             // name
                            itemData[i]["tooltip"].ToString(),              // tooltip
                            itemData[i]["imgpath"].ToString(),              // imgpath
                            (int)itemData[i]["rarity"],                     // rarity
                            itemData[i]["stats"][0]["statName"].ToString(), // 주스텟 이름
                            (int)itemData[i]["stats"][1]["statValue"],      // 주스텟 벨류
                            itemData[i]["stats"][1]["statName"].ToString(), // 부스텟 이름
                            (int)itemData[i]["stats"][1]["statValue"]       // 부스텟 벨류
                        ));
        }
    }

    // 매개변수인 id를 통해서 아이템을 반환하는 아이템 반환자.
    public Item FetchItemById(int id)
    {
        //Debug.Log("너 실행은 되냐?");
        for (int i = 0; i < database.Count; i++)
        {
            //Debug.Log($"체크 중: database[{i}].Id = {database[i].Id}");
            if (database[i].Id == id)
            {
                return database[i];
            }
        }
        //Debug.LogError($"ID {id}에 해당하는 아이템을 찾을 수 없음!");
        return null;
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
        this.ItemName = ItemName;
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