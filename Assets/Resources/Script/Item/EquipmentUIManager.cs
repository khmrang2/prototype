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
        if(!payGold(isads, 900)) return;
        //UI 패널 활성화
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(true);

        List<Item> rewards = gacha(10);
        
        string rewardLog = "🎁 gacha(10) 결과: ";
        foreach (Item item in rewards) { 
            if (item != null) 
                rewardLog += $"[ID: {item.Id}, Name: {item.ItemName}, Rarity: {item.Rarity}] ";
            else rewardLog += "[NULL ITEM] ";
        }
        Debug.Log(rewardLog);
        equipmentPanel.ShowMultipleEquipments(rewards);
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
    
    public void DrawRandom1Reward(bool isads)
    {
        if(!payGold(isads, 100)) return;
        // ✅ UI 패널 활성화
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(true);

        float roll = Random.Range(0f, 100f);

        if (roll < 40f) // 40% 확률로 장비 지급
        {
            // ✅ 장비 1개 뽑기
            List<Item> equipmentList = new List<Item>(1);
            equipmentList.Add(itemDatabase.GetRandomItem());
            if (equipmentList.Count > 0)
            {
                equipmentPanel.ShowSingleEquipment(equipmentList[0]); // ✅ 1개 UI 표시
            }
        }
        else
        {
            // ✅ 60% 확률로 골드 지급 (50G ~ 110G)
            int goldAmount = Random.Range(50, 111);
            equipmentPanel.ShowGoldReward(goldAmount);
            payGold(false, -goldAmount);
        }
    }

    private bool payGold(bool isAds, int amount)
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
    }
}
