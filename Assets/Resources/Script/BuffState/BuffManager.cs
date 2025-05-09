using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class BuffStruct
{
    public int ID;
    public string Name;
    public string Tooltip;
    public string ImagePath;
    public Dictionary<string, float> buffState; // 키-값 형태로 변경
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
    private TextAsset buffDataJSON;
    private Dictionary<int, BuffStruct> buffTable = new Dictionary<int, BuffStruct>();
    private BuffDataList dataList;

    [Header("Buff Selection State")]
    private List<BuffStruct> selectedBuffs;
    private bool isBuffSelected = false;
    private int selectedBuffId = -1;

    private List<BuffSelectUI> activeUIs = new List<BuffSelectUI>();

    private void Awake()
    {
        // 유지 보수를 편하게 해보자. 
        if (playerState == null) playerState = FindObjectOfType<PlayerState>();
    }

    private void Start()
    {
        LoadBuffDataFromJSON();
        selectPanel.SetActive(false); // 시작 시 UI 패널을 비활성화
    }

    // JSON 파일에서 모든 버프를 로드하고 테이블에 저장
    private void LoadBuffDataFromJSON()
    {
        buffDataJSON = Resources.Load<TextAsset>("Data/BuffData");
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonConvert.DeserializeObject<BuffDataList>(jsonText);

        // 데이터를 읽고 각 버프 속성을 출력
        foreach (BuffStruct buff in dataList.buffs)
        {
            Debug.Log($"ID: {buff.ID}, Name: {buff.Name}");
            buffTable[buff.ID] = buff;
        }
    }

    // 1. UI 패널을 활성화하고, 임의의 3개 버프를 표시
    public void ShowBuffSelection()
    {
        isBuffSelected = false; // 초기화
        buffPanelActiveSound.Play();
        selectPanel.SetActive(true); // 패널 활성화
        ShowRandomBuffs();
    }

    // 2. 임의의 3개의 버프를 선택하고, UI에 표시
    private void ShowRandomBuffs()
    {
        selectedBuffs = GetRandomBuffs(3);
        for (int i = 0; i < 3; i++)
        {
            buffUIs[i].getBuffState(selectedBuffs[i]);
            buffUIs[i].RegisterBuffSelectionCallback(OnBuffSelected);
        }
    }

    #region 2-1. 임의의 3개의 버프를 선택하는 함수.

    // 2-1. 임의의 n개의 버프를 선택하는 함수
    private List<BuffStruct> GetRandomBuffs(int count)
    {
        selectedBuffs = new List<BuffStruct>();
        List<int> keys = new List<int>(buffTable.Keys); // 모든 키 값을 리스트로 저장

        // Fisher-Yates 알고리즘으로 키 리스트 셔플
        for (int i = keys.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = keys[i];
            keys[i] = keys[j];
            keys[j] = temp;
        }

        // 셔플된 리스트에서 count개만큼 버프 선택 (리스트 길이보다 count가 클 수 있으므로 조건 확인)
        for (int i = 0; i < count && i < keys.Count; i++)
        {
            int key = keys[i];
            if (buffTable.TryGetValue(key, out BuffStruct effect))
            {
                selectedBuffs.Add(effect);
            }
        }
        Debug.LogError($"{buffTable.Count} 버프테이블 크기 확인.");
        Debug.LogError($"{selectedBuffs.Count} 뽑힌 버프의 크기 확인.");

        return selectedBuffs;
    }
    #endregion 

    // 3. 버프가 선택되었을 때 호출되는 함수
    public void OnBuffSelected(int buffId)
    {
        selectedBuffId = buffId;
        BuffStruct selectedBuff;
        // 이제 여기에다가 버프를 적용하는 시퀀스를 넣자.
        if (buffTable.TryGetValue(buffId, out selectedBuff))
        {
            // 선택된 버프의 buffState를 ApplyBuff에 전달
            ApplyBuff(selectedBuff.buffState);
        }
        else
        {
            Debug.LogWarning("Buff with ID " + buffId + " not found.");
        }
        // 이제 게임매니저의 버프스테이트를 넣어주자.
        isBuffSelected = true; // 선택 완료
        buffClickSound.Play();
        selectPanel.SetActive(false); // 선택 완료시 버프 창을 비활성화.
    }

    // 버프 선택 완료 여부 확인 메서드
    public bool IsBuffSelected()
    {
        return isBuffSelected;
    }

    // 선택된 Buff ID 반환
    public int GetSelectedBuffId()
    {
        return selectedBuffId;
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

    // 이미지 경로로부터 스프라이트 로드 (Resources 폴더 사용 시) -? 왜 안되지..ㅅㅂ
/*    private Sprite LoadSpriteFromPath(string path)
    {
        //Debug.Log(path);

        Sprite loadedSprite = Resources.Load<Sprite>("Image/BuffImage/swordicon");

        if (loadedSprite != null)
        {
            Debug.Log($"Loaded sprite: {loadedSprite.name}, Size: {loadedSprite.rect.size}");
        }
        else
        {
            Debug.LogError("Failed to load sprite at path: " + "Image/BuffImage/swordicon");
        }

        return loadedSprite;
    }*/

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
}