using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public void give500Gold()
    {
        if (CanClaimReward("gold_500"))
        {
            Debug.Log("Attempting to give 500 gold...");
            AdManager.Instance.ShowRewardedInterstitialAd("gold", 500);
            SaveRewardDate("gold_500");
        }
    }

    public void give1000Gold()
    {
        if (CanClaimReward("gold_1000"))
        {
            Debug.Log("Attempting to give 1000 gold...");
            AdManager.Instance.ShowRewardedInterstitialAd("gold", 1000);
            SaveRewardDate("gold_1000");
        }
    }

    public void give2000Gold()
    {
        if (CanClaimReward("gold_2000"))
        {
            Debug.Log("Attempting to give 2000 gold...");
            AdManager.Instance.ShowRewardedInterstitialAd("gold", 2000);
            SaveRewardDate("gold_2000");
        }
    }

    public void give10equipment()
    {
        if (CanClaimReward("item_10"))
        {
            AdManager.Instance.ShowRewardedInterstitialAd("item", 10);
            SaveRewardDate("item_10");
        }
    }

    private bool CanClaimReward(string key)
    {
        string lastDate = DataControl.LoadEncryptedDataFromPrefs(key + "_date");
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        return lastDate != today;
    }

    private void SaveRewardDate(string key)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DataControl.SaveEncryptedDataToPrefs(key + "_date", today);
    }
}