using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

public enum BuffRank
{
    Normal = 0,
    Epic = 1
}

[System.Serializable]
public class BuffStruct
{
    public int ID;
    public BuffRank Rank;
    public string Name;
    public string Tooltip;
    public string ImagePath;
    public Dictionary<string, float> buffState;
}

[System.Serializable]
public class BuffDataList
{
    public List<BuffStruct> buffs;
}

public class BuffManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private List<BuffSelectUI> buffUIs;
    [SerializeField] private GameObject selectPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource buffPanelActiveSound;
    [SerializeField] private AudioSource buffClickSound;

    [Header("Player Reference")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerManger playerManger;

    [Header("Buff Data")]
    [SerializeField] private TextAsset buffDataJSON;
    [SerializeField] private float NormalBuff_GetChance = 95.0f;
    [SerializeField] private float EpicBuff_GetChance = 5.0f;
    
    [Header("Ad Settings")]
    [SerializeField] private bool useAdForEpicBuffs = true;
    [SerializeField] private int buffSelectThreshold = 5; // 버프 선택 횟수 임계값

    private List<BuffStruct> cachedNormalBuffs = new List<BuffStruct>();
    private List<BuffStruct> cachedEpicBuffs = new List<BuffStruct>();
    private List<BuffStruct> selectedBuffs = new List<BuffStruct>();
    private List<BuffStruct> tempNormalBuffs;
    private List<BuffStruct> tempEpicBuffs;
    private bool isBuffSelected = false;
    private int selectedBuffId = -1;
    
    // 광고 관련 변수
    private static int buffSelectCount = 0; // 버프 선택 횟수
    private int adLockedBuffIndex = -1; // 광고로 잠긴 버프 인덱스
    private bool isLowHealth = false; // 체력이 임계값 이하인지 여부
    private bool hasTriggeredLowHealthAd = false;

    private void Awake()
    {
        if (playerState == null) playerState = FindObjectOfType<PlayerState>();
        if (playerManger == null) playerManger = FindObjectOfType<PlayerManger>();
    }

    private void OnEnable()
    {
        // PlayerManger의 체력 비율 이벤트 구독
        PlayerManger.OnHealthRatioChanged += OnHealthRatioChanged;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        PlayerManger.OnHealthRatioChanged -= OnHealthRatioChanged;
    }

    private void Start()
    {
        LoadBuffDataFromJSON();
        InitializeTempLists();
        selectPanel.SetActive(false);
        
        // AdManager에 버프 광고 보상 처리 함수 등록
        if (AdManager.Instance != null && !AdManager.Instance.HasRewardAction("epicBuff"))
        {
            AdManager.Instance.RegisterRewardAction("epicBuff", OnAdRewardEarned);
        }
        
        // 초기 체력 상태 확인
        if (playerManger != null)
        {
            isLowHealth = playerManger.IsLowHealth();
        }
    }
    
    // PlayerManger의 체력 비율 변화 이벤트 처리
    private void OnHealthRatioChanged(float healthRatio)
    {
        // PlayerManger의 IsLowHealth() 메서드를 사용하여 체력 상태 가져오기
        if (playerManger != null)
        {
            bool currentLowHealth = playerManger.IsLowHealth();
            
            // 체력이 낮고, 아직 한 번도 실행되지 않았을 때만 처리
            if (currentLowHealth && !hasTriggeredLowHealthAd)
            {
                isLowHealth = true;
                hasTriggeredLowHealthAd = true; // 한 번 실행됨을 표시
                Debug.Log($"BuffManager: 체력 비율 {healthRatio:F2}, 낮은 체력 상태: {isLowHealth} (최초 1회)");
            }
            else
            {
                isLowHealth = currentLowHealth;
            }
        }
    }

    private void InitializeTempLists()
    {
        // 필요할 때만 초기화하도록 변경
        tempNormalBuffs = new List<BuffStruct>(cachedNormalBuffs.Count);
        tempEpicBuffs = new List<BuffStruct>(cachedEpicBuffs.Count);
    }

    private void LoadBuffDataFromJSON()
    {
        buffDataJSON = Resources.Load<TextAsset>("Data/BuffData");
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonConvert.DeserializeObject<BuffDataList>(jsonText);

        // 데이터를 직접 리스트로 분류
        cachedNormalBuffs.Clear();
        cachedEpicBuffs.Clear();

        foreach (BuffStruct buff in dataList.buffs)
        {
            if (buff.Rank == BuffRank.Normal)
            {
                cachedNormalBuffs.Add(buff);
            }
            else if (buff.Rank == BuffRank.Epic)
            {
                cachedEpicBuffs.Add(buff);
            }
            else
            {
                Debug.LogError("잘못된 버프 랭크입니다");
            }
        }
    }

    // 1. UI 패널을 활성화하고, 임의의 3개 버프를 표시
    public void ShowBuffSelection()
    {
        isBuffSelected = false;
        if (selectedBuffs == null)
            selectedBuffs = new List<BuffStruct>();
        else
            selectedBuffs.Clear();
            
        adLockedBuffIndex = -1; // 광고 잠금 버프 인덱스 초기화
        
        buffPanelActiveSound.Play();
        selectPanel.SetActive(true);
        ShowRandomBuffs();
    }

    private void ShowRandomBuffs()
    {
        // 기본 버프 선택
        selectedBuffs = GetRandomBuffs(3);
        
        // 조건 충족 시 광고 에픽 버프 추가 (버프 선택 횟수 5회 이상 또는 체력 50% 이하)
        bool showAdEpicBuff = false;
        if (useAdForEpicBuffs)
        {
            showAdEpicBuff = (buffSelectCount >= buffSelectThreshold) || isLowHealth;
        }
        
        // 광고 에픽 버프 추가 (조건 충족 시)
        if (showAdEpicBuff && cachedEpicBuffs.Count > 0)
        {
            // 랜덤 에픽 버프 선택
            int randomIndex = UnityEngine.Random.Range(0, cachedEpicBuffs.Count);
            BuffStruct adEpicBuff = cachedEpicBuffs[randomIndex];
            
            // 버프 UI 위치 결정 (랜덤)
            adLockedBuffIndex = UnityEngine.Random.Range(0, selectedBuffs.Count);
            
            // 해당 위치의 버프를 광고 에픽 버프로 교체
            selectedBuffs[adLockedBuffIndex] = adEpicBuff;
        }
        
        // UI 업데이트
        for (int i = 0; i < selectedBuffs.Count && i < buffUIs.Count; i++)
        {
            bool isAdLocked = (i == adLockedBuffIndex);
            buffUIs[i].getBuffState(selectedBuffs[i], isAdLocked);
            buffUIs[i].RegisterBuffSelectionCallback(OnBuffSelected);
        }
    }

    private List<BuffStruct> GetRandomBuffs(int count)
    {
        if (count <= 0)
        {
            Debug.LogWarning("Requested buff count is less than or equal to 0");
            return new List<BuffStruct>();
        }

        selectedBuffs.Clear();
        tempNormalBuffs.Clear();
        tempEpicBuffs.Clear();

        // 캐시된 리스트 복사
        tempNormalBuffs.AddRange(cachedNormalBuffs);
        tempEpicBuffs.AddRange(cachedEpicBuffs);

        // 각 리스트 셔플
        ShuffleList(tempNormalBuffs);
        ShuffleList(tempEpicBuffs);

        int selectedCount = 0;
        while (selectedCount < count && (tempNormalBuffs.Count > 0 || tempEpicBuffs.Count > 0))
        {
            float chance = UnityEngine.Random.Range(0f, 100f);
            
            if (chance < EpicBuff_GetChance && tempEpicBuffs.Count > 0)
            {
                selectedBuffs.Add(tempEpicBuffs[0]);
                tempEpicBuffs.RemoveAt(0);
            }
            else if (tempNormalBuffs.Count > 0)
            {
                selectedBuffs.Add(tempNormalBuffs[0]);
                tempNormalBuffs.RemoveAt(0);
            }
            selectedCount++;
        }

        return selectedBuffs;
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public void OnBuffSelected(int buffId)
    {
        selectedBuffId = buffId;
        BuffStruct selectedBuff = selectedBuffs.Find(buff => buff.ID == buffId);
        
        if (selectedBuff != null)
        {
            // 광고 잠금 버프인지 확인
            int selectedIndex = selectedBuffs.IndexOf(selectedBuff);
            if (selectedIndex == adLockedBuffIndex)
            {
                // 광고 시청 요청
                if (AdManager.Instance != null)
                {
                    AdManager.Instance.ShowRewardedInterstitialAd("epicBuff", selectedIndex);
                    // 광고 보상 처리는 OnAdRewardEarned에서 수행
                    return;
                }
                else
                {
                    Debug.LogWarning("AdManager 인스턴스를 찾을 수 없습니다. 광고 없이 버프를 적용합니다.");
                }
            }
            
            // 일반 버프 적용
            ApplyBuff(selectedBuff.buffState);
            buffSelectCount++; // 버프 선택 횟수 증가
        }

        isBuffSelected = true;
        buffClickSound.Play();
        selectPanel.SetActive(false);
    }
    
    // 광고 보상 처리 함수 (AdManager에서 호출)
    public void OnAdRewardEarned(int buffIndex)
    {
        if (buffIndex >= 0 && buffIndex < selectedBuffs.Count)
        {
            BuffStruct selectedBuff = selectedBuffs[buffIndex];
            
            // 버프 적용
            ApplyBuff(selectedBuff.buffState);
            buffSelectCount++;
            
            // UI 업데이트 (광고 잠금 해제)
            if (buffIndex < buffUIs.Count)
            {
                buffUIs[buffIndex].UnlockAdBuff();
            }
        }
        
        isBuffSelected = true;
        buffClickSound.Play();
        selectPanel.SetActive(false);
    }

    private void ApplyBuff(Dictionary<string, float> buffs)
    {
        foreach (var buff in buffs)
        {
            playerState.ApplyBuff(buff.Key, buff.Value);

            if (buff.Key == "Enemy_Health" || buff.Key == "Enemy_Attack")
            {
                foreach (var enemy in FindObjectsOfType<EnemyStatus>())
                {
                    enemy.ApplyBuffToEnemy(playerState);
                }
            }
        }
    }

    public bool IsBuffSelected()
    {
        return isBuffSelected;
    }

    public int GetSelectedBuffId()
    {
        return selectedBuffId;
    }

    private void PlayOneShotSound(AudioSource source)
    {
        if (source == null || source.clip == null) return;

        GameObject tempAudioObj = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioObj.AddComponent<AudioSource>();
        tempAudio.clip = source.clip;
        tempAudio.outputAudioMixerGroup = source.outputAudioMixerGroup;
        tempAudio.volume = source.volume;
        tempAudio.spatialBlend = 0f;
        tempAudio.Play();

        Destroy(tempAudioObj, tempAudio.clip.length);
    }

    private void _debugPrintBuffData()
    {
        foreach (BuffStruct buff in cachedNormalBuffs)
        {
            Debug.Log($"{buff.ID} : {buff.Name}");
        }
        foreach (BuffStruct buff in cachedEpicBuffs)
        {
            Debug.Log($"{buff.ID} : {buff.Name}");
        }

        // [코드 리팩토링] 최초 실행시 키 값을 저장.
        selectedBuffs = new List<BuffStruct>();
        Debug.Log($"[BM] : 버프 테이블 로드 완료\n노말 버프: {cachedNormalBuffs.Count}개\n에픽 버프: {cachedEpicBuffs.Count}개");
    }
}
