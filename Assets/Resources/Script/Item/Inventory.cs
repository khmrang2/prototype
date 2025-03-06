using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    /// <summary>
    /// �����Ϳ��� ȹ���� �������� ������� �����͸� �ε��ϰ� �ҷ��´�.
    /// </summary>
    private void Start()
    {
        //ȹ���� �������� ������� �����͸� �ε��ϰ� �ҷ����� �ڵ带 �ۼ�.
        //database.dataload();
        //database = GetComponent<ItemDatabase>();

        slotAmount = 16;

        for(int i = 0; i < slotAmount; i++)
        {
            // �� ������ ������ ����.
            slots.Add(Instantiate(inventorySlot));
            // �� ������ ����Ʈ�� ����.
            items.Add(new Item());
            // ������ ������ ������ ��� ������Ʈ(contents)�� �ڽ����� �Ҵ�.
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        AddItem(1, 0);
        //AddItem(2);
        //AddItem(3);
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
        for (int i = 0; i < items.Count; i++)
        {
            // ������ ����ִٸ�(�ش� ���Կ� �������� ���� ����)
            if (items[i].Id == -1)
            { 
                // ������ ����Ʈ�� �ְ�,
                items[i] = itemToAdd;
                // ���������� �������� ����.
                GameObject itemObj = Instantiate(inventoryItem);

                // ������ ������ �θ�� �Ͽ� ����.
                itemObj.transform.SetParent(slots[i].transform, false);
                // ��Ŀ ������ ���ؼ� ���� ���� ���� �ڵ�(���� �ڵ��.)
                RectTransform rect = itemObj.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                // �̹��� ��������.
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                // �������� ������Ʈ�� �̸��� ����.
                itemObj.name = itemToAdd.Tooltip;
                // �״��� ������Ʈ���� �絵 �����´�.
                itemObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "12";
                break;
            }
        }
    }

    /// <summary>
    /// �κ��丮�� ��� ������ �����ϴ� �ڵ�.
    /// </summary>
    public void sortInventory()
    {
        return;
    }


}
