using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.CompilerServices;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;
    // Start is called before the first frame update
    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/itemData.json"));
        ConstructItemDatabase();
    }

    // json 파일에서 데이터를 읽어오는 함수.
    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["tooltip"].ToString(), itemData[i]["imgpath"].ToString(), itemData[i]["rarity"]));
        }
        Debug.Log(database.Count);
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


// 기본 아이템 클래스
// 생성자와 반환자.
public class Item
{
    public int Id {  get; set; }        // 아이템 식별자.
    public string Tooltip { get; set; } // 아이템 툴팁.
    public string ImgPath { get; set; } // 아이템 이미지 경로
    public Sprite Sprite { get; set; }  // 아이템 스프라이트

    public int rarity { get; set; }     // 아이템 희귀도.

    public Item(int id, string tooltip, string path, int rarity)
    {
        this.Id = id;
        this.Tooltip = tooltip;
        this.ImgPath = path;
        this.Sprite = Resources.Load<Sprite>("Image/Items/" + path);
        this.rarity = rarity;
    }

    /// <summary>
    /// 최초 생성자. 아이템이 생성되지 않았으므로 id = -1
    /// </summary>
    public Item()
    {
        this.Id = -1;
    }
}