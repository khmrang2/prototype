using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentUIManager : MonoBehaviour
{
    public EquipmentUIPanel equipmentPanel; // UI 패널 (Inspector에서 연결)
    public Button draw_10_rewardsButton; // 뽑기 버튼 (Inspector에서 연결)
    public Button draw_1_rewardButton; // 단일 뽑기 버튼
    public Button close_Button; // 닫기 버튼 (Inspector에서 연결)

    void Start()
    {
        //올바른 UI 패널 비활성화 방식
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(false);

        //버튼 이벤트 연결 (람다식 사용하여 매개변수 전달)
        if (draw_10_rewardsButton != null)
        {
            draw_10_rewardsButton.onClick.AddListener(() => DrawRandom10Rewards(false));
        }
        if (draw_1_rewardButton != null)
        {
            draw_1_rewardButton.onClick.AddListener(() => DrawRandom1Reward(false));
        }
        if (close_Button != null)
        {
            close_Button.onClick.AddListener(() => closePanel());
        }
    }

    public void DrawRandom10Rewards(bool isads)
    {
        if(!payGold(900, isads)) return;
        //UI 패널 활성화
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(true);
        
        equipmentPanel.ShowMultipleEquipments(gacha(10));
        addEquipmentToInventory();
    }
    
    public void DrawRandom1Reward(bool isads)
    {
        if(!payGold(100, isads)) return;
        // ✅ UI 패널 활성화
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(true);
        
        equipmentPanel.ShowSingleEquipment(gacha(1)); // ✅ 1개 UI 표시
        addEquipmentToInventory();
    }

    private bool payGold(int amount, bool isAds=false)
    {
        if (PlayerStatusInMain.Instance == null)
        {
            Debug.LogError("🚨 PlayerStateInMain.Instance가 null입니다! 초기화 확인 필요");
            return false;
        }
        if (isAds) return true;
        else
        {
            return PlayerStatusInMain.Instance.payGold(amount);
        }
    }

    public void closePanel()
    {
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        //int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold + equipmentPanel.GetEarnedGold()).ToString());
        //DataControl.SaveEncryptedDataToPrefs("UpgradeStone", (currentUpgradeStone + equipmentPanel.GetEarnedUpgradeStone()).ToString());
    }

    /// <summary>
    /// handItem을 인벤토리에 넘겨줘야함.
    /// </summary>
    /// <param name="handItem"></param>
    public void addEquipmentToInventory()
    {
        Inventory.Instance.AddOrUpdateItems(equipmentPanel.getItemDatas());
    }
    
    /// <summary>
    /// param : gacha_count만큼 랜덤 뽑기를 시행.
    /// </summary>
    /// <param name="gacha_count"></param>
    /// <returns></returns>
    private List<Item> gacha(int gacha_count)
    {
        List<Item> equipmentList = new List<Item>(gacha_count);
        for (int i = 0; i < gacha_count; i++)
        {
            float roll = Random.Range(0f, 100f);
            if (roll < 40)
            {
                // 40% 장비
                equipmentList.Add(ItemDatabase.Instance.GetRandomItem());
            }else if(40 < roll && roll <= 70){
                // 30% 업그레이드 아이템.
                equipmentList.Add(ItemDatabase.Instance.FetchItemById(ItemDatabase.ID_UPGRADE_ITEM));
            }
            else
            {
                // 30% 골드.
                equipmentList.Add(ItemDatabase.Instance.FetchItemById(ItemDatabase.ID_GOLD_POT));              
            }
        }

        return equipmentList;
    }
}
