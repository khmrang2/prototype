using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiveRewardOnClear : MonoBehaviour
{
    public PlayerState playerState;

    [Header("Clear reward settings")]
    [SerializeField] private int clearGold;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject itemSlot;
    


    private void OnEnable()
    {
        clearGold = (int)(clearGold * (1.0f + playerState.Player_More_Economy));


        // UI 갱신
        rewardText.text = "보상: " + clearGold + "G";

        // 슬롯 생성
        GameObject goldPot = Instantiate(itemSlot, slotParent.transform, false);
        SlotInven goldPotData = goldPot.GetComponent<SlotInven>();
        goldPotData.setInit(new ItemData(ItemDatabase.Instance.FetchItemById(ItemDatabase.ID_GOLD_POT), clearGold));

        int currentStage = PlayerStatusInMain.Instance.GetCurStage(); // ← 현재 스테이지 index 받아와야 함

        PlayerStatusInMain.Instance.CompleteStage(currentStage, clearGold, success =>
        {
            if (!success)
            {
                Debug.LogError("❌ 스테이지 클리어 저장 실패");
                return;
            }
            Debug.Log("🎉 스테이지 클리어 보상 지급 완료!");
        });
    }
}
