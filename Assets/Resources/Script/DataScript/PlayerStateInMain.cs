using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusInMain : MonoBehaviour
{
    public static PlayerStatusInMain Instance { get; private set; }

    public static int player_has_gold = 0;
    public static int player_has_upgradeStone = 0;
    public static List<ItemDataForSave> player_has_inventory;
    public static List<ItemDataForSave> player_has_equip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 다른 씬에서도 유지되도록 설정
    }

    /// <summary>
    /// 플레이어가 amount의 골드를 소비하는 메소드. 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool payGold(int amount)
    {
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        if (currentGold < amount) return false;
        DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold - amount).ToString());
        return true;
    }

    public void getGold(int amount)
    {
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        Debug.Log($"Player received {amount} gold.");
        DataControl.SaveEncryptedDataToPrefs("Gold", (currentGold + amount).ToString());
        // TODO: 골드 추가 로직 구현 (예: 플레이어의 골드 데이터 업데이트)
    }

    public bool payUpgradeStone(int amount)
    {
        int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        if (currentUpgradeStone < amount) return false;
        DataControl.SaveEncryptedDataToPrefs("Gold", (currentUpgradeStone - amount).ToString());
        return true;
    }

    public void getUpgradeStone(int amount)
    {
        int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        Debug.Log($"Player received {amount} upgrade stone");
        DataControl.SaveEncryptedDataToPrefs("UpgradeStone", (currentUpgradeStone + amount).ToString());
        // TODO: 골드 추가 로직 구현 (예: 플레이어의 골드 데이터 업데이트)
    }
    // player stat : migration with tae yeon.

    // gold
    // upgreader item
    // inventory
    // jangbi
    // data load system.
}
