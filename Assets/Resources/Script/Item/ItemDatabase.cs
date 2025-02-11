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
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["tooltip"].ToString(), itemData[i]["imgpath"].ToString()));
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
    public int Id {  get; set; }
    public string Tooltip { get; set; }
    public string ImgPath { get; set; }
    public Sprite Sprite { get; set; }

    public Item(int id, string tooltip, string path)
    {
        this.Id = id;
        this.Tooltip = tooltip;
        this.ImgPath = path;
        this.Sprite = Resources.Load<Sprite>("Image/Items/" + path);
    }

    public Item()
    {
        this.Id = -1;
    }
}