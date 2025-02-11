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

    // json ���Ͽ��� �����͸� �о���� �Լ�.
    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["tooltip"].ToString(), itemData[i]["imgpath"].ToString()));
        }
        Debug.Log(database.Count);
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


// �⺻ ������ Ŭ����
// �����ڿ� ��ȯ��.
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