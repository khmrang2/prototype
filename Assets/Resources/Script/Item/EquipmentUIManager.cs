using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentUIManager : MonoBehaviour
{
    public EquipmentUIPanel equipmentPanel; // UI 패널 (Inspector에서 연결)
    public Button draw_10_rewardsButton; // 뽑기 버튼 (Inspector에서 연결)
    public Button draw_1_rewardButton; // 단일 뽑기 버튼

    void Start()
    {
        // ✅ 올바른 UI 패널 비활성화 방식
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(false);

        // ✅ 버튼 이벤트 연결 (람다식 사용하여 매개변수 전달)
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
        payGold(isads, -900);
        // ✅ UI 패널 활성화
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(true);

        float roll = Random.Range(0f, 100f);

        if (roll < 40f) // 40% 확률로 장비 지급
        {
            // ✅ 장비 10개 뽑기
            List<EquipmentData> equipmentList = EquipmentDatabase.GetRandomEquipment(10);
            if (equipmentList.Count > 0)
            {
                equipmentPanel.ShowMultipleEquipments(equipmentList);
            }
        }
        else
        {
            // ✅ 60% 확률로 골드 지급 (50G ~ 110G)
            int goldAmount = Random.Range(50, 111);
            equipmentPanel.ShowGoldReward(goldAmount);
            payGold(false, goldAmount);
        }
    }
    
    public void DrawRandom1Reward(bool isads)
    {
        payGold(isads, -100);
        // ✅ UI 패널 활성화
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(true);

        float roll = Random.Range(0f, 100f);

        if (roll < 40f) // 40% 확률로 장비 지급
        {
            // ✅ 장비 1개 뽑기
            List<EquipmentData> equipmentList = EquipmentDatabase.GetRandomEquipment(1);
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
            payGold(false, goldAmount);
        }
    }

    private void payGold(bool isAds, int amount)
    {
        if (isAds) return;
        else
        {
            int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
            DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold + amount).ToString());
        }
    }

    public void closePanel()
    {
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
    }
}
