using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    /// <summary>
    /// 데이터에서 획득한 아이템을 기반으로 데이터를 로드하고 불러온다.
    /// </summary>
    private void Start()
    {
        //획득한 아이템을 기반으로 데이터를 로드하고 불러오는 코드를 작성.
        //database.dataload();
        //database = GetComponent<ItemDatabase>();

        slotAmount = 16;

        for(int i = 0; i < slotAmount; i++)
        {
            // 빈 아이템 슬롯을 생성.
            slots.Add(Instantiate(inventorySlot));
            // 빈 아이템 리스트에 생성.
            items.Add(new Item());
            // 생성한 아이템 슬롯을 뷰어 오브젝트(contents)의 자식으로 할당.
            slots[i].transform.SetParent(slotPanel.transform, false);
        }
        AddItem(1, 0);
        //AddItem(2);
        //AddItem(3);
    }

    /// <summary>
    /// 아이템을 인벤토리에 추가하는 코드.
    /// </summary>
    /// <param name="id"></param>
    public void AddItem(int id, int amount)
    {
        // 아이템 코드 데이터베이스에서 id를 통해 아이템을 넣는다.
        // 즉, 아이템 코드로 우리는 아이템들을 불러올 수 있다.
        Item itemToAdd = database.FetchItemById(id);
        for (int i = 0; i < items.Count; i++)
        {
            // 슬롯이 비어있다면(해당 슬롯에 아이템이 없는 상태)
            if (items[i].Id == -1)
            { 
                // 아이템 리스트에 넣고,
                items[i] = itemToAdd;
                // 프리팹으로 아이템을 생성.
                GameObject itemObj = Instantiate(inventoryItem);

                // 아이템 슬롯을 부모로 하여 생성.
                itemObj.transform.SetParent(slots[i].transform, false);
                // 앵커 오류로 인해서 넣은 오류 수정 코드(상동한 코드다.)
                RectTransform rect = itemObj.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                // 이미지 가져오고.
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                // 툴팁으로 오브젝트의 이름을 설정.
                itemObj.name = itemToAdd.Tooltip;
                // 그다음 오브젝트에서 양도 가져온다.
                itemObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "12";
                break;
            }
        }
    }

    /// <summary>
    /// 인벤토리를 레어도 순으로 정렬하는 코드.
    /// </summary>
    public void sortInventory()
    {
        return;
    }


}
