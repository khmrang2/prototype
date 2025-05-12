using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

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

    [Header("Buff Data")]
    [SerializeField] private TextAsset buffDataJSON;
    [SerializeField] private float NormalBuff_GetChance = 95.0f;
    [SerializeField] private float EpicBuff_GetChance = 5.0f;
    [SerializeField] private List<Dictionary<int, BuffStruct>> _BuffTable;

    private Dictionary<int, BuffStruct> NormalBuffTable = new Dictionary<int, BuffStruct>();
    private Dictionary<int, BuffStruct> EpicBuffTable = new Dictionary<int, BuffStruct>();
    private List<BuffStruct> cachedNormalBuffs = new List<BuffStruct>();
    private List<BuffStruct> cachedEpicBuffs = new List<BuffStruct>();
    private List<BuffStruct> selectedBuffs = new List<BuffStruct>();
    private List<BuffStruct> tempNormalBuffs = new List<BuffStruct>();
    private List<BuffStruct> tempEpicBuffs = new List<BuffStruct>();
    private bool isBuffSelected = false;
    private int selectedBuffId = -1;

    private void Awake()
    {
        if (playerState == null) playerState = FindObjectOfType<PlayerState>();
    }

    private void Start()
    {
        LoadBuffDataFromJSON();
        CacheBuffs();
        InitializeTempLists();
        selectPanel.SetActive(false);
    }

    private void InitializeTempLists()
    {
        tempNormalBuffs = new List<BuffStruct>(cachedNormalBuffs.Count);
        tempEpicBuffs = new List<BuffStruct>(cachedEpicBuffs.Count);
    }

    private void LoadBuffDataFromJSON()
    {
        buffDataJSON = Resources.Load<TextAsset>("Data/BuffData");
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonConvert.DeserializeObject<BuffDataList>(jsonText);

        foreach (BuffStruct buff in dataList.buffs)
        {
            //Debug.Log($"ID: {buff.ID}, Name: {buff.Name}");
            if (buff.Rank == BuffRank.Normal)
            {
                //buffTable[buff.ID] = buff;
                NormalBuffTable.Add(buff.ID, buff);
            }
            else if (buff.Rank == BuffRank.Epic)
            {
                EpicBuffTable.Add(buff.ID, buff);
            }
            else
            {
                Debug.LogError("? 너가 실행되면 안되는데 ");
            }
        }
    }

    private void CacheBuffs()
    {
        cachedNormalBuffs = new List<BuffStruct>(NormalBuffTable.Values);
        cachedEpicBuffs = new List<BuffStruct>(EpicBuffTable.Values);
    }

    // 1. UI 패널을 활성화하고, 임의의 3개 버프를 표시
    public void ShowBuffSelection()
    {
        isBuffSelected = false;
        selectedBuffs.Clear();
        buffPanelActiveSound.Play();
        selectPanel.SetActive(true);
        ShowRandomBuffs();
    }

    private void ShowRandomBuffs()
    {
        selectedBuffs = GetRandomBuffs(3);
        for (int i = 0; i < 3; i++)
        {
            buffUIs[i].getBuffState(selectedBuffs[i]);
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
            float chance = Random.Range(0f, 100f);
            
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

        if (selectedCount < count)
        {
            Debug.LogWarning($"Could not select {count} buffs. Only selected {selectedCount} buffs.");
        }

        return selectedBuffs;
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
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
            ApplyBuff(selectedBuff.buffState);
        }
        else
        {
            Debug.LogWarning($"Buff with ID {buffId} not found.");
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

        // 임시 오브젝트 생성
        GameObject tempAudioObj = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioObj.AddComponent<AudioSource>();
        tempAudio.clip = source.clip;
        tempAudio.outputAudioMixerGroup = source.outputAudioMixerGroup; // 믹서 연결 유지
        tempAudio.volume = source.volume;
        tempAudio.spatialBlend = 0f; // 2D
        tempAudio.Play();

        Destroy(tempAudioObj, tempAudio.clip.length);
    }

    private void _debugPrintBuffData()
    {
        foreach (BuffStruct buff in NormalBuffTable.Values)
        {
            Debug.Log($"{buff.ID} : {buff.Name}");
        }
        foreach (BuffStruct buff in EpicBuffTable.Values)
        {
            Debug.Log($"{buff.ID} : {buff.Name}");
        }

        // [코드 리팩토링] 최초 실행시 키 값을 저장.
        selectedBuffs = new List<BuffStruct>();
        Debug.Log($"[BM] : 버프 테이블 로드 완료\n노말 버프: {NormalBuffTable.Count}개\n에픽 버프: {EpicBuffTable.Count}개");
    }
}
