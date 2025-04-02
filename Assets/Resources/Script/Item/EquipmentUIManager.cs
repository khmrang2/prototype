using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using TMPro;


public class EquipmentUIManager : MonoBehaviour
{
    public EquipmentUIPanel equipmentPanel; // UI 패널 (Inspector에서 연결)
    public Button draw_10_rewardsButton; // 뽑기 버튼 (Inspector에서 연결)
    public Button draw_1_rewardButton; // 단일 뽑기 버튼
    public Button close_Button; // 닫기 버튼 (Inspector에서 연결)
    public AudioSource reward_Sound;

	public TMP_Text _500_gold_ad_text;
	public TMP_Text _1000_gold_ad_text;
	public TMP_Text _2000_gold_ad_text;
	public TMP_Text _10_equipments_ad_text;

    private List<ItemDataForSave> item_id_list = null;
    private List<ItemDataForSave> show_item_id_list = null;

    public TextMeshProUGUI fordebug;

    private int earn_gold = 0;
    private int earn_upgrade_stone = 0;

    void Start()
    {
        // 내부 변수 아이템 id 리스트 초기화.
        if (item_id_list == null)
        {
            item_id_list = new List<ItemDataForSave>();
        }

        if (show_item_id_list == null)
        {
            show_item_id_list = new List<ItemDataForSave>();
        }

        //올바른 UI 패널 비활성화 방식
        if (equipmentPanel != null)
            equipmentPanel.gameObject.SetActive(false);

        //버튼 이벤트 연결 (람다식 사용하여 매개변수 전달)
        if (draw_10_rewardsButton != null)
        {
            draw_10_rewardsButton.onClick.AddListener(() => PerformDrawReward(900, 10, false));
        }
        if (draw_1_rewardButton != null)
        {
            draw_1_rewardButton.onClick.AddListener(() => PerformDrawReward(100, 1, false));
        }
        if (close_Button != null)
        {
            close_Button.onClick.AddListener(() => closePanel());
        }
    }

	public void Update(){
		_500_gold_ad_text.text = CanClaimReward("gold_500") ? "0/1" : "1/1";
    	_1000_gold_ad_text.text = CanClaimReward("gold_1000") ? "0/1" : "1/1";
    	_2000_gold_ad_text.text = CanClaimReward("gold_2000") ? "0/1" : "1/1";
    	_10_equipments_ad_text.text = CanClaimReward("item_10") ? "0/1" : "1/1";
	}

    public void PerformDrawReward(int goldCost, int count, bool isAds)
    {
        if (PlayerStatusInMain.Instance == null) return;

        if (!PlayerStatusInMain.Instance.hasEnoughGold(goldCost))
        {
            Debug.Log("골드 부족! UI 표시 안 함");
            return;
        }

        gacha(count);
        var rewards = item_id_list;

        PlayerStatusInMain.Instance.TryBuyRewardPack(goldCost, rewards, earn_gold, earn_upgrade_stone,  success =>
        {
            if (!success)
            {
                Debug.LogError("❌ 뽑기 실패! 상태 복구됨.");
            }
            else
            {
                //저장이 완료된 이후에 결과화면 출력

                equipmentPanel.gameObject.SetActive(true);

                if (isAds)
                {
                    Inventory.Instance.AddOrUpdateItems(item_id_list, true);
                    reward_Sound.Play();
                    equipmentPanel.ShowMultipleEquipments(show_item_id_list);
                    return;
                }

                if (count == 1)
                {
                    reward_Sound.Play();
                    equipmentPanel.ShowSingleEquipment(show_item_id_list);
                }
                else
                {
                    reward_Sound.Play();
                    equipmentPanel.ShowMultipleEquipments(show_item_id_list);
                }

            }
        });

        
    }

