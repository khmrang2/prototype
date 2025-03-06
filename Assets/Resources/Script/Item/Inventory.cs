using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 개별 아이템이 저장되기위한 인벤토리 슬롯에 대한 저장구조
/// </summary>
[System.Serializable]
public class ItemData
{
    public int id;      // 아이템 고유 ID
    public int amount;  // 아이템 수량
}
/// <summary>
/// 아이템들을 하나의 인벤토리 저장 구조로 관리하기 위한 리스트.
/// </summary>
[System.Serializable]
public class InventoryData
{
    public List<ItemData> items = new List<ItemData>();
}

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
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    /// <summary>
    /// 인벤토리에서 사용하고 저장할 아이템과 슬롯들.
    /// </summary>
    public List<GameObject> slots = new List<GameObject>();
    public List<ItemData> inventory = new List<ItemData>();

    /// <summary>
    /// 데이터에서 획득한 아이템을 기반으로 데이터를 로드하고 불러온다.
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
    /// 아이템을 인벤토리에 추가하는 코드.
    /// </summary>
    /// <param name="id"></param>
    public void AddItem(int id, int amount)
    {
        makeItemUIToInventory(id, amount);

        // 이제 데이터에 실제로 넣는다. 
        ItemData item = new ItemData();
        item.id = id;
        item.amount = amount;
        inventory.Add(item);

    }
    /// <summary>
    /// 인벤토리 UI에 아이템을 보이게 하는 그래픽적인 코드.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    public void makeItemUIToInventory(int id, int amount)
    {
        // 아이템 코드 데이터베이스에서 id를 통해 아이템을 넣는다.
        // 즉, 아이템 코드로 우리는 아이템들을 불러올 수 있다.
        Item itemToAdd = database.FetchItemById(id);

        // 슬롯에 생성.
        GameObject slot = Instantiate(inventorySlot);
        slot.transform.SetParent(slotPanel.transform, false);
        slots.Add(slot);
        // 인벤토리 아이템 UI 생성 후 슬롯에 배치.
        GameObject itemObj = Instantiate(inventoryItem);
        itemObj.transform.SetParent(slot.transform, false);
        RectTransform rect = itemObj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        // 이미지 가져오고.
        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
        // 툴팁으로 오브젝트의 이름을 설정.
        itemObj.name = itemToAdd.Tooltip;
        // 그다음 오브젝트에서 양도 가져온다.
        itemObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    /// <summary>
    /// 인벤토리를 레어도 순으로 정렬하는 코드.
    /// </summary>
    public void sortInventory()
    {
        return;
    }

    /// <summary>
    /// 인벤토리의 상태를 저장하고, 불러오는 메소드
    /// </summary>
    public void saveToJson()
    {
        // 저장하기 위한 data 생성.
        InventoryData data = new InventoryData();
        // 현재 가지고 있는 inv를 가져오고,
        data.items = inventory;
        string json = JsonUtility.ToJson(data, true);
        // Resource는 읽기 전용이기 때문에, Application.persistentdatapath == /data/data/[패키지명]/inventoryData.json에 저장됨.
        string path = Application.persistentDataPath + "/inventoryData.json";
        File.WriteAllText(path, json);
        //Debug.Log("인벤토리의 현재 상태가 저장되었습니다." + path);
        return; 
    }

    public void loadFromJson()
    {
        string path = Application.persistentDataPath + "/inventoryData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            // 저장한 데이터를 불러오기 위한 초기화 작업.
            foreach(GameObject slot in slots)
            {
                // 그래픽 초기화
                Destroy(slot);
            }
            // 데이터 초기화
            slots.Clear();
            inventory.Clear();

            foreach(ItemData item in data.items)
            {
                AddItem(item.id, item.amount);
            }
        }
        else
        {
            Debug.Log("저장된 인벤토리 파일이 존재하지 않습니다.");
        }
    }
}
