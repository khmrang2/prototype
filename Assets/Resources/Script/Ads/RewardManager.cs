using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public void give500Gold()
    {
        Debug.Log("Attempting to give 500 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 500);
        
    }
    
    public void give1000Gold()
    {
        Debug.Log("Attempting to give 1000 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 1000);
    }
    
    public void give2000Gold()
    {
        Debug.Log("Attempting to give 2000 gold...");
        AdManager.Instance.ShowRewardedInterstitialAd("gold", 2000);
    }

    public void give1equipment()
    {
        AdManager.Instance.ShowRewardedInterstitialAd("item", 1);
    }
}