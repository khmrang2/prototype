using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    GameObject inventoryPanel;
    [SerializeField]
    GameObject slotPanel;
    [SerializeField]
    ItemDatabase database;
    [SerializeField]
    public GameObject inventorySlot;
    [SerializeField]
    public GameObject inventoryItem;

    /// <summary>
    /// �κ��丮���� ����ϰ� ������ �����۰� ���Ե�.
    /// </summary>
    int slotAmount = 16;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();


    private void Start()
    {
        //database = GetComponent<ItemDatabase>();

        slotAmount = 16;

        for(int i = 0; i < slotAmount; i++)
        {
            // ���Ը� �����ϰ�, �������� �ִ��� Ȯ��.
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        AddItem(1);
        AddItem(2);
        AddItem(3);
    }

    public void AddItem(int id)
    {
        // �������� �ٷ� �߰�. 
        Item itemToAdd = database.FetchItemById(id);
        for (int i = 0; i < items.Count; i++)
        {
            // ���Կ� �������� �����ϴ°�?
            if (items[i].Id == -1)
            { 
                // �����ϸ� id�� �ҷ��� �������� �־��ش�. �ش� ��������,
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                // �θ��� ��ġ����
                itemObj.transform.SetParent(slots[i].transform, false);
                // 0,0�� ��ġ�ϵ���.
                RectTransform rect = itemObj.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                // �̹��� ��������.
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                // �������� �̹��� �̸� ����.
                itemObj.name = itemToAdd.Tooltip;
                break;
            }
        }
    }


}
