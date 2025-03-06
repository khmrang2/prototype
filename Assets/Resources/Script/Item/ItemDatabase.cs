using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;

/// <summary>
/// ���Ƽ�� ǥ���ϱ� ����.
/// </summary>
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

    // json ���Ͽ��� �����͸� �о���� �Լ�.
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
                            itemData[i]["stats"][0]["statName"].ToString(), // �ֽ��� �̸�
                            (int)itemData[i]["stats"][1]["statValue"],      // �ֽ��� ����
                            itemData[i]["stats"][1]["statName"].ToString(), // �ν��� �̸�
                            (int)itemData[i]["stats"][1]["statValue"]       // �ν��� ����
                        ));
        }
    }

    // �Ű������� id�� ���ؼ� �������� ��ȯ�ϴ� ������ ��ȯ��.
    public Item FetchItemById(int id)
    {
        //Debug.Log("�� ������ �ǳ�?");
        for (int i = 0; i < database.Count; i++)
        {
            //Debug.Log($"üũ ��: database[{i}].Id = {database[i].Id}");
            if (database[i].Id == id)
            {
                return database[i];
            }
        }
        //Debug.LogError($"ID {id}�� �ش��ϴ� �������� ã�� �� ����!");
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
// �⺻ ������ Ŭ����
// �����ڿ� ��ȯ��.
public class Item
{
    // ������ �ĺ���.
    public int Id {  get; set; }        
    // ������ �̸�.
    public string ItemName { get; set; }
    // ������ ����.
    public string Tooltip { get; set; } 
    // ������ �̹��� ���
    public string ImgPath { get; set; } 
    // ������ ��������Ʈ
    public Sprite Sprite { get; set; }  
    // ������ ��͵�. (0: Ŀ��, 1: ��Ŀ��, 2: ����, 3: ����, 4: �����帮)
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
    /// ���� ������. �������� �������� �ʾ����Ƿ� id = -1
    /// </summary>
    public Item()
    {
        this.Id = -1;
    }
}