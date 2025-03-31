using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusInMain : MonoBehaviour
{
    public TextMeshProUGUI debug;
    public static PlayerStatusInMain Instance { get; private set; }


    [SerializeField] private DataControl datactr; // 데이터 컨트롤. 인스펙터에서 할당.
    [SerializeField] private SaveAndLoadError SaveandLoaderror;

    // public static int player_has_gold = 0;
    // public static int player_has_upgradeStone = 0;
    // public static List<ItemDataForSave> player_has_inventory;
    // public static List<ItemDataForSave> player_has_equip;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI upgradestoneText;

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
    /// 외부호출용
    /// 플레이어가 amount의 골드를 소비하는 메소드.
    /// 돈을 서버에서 로드한 다음
    /// 사용 여부 판단,
    /// 돈을 실제로 사용합니다.
    ///
    /// 그래서 성공하면 true, 실패하면 false를 리턴합니다.
    ///
    /// 프렙스에 저장.
    /// gpgs저장
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> bool </returns>
    public void PayGold(int amount, Action<bool> onComplete)
    {
        // 예시상황 146골드의 상황에서 만약 amount로 100골드가 들어온다면.
        // 프렙스에서 146골드를 가져옴.
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        Debug.Log($"{currentGold} 1. 골드를 로드해옴.");

        int afterGold = currentGold - amount;
        // 1. 바로 일단 사용한 돈 저장. 46골드가 저장.
        DataControl.SaveEncryptedDataToPrefs("Gold", afterGold.ToString());
        UpdateGoldUI(afterGold);
        // 2. 바로 성공 처리해서 UI 반응 허용
        // 어차피 골드랑 그런거는
        // gpgs에 저장이 안되었기 때문에
        onComplete?.Invoke(true);

        // 3. 백그라운드로 gpgs에 프렙스의 모든 정보를 저장함
        // gpgs에는 46골드가 저장됩니다.
        datactr.SaveDataWithCallback(success =>
        {
            if (!success)
            {
                // 이걸 프렙스에 저장할 이유도 없음.
                // DataControl.SaveEncryptedDataToPrefs("Gold", currentGold.ToString());
                UpdateGoldUI(currentGold);
                Debug.Log($"서버 연결 실패! 복구 중 ! {currentGold}");
                SaveandLoaderror.ShowErrorScreen();

                AppControl.IsRestoreCompleted = true; // 복구 완료
            }
            else
            {
                // gpgs에 프렙스 저장을 성공함.
                Debug.Log("✅ 서버 저장 성공!");
            }
        });
    }




    /// <summary>
    /// 외부호출용
    /// amount의 골드를 얻는 메소드.
    /// </summary>
    /// <param name="amount"></param>
    public void getGold(int amount)
    {
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        int afterGold = currentGold + amount;

        DataControl.SaveEncryptedDataToPrefs("Gold", afterGold.ToString());
        UpdateGoldUI(afterGold);

        datactr.SaveDataWithCallback(success => {
            if (!success)
            {
                // 실패했으면
                DataControl.SaveEncryptedDataToPrefs("Gold", currentGold.ToString());
                UpdateGoldUI(currentGold);
                Debug.LogError("⛔ 서버 저장 실패! 골드 되돌림.");
                SaveandLoaderror.ShowErrorScreen();
            }
            else
            {
                Debug.Log("✅ 골드 획득 서버 저장 성공!");
            }
        });
    }

    /// <summary>
    /// 외부호출용
    /// amount의 강화석을 "사용"하는 메소드.
    /// </summary>
    /// <param name="amount"></param>
    public void payUpgradeStone(int amount, Action<bool> onComplete)
    {
        // 예시상황 146골드의 상황에서 만약 amount로 100골드가 들어온다면.
        // 프렙스에서 146골드를 가져옴.
        int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        int afterUpgradeStone = currentUpgradeStone - amount;

        Debug.Log($"{currentUpgradeStone} 1. 업그레이드 돌을 로드해옴.");

        // 1. 바로 일단 사용한 돈 저장. 46골드가 저장.
        DataControl.SaveEncryptedDataToPrefs("UpgradeStone", afterUpgradeStone.ToString());
        UpdateStoneUI(afterUpgradeStone);
        // 2. 바로 성공 처리해서 UI 반응 허용
        // 어차피 골드랑 그런거는
        // gpgs에 저장이 안되었기 때문에
        onComplete?.Invoke(true);

        // 3. 백그라운드로 gpgs에 프렙스의 모든 정보를 저장함
        // gpgs에는 46골드가 저장됩니다.
        datactr.SaveDataWithCallback(success =>
        {
            if (!success)
            {
                // 이걸 프렙스에 저장할 이유도 없음.
                //DataControl.SaveEncryptedDataToPrefs("UpgradeStone", currentUpgradeStone.ToString());
                UpdateStoneUI(currentUpgradeStone);
                Debug.Log($"서버 연결 실패! 복구 중 ! {currentUpgradeStone}");
                SaveandLoaderror.ShowErrorScreen();

                AppControl.IsRestoreCompleted = true; // 복구 완료
            }
            else
            {
                // gpgs에 프렙스 저장을 성공함.
                Debug.Log("✅ 서버 저장 성공!");
            }
        });
    }
    /// <summary>
    /// 외부호출용
    /// amount의 강화석을 "얻는" 메소드
    /// </summary>
    /// <param name="amount"></param>
    public void getUpgradeStone(int amount)
    {
        int currentUpgradeStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        int afterUpgradeStone = currentUpgradeStone + amount;

        DataControl.SaveEncryptedDataToPrefs("UpgradeStone", afterUpgradeStone.ToString());
        UpdateStoneUI(afterUpgradeStone);

        datactr.SaveDataWithCallback(success => {
            if (!success)
            {
                // 실패했으면
                DataControl.SaveEncryptedDataToPrefs("UpgradeStone", currentUpgradeStone.ToString());
                UpdateStoneUI(currentUpgradeStone);
                Debug.LogError("⛔ 서버 저장 실패! 업그레이드 스톤 되돌림.");
                SaveandLoaderror.ShowErrorScreen();
            }
            else
            {
                Debug.Log("✅ 골드 획득 서버 저장 성공!");
            }
        });
    }

    private void UpdateGoldUI(int amount)
    {
        // goldText 같은 TextMeshProUGUI가 있다면:
        if (goldText != null)
        {
            goldText.text = amount.ToString();
        }
    }

    private void UpdateStoneUI(int amount)
    {
        // goldText 같은 TextMeshProUGUI가 있다면:
        if (upgradestoneText != null)
        {
            upgradestoneText.text = amount.ToString();
        }
    }

    public void TryBuyRewardPack(int goldCost, List<ItemDataForSave> items, int earn_gold, int earn_upgrade_stone, Action<bool> onComplete)
    {

        // 1. 로드
        int currentGold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        int currentStone = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));
        var currentInventory = DataControl.LoadItemDataFromPrefs("PlayerInventory");

        // 2. 계산 및 저장
        int usedGold = currentGold - goldCost;
        int afterGold = usedGold + earn_gold;
        int afterStone = currentStone + earn_upgrade_stone;


        // 3. 프렙스 저장.
        DataControl.SaveEncryptedDataToPrefs("Gold", afterGold.ToString());
        DataControl.SaveEncryptedDataToPrefs("UpgradeStone", afterStone.ToString());
        Inventory.Instance.AddOrUpdateItems(items, true); // 프렙스에 저장 및 ui 새로고침
        UpdateGoldUI(afterGold);        // 골드 ui 새로고침.
        UpdateStoneUI(afterStone);      // 업그레이드 스톤 ui 새로고침.

        //유니티환경 테스트용 모바일에는 디버그용을 풀어야함.
        //onComplete?.Invoke(true);
        //return;
        // 3. 서버 저장 시도
        datactr.SaveDataWithCallback(success =>
        {
            if (!success)
            {
                // 롤백 - 근데 할 이유 없음.
                //DataControl.SaveEncryptedDataToPrefs("Gold", currentGold.ToString());
                //DataControl.SaveEncryptedDataToPrefs("UpgradeStone", currentStone.ToString());
                //Inventory.Instance.RestoreBackup(originalInventory);

                UpdateGoldUI(currentGold);
                UpdateStoneUI(currentStone);

                SaveandLoaderror.ShowErrorScreen();
                AppControl.IsRestoreCompleted = true;
                Debug.LogError("❌ 서버 저장 실패! 전체 롤백");
                onComplete?.Invoke(false);
            }
            else
            {
                Debug.Log("✅ 서버 저장 성공!");
                onComplete?.Invoke(true);
            }
        });
    }

    public bool hasEnoughGold(int goldCost)
    {
        return int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold")) > goldCost;
    }

    public bool hasEnoughStone(int stoneCost)
    {
        return int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone")) > stoneCost;
    }
    // player stat : migration with tae yeon.

    // gold
    // upgreader item
    // inventory
    // jangbi
    // data load system.
}
