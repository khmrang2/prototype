using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 각종변수
    /// 인벤토리 패널
    /// 슬롯 패널
    /// 
    /// 데이터베이스
    /// 
    /// 인벤토리 슬롯
    /// 인벤토리 아이템
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
    /// 인벤토리에서 사용하고 저장할 아이템과 슬롯들.
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
            // 슬롯만 생성하고, 아이템이 있는지 확인.
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
        // 아이템을 바로 추가. 
        Item itemToAdd = database.FetchItemById(id);
        for (int i = 0; i < items.Count; i++)
        {
            // 슬롯에 아이템이 존재하는가?
            if (items[i].Id == -1)
            { 
                // 존재하면 id로 불러온 아이템을 넣어준다. 해당 슬롯으로,
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                // 부모의 위치에서
                itemObj.transform.SetParent(slots[i].transform, false);
                // 0,0에 위치하도록.
                RectTransform rect = itemObj.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                // 이미지 가져오고.
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                // 툴팁으로 이미지 이름 설정.
                itemObj.name = itemToAdd.Tooltip;
                break;
            }
        }
    }


}
