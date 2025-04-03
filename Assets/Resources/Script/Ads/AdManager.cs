using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }  // ✅ 싱글톤 인스턴스
    private RewardedInterstitialAd _rewardedInterstitialAd; // ✅ 보상형 전면 광고 객체

    #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/5354046379";  // ✅ 테스트 광고 ID (Android)
    #elif UNITY_IPHONE
        private string _adUnitId = "ca-app-pub-3940256099942544/6978759866";  // ✅ 테스트 광고 ID (iOS)
    #else
        private string _adUnitId = "unused";  // ✅ 기타 플랫폼에서는 광고를 사용하지 않음
    #endif

    // ✅ 보상 종류별 실행할 함수 저장 (골드, 아이템, 버프)
    private Dictionary<string, Action<int>> rewardActions;

    private void Awake()
    {
        // ✅ 싱글톤 패턴 적용: 인스턴스가 없으면 생성하고, 있으면 삭제
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ 씬이 변경되어도 유지됨
        }
        else
        {
            Destroy(gameObject); // ✅ 중복 생성 방지
        }
    }

    private void Start()
    {
        // ✅ Google Ads SDK 초기화 및 광고 로드
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads Initialized.");
            LoadRewardedInterstitialAd();
        });

        // ✅ 보상 액션을 사전에 등록 (Dictionary 활용)
        rewardActions = new Dictionary<string, Action<int>>
        {
            { "gold", GiveGold },   // 골드 지급
            { "upgradeStone", GiveUpgradeStone },
            { "item", GiveItem }
        };
    }

    /// <summary>
    /// ✅ 보상형 전면 광고 로드 함수
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        // ✅ 기존 광고 객체가 있다면 삭제
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        // ✅ 광고 요청 객체 생성
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

                // ✅ 광고 닫힌 후 자동 재로딩을 위해 이벤트 핸들러 등록
                RegisterReloadHandler(_rewardedInterstitialAd);
            });
    }

    /// <summary>
    /// ✅ 광고가 닫혔을 때 자동으로 새로운 광고를 로드하도록 설정
    /// </summary>
    private void RegisterReloadHandler(RewardedInterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad closed. Reloading...");
            LoadRewardedInterstitialAd();  // 광고 닫힌 후 새로운 광고 로드
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed: " + error);
            LoadRewardedInterstitialAd();  // 광고가 실패하면 다시 로드
        };
    }

    /// <summary>
    /// ✅ 버튼 클릭 시 광고를 실행하고, 보상을 지급하는 함수
    /// </summary>
    /// <param name="rewardType">보상 유형 (gold, item, buff)</param>
    /// <param name="amount">보상 수량</param>
    public void ShowRewardedInterstitialAd(string rewardType, int amount)
    {
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show(reward =>
            {
                Debug.Log($"User earned reward: {amount} {rewardType}");

                // ✅ 보상 지급 함수 실행
                if (rewardActions.ContainsKey(rewardType))
                {
                    Debug.Log($"Executing reward action for {rewardType}");
                    rewardActions[rewardType].Invoke(amount);
                }
                else
                {
                    Debug.LogError($"Reward type '{rewardType}' not found.");
                }
            });
        }
        else
        {
            Debug.Log("Rewarded Interstitial Ad is not ready yet. Loading a new ad...");
            LoadRewardedInterstitialAd();
        }
    }

    // ✅ 골드 지급 함수
    private void GiveGold(int amount)
    {
        PlayerStatusInMain.Instance.getGold(amount);
    }

    // ✅ 강화석 지급 함수
    private void GiveUpgradeStone(int amount)
    {
        PlayerStatusInMain.Instance.getUpgradeStone(amount);
    }

    // ✅ 아이템 지급 함수
    private void GiveItem(int amount)
    {
        EquipmentUIManager equipmentUiManager = FindObjectOfType<EquipmentUIManager>();
        Debug.Log($"이거 뭐야 {amount}");
        equipmentUiManager.PerformDrawReward(0, amount, true);
    }
}
