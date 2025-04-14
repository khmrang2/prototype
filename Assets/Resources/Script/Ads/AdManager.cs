using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private RewardedInterstitialAd _rewardedInterstitialAd;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
    private string _adUnitId = "unused";
#endif

    private Dictionary<string, Action<int>> rewardActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads Initialized.");
            LoadRewardedInterstitialAd();
        });

        rewardActions = new Dictionary<string, Action<int>>
        {
            { "gold", GiveGold },
            { "upgradeStone", GiveUpgradeStone },
            { "item", GiveItem }
        };
    }

    public void LoadRewardedInterstitialAd()
    {
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        AdRequest request = new AdRequest();
        RewardedInterstitialAd.Load(_adUnitId, request,
            (ad, error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Rewarded Interstitial Ad failed to load: " + error.GetMessage());
                    return;
                }

                _rewardedInterstitialAd = ad;
                Debug.Log("Rewarded Interstitial Ad loaded successfully.");
                RegisterReloadHandler(_rewardedInterstitialAd);
            });
    }

    private void RegisterReloadHandler(RewardedInterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad closed. Reloading...");
            LoadRewardedInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed: " + error);
            LoadRewardedInterstitialAd();
        };
    }

    public void ShowRewardedInterstitialAd(string rewardType, int amount)
    {
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show(reward =>
            {
                try
                {
                    StartCoroutine(DelayedReward(rewardType, amount));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[광고 보상 중 예외] {ex.Message}\n{ex.StackTrace}");
                }
            });
        }
        else
        {
            Debug.Log("광고가 아직 준비되지 않았습니다. 광고를 다시 로드합니다.");
            LoadRewardedInterstitialAd();
        }
    }

    private IEnumerator DelayedReward(string rewardType, int amount)
    {
        yield return null;

        if (rewardActions != null && rewardActions.ContainsKey(rewardType))
        {
            Debug.Log($"[보상 지급 시작] {rewardType} - {amount}");
            try
            {
                rewardActions[rewardType]?.Invoke(amount);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[보상 함수 실행 중 예외] {ex.Message}\n{ex.StackTrace}");
            }
        }
        else
        {
            Debug.LogError($"[보상 타입 미등록 또는 rewardActions가 null입니다] rewardType: {rewardType}");
        }
    }

    private void GiveGold(int amount)
    {
        PlayerStatusInMain.Instance?.getGold(amount);
    }

    private void GiveUpgradeStone(int amount)
    {
        PlayerStatusInMain.Instance?.getUpgradeStone(amount);
    }

    private void GiveItem(int amount)
    {
        EquipmentUIManager equipmentUiManager = FindObjectOfType<EquipmentUIManager>();
        if (equipmentUiManager != null)
        {
            try
            {
                equipmentUiManager.PerformDrawReward(0, amount, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[아이템 지급 중 예외] {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("[GiveItem] EquipmentUIManager를 찾을 수 없습니다. 현재 씬에 존재하나요?");
        }
    }
}
