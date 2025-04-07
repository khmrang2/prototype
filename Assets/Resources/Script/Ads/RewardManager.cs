using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public void give100Gold()
    {
        Debug.Log("Attempting to give 100 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 100);
        
    }
    
    public void give200Gold()
    {
        Debug.Log("Attempting to give 200 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 200);
    }
    
    public void give300Gold()
    {
        Debug.Log("Attempting to give 300 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 300);
    }

    public void give10equipment()
    {
        if (CanClaimReward("item_1"))
        {
            AdManager.Instance.ShowRewardedInterstitialAd("item", 1);
            SaveRewardDate("item_1");
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