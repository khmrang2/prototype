using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class EquipmentUIManager : MonoBehaviour
{
    public EquipmentUIPanel equipmentPanel; // UI 패널 (Inspector에서 연결)
    public Button draw_10_rewardsButton; // 뽑기 버튼 (Inspector에서 연결)
    public Button draw_1_rewardButton; // 단일 뽑기 버튼
    public Button close_Button; // 닫기 버튼 (Inspector에서 연결)

    private List<ItemDataForSave> item_id_list = null;

    public TextMeshProUGUI fordebug;

    void Start()
    {
        // 내부 변수 아이템 id 리스트 초기화.
        if (item_id_list == null)
        {
            item_id_list = new List<ItemDataForSave>();
        }

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

    public void DrawRandom10Rewards(bool isAds)
    {
        makepayment(900, isAds, (success) =>
        {
            if (!success) return;

            if (equipmentPanel != null)
            {
                Debug.Log("패널 활성화 시작");
                equipmentPanel.gameObject.SetActive(true);
            }

            Debug.Log("가챠 시작");
            if (equipmentPanel.ShowMultipleEquipments(gacha(10)))
            {
                Debug.Log("가챠에 넣기");
                addEquipmentToInventory();
                Debug.Log("인벤토리에 넣기 완료.");
            }
            else
            {
                Debug.LogWarning("? 실패했니?");
            }
        });
    }

    public void DrawRandom1Reward(bool isads)
    {
        makepayment(100, isads, (success) =>
        {
            if (!success) return;

            if (equipmentPanel != null)
                equipmentPanel.gameObject.SetActive(true);

            equipmentPanel.ShowSingleEquipment(gacha(1));
            addEquipmentToInventory();
        });
    }

    private void makepayment(int amount, bool isAds, System.Action<bool> onComplete)
    {
        if (PlayerStatusInMain.Instance == null)
        {
            Debug.LogError("🚨 PlayerStatusInMain.Instance가 null입니다! 초기화 확인 필요");
            onComplete?.Invoke(false);
            return;
        }

        if (isAds)
        {
            // 광고로 지불하면 바로 성공
            onComplete?.Invoke(true);
            return;
        }

        PlayerStatusInMain.Instance.PayGold(amount, (success) =>
        {
            if (success)
            {
                Debug.Log("💰 가격 지불 성공!");
            }
            else
            {
                Debug.LogError("❌ 가격 지불 실패!");
            }
            onComplete?.Invoke(success);
        });
    }

    public void closePanel()
    {
        PlayerStatusInMain.Instance.getGold(equipmentPanel.GetEarnedGold());
        PlayerStatusInMain.Instance.getUpgradeStone(equipmentPanel.GetEarnedUpgradeStone());
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
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
    private List<ItemDataForSave> gacha(int gacha_count)
    {
        if(item_id_list != null)
        {
            item_id_list.Clear();
        }

        for (int i = 0; i < gacha_count; i++)
        {
            int id = 0, amount = 0;
            float roll = Random.Range(0f, 100f);
            if (roll < 40)
            {
                // 40% 장비 수량 결정.
                id = ItemDatabase.Instance.GetRandomItemId(ItemDatabase.RANGE_EQUIPMENT);
                amount = Random.Range(1, 3);
            }else if(40 < roll && roll <= 70){
                // 30% 업그레이드 아이템. 수량 결정.
                id = ItemDatabase.RANGE_COSUMABLE;
                amount = Random.Range(1, 5);
            }
            else
            {
                // 30% 골드. 수량 결정.
                id = ItemDatabase.RANGE_GOLD_POT;
                amount = Random.Range(50, 110);      
            }
            item_id_list.Add(new ItemDataForSave(id, amount));
        }

        return item_id_list;
    }
}
