using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentUIManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public EquipmentUIPanel equipmentPanel; // UI 패널 (Inspector에서 연결)
    public Button draw_10_rewardsButton; // 뽑기 버튼 (Inspector에서 연결)
    public Button draw_1_rewardButton; // 단일 뽑기 버튼

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
    }

    public void DrawRandom10Rewards(bool isads)
    {
        if(!payGold(900, isads)) return;
        //UI 패널 활성화
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(true);
        
        equipmentPanel.ShowMultipleEquipments(gacha(10));
    }
    
    public void DrawRandom1Reward(bool isads)
    {
        if(!payGold(100, isads)) return;
        // ✅ UI 패널 활성화
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(true);
        
        equipmentPanel.ShowSingleEquipment(gacha(1)); // ✅ 1개 UI 표시
    }

    private bool payGold(int amount, bool isAds=false)
    {
        if (isAds) return true;
        else
        {
            int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
            if (currentGold < amount) return false;
            DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold - amount).ToString());
            return true;
        }
    }

    public void closePanel()
    {
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold + equipmentPanel.GetEarnedGold()).ToString());
    }
    
    private List<Item> gacha(int gacha_count)
    {
        List<Item> equipmentList = new List<Item>(gacha_count);
        for (int i = 0; i < gacha_count; i++)
        {
            float roll = Random.Range(0f, 100f);
            if (roll < 40)
            {
                equipmentList.Add(itemDatabase.GetRandomItem());
            }
            else
            {
                equipmentList.Add(itemDatabase.FetchItemById(30));              
            }
        }

        return equipmentList;
    }
}
