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

    //public bool payUpgradeStone(int amount)
    //{
    //    int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
    //    if (currentUpgradeStone < amount) return false;
    //    DataControl.SaveEncryptedDataToPrefs("Gold", (currentUpgradeStone - amount).ToString());
    //    return true;
    //}
    // player stat : migration with tae yeon.

    // gold
    // upgreader item
    // inventory
    // jangbi
    // data load system.
}
