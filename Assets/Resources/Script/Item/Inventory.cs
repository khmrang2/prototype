using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���� �������� ����Ǳ����� �κ��丮 ���Կ� ���� ���屸��
/// </summary>
[System.Serializable]
public class ItemData
{
    public int id;      // ������ ���� ID
    public int amount;  // ������ ����
}
/// <summary>
/// �����۵��� �ϳ��� �κ��丮 ���� ������ �����ϱ� ���� ����Ʈ.
/// </summary>
[System.Serializable]
public class InventoryData
{
    public List<ItemData> items = new List<ItemData>();
}

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// �κ��丮 �г�
    /// ���� �г�
    /// 
    /// �����ͺ��̽�
    /// 
    /// �κ��丮 ����
    /// �κ��丮 ������
    /// </summary>
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    /// <summary>
    /// �κ��丮���� ����ϰ� ������ �����۰� ���Ե�.
    /// </summary>
    public List<GameObject> slots = new List<GameObject>();
    public List<ItemData> inventory = new List<ItemData>();

    /// <summary>
    /// �����Ϳ��� ȹ���� �������� ������� �����͸� �ε��ϰ� �ҷ��´�.
    /// </summary>
    private void Start()
    {

    }

    public void fordebugingRandomItemAdd()
    {
        AddItem(Mathf.RoundToInt(Random.Range(1,10)), Mathf.RoundToInt(Random.Range(1, 100)));
    }
    public void fordebugingSave()
    {
        saveToJson();
    }
    public void fordebugingLoad()
    {
        loadFromJson();
    }
    /// <summary>
    /// �������� �κ��丮�� �߰��ϴ� �ڵ�.
    /// </summary>
    /// <param name="id"></param>
    public void AddItem(int id, int amount)
    {
        // ������ �ڵ� �����ͺ��̽����� id�� ���� �������� �ִ´�.
        // ��, ������ �ڵ�� �츮�� �����۵��� �ҷ��� �� �ִ�.
        Item itemToAdd = database.FetchItemById(id);
        
        // ���Կ� ����.
        GameObject slot = Instantiate(inventorySlot);
        slot.transform.SetParent(slotPanel.transform, false);
        slots.Add(slot);

        // �κ��丮 ������ UI ���� �� ���Կ� ��ġ.
        GameObject itemObj = Instantiate(inventoryItem);
        itemObj.transform.SetParent(slot.transform, false);
        RectTransform rect = itemObj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        // �̹��� ��������.
        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
        // �������� ������Ʈ�� �̸��� ����.
        itemObj.name = itemToAdd.Tooltip;
        // �״��� ������Ʈ���� �絵 �����´�.
        itemObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.ToString();

        // ���� �����Ϳ� ������ �ִ´�. 
        ItemData item = new ItemData();
        item.id = id;
        item.amount = amount;
        inventory.Add(item);

    }

    /// <summary>
    /// �κ��丮�� ��� ������ �����ϴ� �ڵ�.
    /// </summary>
    public void sortInventory()
    {
        return;
    }

    /// <summary>
    /// �κ��丮�� ���¸� �����ϰ�, �ҷ����� �޼ҵ�
    /// </summary>
    public void saveToJson()
    {
        // �����ϱ� ���� data ����.
        InventoryData data = new InventoryData();
        // ���� ������ �ִ� inv�� ��������,
        data.items = inventory;
        string json = JsonUtility.ToJson(data, true);
        // Resource�� �б� �����̱� ������, Application.persistentdatapath == /data/data/[��Ű����]/inventoryData.json�� �����.
        string path = Application.persistentDataPath + "/inventoryData.json";
        File.WriteAllText(path, json);
        //Debug.Log("�κ��丮�� ���� ���°� ����Ǿ����ϴ�." + path);
        return; 
    }

    public void loadFromJson()
    {
        string path = Application.persistentDataPath + "/inventoryData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            // ������ �����͸� �ҷ����� ���� �ʱ�ȭ �۾�.
            foreach(GameObject slot in slots)
            {
                // �׷��� �ʱ�ȭ
                Destroy(slot);
            }
            // ������ �ʱ�ȭ
            slots.Clear();
            inventory.Clear();

            foreach(ItemData item in data.items)
            {
                AddItem(item.id, item.amount);
            }
        }
        else
        {
            Debug.Log("����� �κ��丮 ������ �������� �ʽ��ϴ�.");
        }
    }
}