    public void closePanel()
    {
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// handItem을 인벤토리에 넘겨줘야함.
    /// </summary>
    /// <param name="handItem"></param>
    public void addEquipmentToInventory()
    {
        // 저장해야하기 때문에 플레이어 프렙스에 무조건 저장.
        Inventory.Instance.AddOrUpdateItems(item_id_list, true);
    }
    
    /// <summary>
    /// param : gacha_count만큼 랜덤 뽑기를 시행.
    /// </summary>
    /// <param name="gacha_count"></param>
    /// <returns></returns>
    private List<ItemDataForSave> gacha(int gacha_count)
    {
        earn_gold = 0;
        earn_upgrade_stone = 0;

        if (item_id_list != null)
        {
            item_id_list.Clear();
        }

        if (show_item_id_list != null)
        {
            show_item_id_list.Clear();
        }

        for (int i = 0; i < gacha_count; i++)
        {
            int id = 0, amount = 0;
            float roll = UnityEngine.Random.Range(0f, 100f);
            if (roll < 40)
            {
                // 40% 장비 수량 결정.
                id = ItemDatabase.Instance.GetRandomItemId(ItemDatabase.RANGE_EQUIPMENT);
                amount = UnityEngine.Random.Range(1, 3);
                item_id_list.Add(new ItemDataForSave(id, amount));
            }
            else if(40 < roll && roll <= 70){
                // 30% 업그레이드 아이템. 수량 결정.
                id = ItemDatabase.RANGE_COSUMABLE;
                amount = UnityEngine.Random.Range(1, 5);
                earn_upgrade_stone += amount;
            }
            else
            {
                // 30% 골드. 수량 결정.
                id = ItemDatabase.RANGE_GOLD_POT;
                amount = UnityEngine.Random.Range(50, 110);
                earn_gold = amount;
            }
            show_item_id_list.Add(new ItemDataForSave(id, amount));
        }

        return show_item_id_list;
    }
	
	private bool CanClaimReward(string key)
    {
        string lastDate = DataControl.LoadEncryptedDataFromPrefs(key + "_date");
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        return lastDate != today;
    }
}
/***
 * 
 * using UnityEngine;
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
    private List<ItemDataForSave> show_item_id_list = null;

    public TextMeshProUGUI fordebug;

    private int earn_gold = 0;
    private int earn_upgrade_stone = 0;

    void Start()
    {
        // 내부 변수 아이템 id 리스트 초기화.
        if (item_id_list == null)
        {
            item_id_list = new List<ItemDataForSave>();
        }

        if (show_item_id_list == null)
        {
            show_item_id_list = new List<ItemDataForSave>();
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
            // 자체가 makepayment가 끝나야 실행이 됨..
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
        if (equipmentPanel != null) equipmentPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// handItem을 인벤토리에 넘겨줘야함.
    /// </summary>
    /// <param name="handItem"></param>
    public void addEquipmentToInventory()
    {
        // 저장해야하기 때문에 플레이어 프렙스에 무조건 저장.
        Inventory.Instance.AddOrUpdateItems(item_id_list, true);
    }
    
    /// <summary>
    /// param : gacha_count만큼 랜덤 뽑기를 시행.
    /// </summary>
    /// <param name="gacha_count"></param>
    /// <returns></returns>
    private List<ItemDataForSave> gacha(int gacha_count)
    {
        if (item_id_list != null)
        {
            item_id_list.Clear();
        }

        if (show_item_id_list != null)
        {
            show_item_id_list.Clear();
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
                item_id_list.Add(new ItemDataForSave(id, amount));
            }
            else if(40 < roll && roll <= 70){
                // 30% 업그레이드 아이템. 수량 결정.
                id = ItemDatabase.RANGE_COSUMABLE;
                amount = Random.Range(1, 5);
                earn_upgrade_stone += amount;
            }
            else
            {
                // 30% 골드. 수량 결정.
                id = ItemDatabase.RANGE_GOLD_POT;
                amount = Random.Range(50, 110);
                earn_gold = amount;
            }
            show_item_id_list.Add(new ItemDataForSave(id, amount));
        }

        return show_item_id_list;
    }
}

 * 
 * 
 */