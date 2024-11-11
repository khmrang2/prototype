using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffEffectState
{
    public int? Player_Damage;
    public float? Player_CriticalChance;
    public int? Player_HealthIncrease;
    public float? Player_DoubleUpChance;
    public int? Player_ShieldPower;
    public float? Ball_Size;
    public int? Ball_Count;
    public float? Ball_Elasticity;
    public int? Ball_PiercePower;
    public int? Ball_BallSplitCount;
}

[System.Serializable]
public class BuffEffect
{
    public int ID;
    public string Tooltip;
    public string ImagePath;
    public BuffEffectState buffEffect;
}

[System.Serializable]
public class BuffDataList
{
    public List<BuffEffect> buffs;
}

public class BuffManager : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel; // SelectPanel 오브젝트
    [SerializeField] private TextAsset buffDataJSON; // JSON 파일
    private Dictionary<int, BuffEffect> buffTable = new Dictionary<int, BuffEffect>(); // ID로 접근할 수 있는 버프 테이블
    private bool isBuffSelected = false; // 버프가 선택되었는지 여부를 저장하는 플래그
    private int selectedBuffId = -1; // 선택된 Buff ID

    private void Start()
    {
        LoadBuffDataFromJSON();
        selectPanel.SetActive(false); // 시작 시 UI 패널을 비활성화
    }

    // JSON 파일에서 모든 버프를 로드하고 테이블에 저장
    private void LoadBuffDataFromJSON()
    {
        string jsonText = buffDataJSON.text;
        BuffDataList dataList = JsonUtility.FromJson<BuffDataList>(jsonText);

        foreach (BuffEffect buff in dataList.buffs)
        {
            buffTable[buff.ID] = buff;
        }
    }

    // UI 패널을 활성화하고, 임의의 3개 버프를 표시
    public void ShowBuffSelection()
    {
        isBuffSelected = false; // 초기화
        selectPanel.SetActive(true); // 패널 활성화
        ShowRandomBuffs();
    }

    // 임의의 3개의 버프를 선택하고, UI에 표시
    private void ShowRandomBuffs()
    {
        List<BuffEffect> selectedBuffs = GetRandomBuffs(3);

        for (int i = 0; i < selectedBuffs.Count; i++)
        {
            BuffEffect buff = selectedBuffs[i];
            GameObject buffSlot = selectPanel.transform.GetChild(i).gameObject;

            Image buffImage = buffSlot.transform.Find("buffImage").GetComponent<Image>();
            TextMeshProUGUI buffText = buffSlot.transform.Find("buffToolTip").GetComponent<TextMeshProUGUI>();

            buffImage.sprite = LoadSpriteFromPath(buff.ImagePath);
            buffText.text = buff.Tooltip;

            int buffId = buff.ID;
            buffSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            buffSlot.GetComponent<Button>().onClick.AddListener(() => OnBuffSelected(buffId));
        }
    }

    // 임의의 n개의 버프를 선택하는 함수
    private List<BuffEffect> GetRandomBuffs(int count)
    {
        List<BuffEffect> randomBuffs = new List<BuffEffect>(buffTable.Values);
        List<BuffEffect> selectedBuffs = new List<BuffEffect>();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, randomBuffs.Count);
            selectedBuffs.Add(randomBuffs[index]);
            randomBuffs.RemoveAt(index);
        }

        return selectedBuffs;
    }

    // 버프가 선택되었을 때 호출되는 함수
    public void OnBuffSelected(int buffId)
    {
        selectedBuffId = buffId;
        isBuffSelected = true; // 선택 완료
        selectPanel.SetActive(false); // 선택 완료 시 패널 비활성화
        Debug.Log("Selected Buff ID: " + buffId);
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

    // 이미지 경로로부터 스프라이트 로드 (Resources 폴더 사용 시) -? 왜 안되지..ㅅㅂ
    private Sprite LoadSpriteFromPath(string path)
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
    }

}

//// 버프 매니저
//// 1. json 파일을 load하여 buffeffect 테이블을 관리함.
//// 2. ui로 버프를 보여주고 적용하는 기능
//// 3. 
//public GameManager gameManager; // 게임 매니저 참조

//PlayerState playerDefaultState = null;

//[SerializeField] private BuffState buffState; // buffState 참조
//private List<BuffEffect> allBuffs = new List<BuffEffect>(); // 모든 활성화된 버프
//private List<BuffEffect> newBuffs = new List<BuffEffect>(); // 새로 추가된 버프

//private void Start()
//{
//    // 플레이어 스테이트를 최초 생성 해서 가져옴.
//    // => json으로 load해서 플레이어의 설정을 가져오는 형식으로 바꿀 수 있음.
//    playerDefaultState = new PlayerState();
//}

//// 새 버프를 추가하고 newBuffs에 저장
//public void addBuff(BuffEffect buff)
//{
//    newBuffs.Add(buff);
//}

//// 새 버프에 대해서 업데이트 해주어 buffState를 업데이트.
//public BuffState updateBuffState()
//{
//    foreach (BuffEffect buff in newBuffs)
//    {
//        ApplyBuffEffectToPlayer(buff);
//        allBuffs.Add(buff); // 적용한 버프를 allBuffs로 이동
//    }

//    newBuffs.Clear(); // newBuffs 초기화
//    return buffState;
//}

//// 인자로 들어온 버프를 계산하는 메소드
//private void ApplyBuffEffectToPlayer(BuffEffect buff)
//{
//    if (buff.Player_Damage.HasValue)
//        buffState.Player_Damage += buff.Player_Damage.Value;

//    if (buff.Player_CriticalChance.HasValue)
//        buffState.Player_CriticalChance += buff.Player_CriticalChance.Value;

//    if (buff.Player_HealthIncrease.HasValue)
//        buffState.Player_HealthIncrease += buff.Player_HealthIncrease.Value;

//    if (buff.Player_DoubleUpChance.HasValue)
//        buffState.Player_DoubleUpChance += buff.Player_DoubleUpChance.Value;

//    if (buff.Player_ShieldPower.HasValue)
//        buffState.Player_ShieldPower += buff.Player_ShieldPower.Value;

//    if (buff.Ball_Size.HasValue)
//        buffState.Ball_Size += buff.Ball_Size.Value;

//    if (buff.Ball_Count.HasValue)
//        buffState.Ball_Count += buff.Ball_Count.Value;

//    if (buff.Ball_Elasticity.HasValue)
//        buffState.Ball_Elasticity += buff.Ball_Elasticity.Value;

//    if (buff.Ball_PiercePower.HasValue)
//        buffState.Ball_PiercePower += buff.Ball_PiercePower.Value;

//    if (buff.Ball_BallSplitCount.HasValue)
//        buffState.Ball_BallSplitCount += buff.Ball_BallSplitCount.Value;
//}

//public BuffState getBuffState()
//{
//    return this.buffState;
//}
////----------------------------------------------------------------------------------
////----------------------------------------------------------------------------------
////                                  버프 ui 관리 공간
////----------------------------------------------------------------------------------
////----------------------------------------------------------------------------------
//[SerializeField] private GameObject buffUIPrefab; // UI 프리팹
//[SerializeField] private Transform uiParent; // UI가 배치될 부모 객체
//private List<GameObject> uiPool = new List<GameObject>(); // UI 오브젝트 풀링 리스트

////private void Start()
////{
////    InitializeUIPool(3); // 예시로 5개의 UI를 풀링해둠
////}

//// UI 오브젝트 풀을 초기화
//private void InitializeUIPool(int count)
//{
//    for (int i = 0; i < count; i++)
//    {
//        GameObject ui = Instantiate(buffUIPrefab, uiParent);
//        ui.SetActive(false); // 초기에는 비활성화
//        uiPool.Add(ui);
//    }
//}

//// UI를 활성화하고 데이터 업데이트
//public void ShowBuffUI(BuffEffect buffEffect)
//{
//    GameObject ui = GetAvailableUI();
//    if (ui != null)
//    {
//        ui.SetActive(true);
//        UpdateBuffUI(ui, buffEffect); // UI 요소를 버프 데이터로 업데이트
//    }
//}

//// 사용 가능한 UI를 반환
//private GameObject GetAvailableUI()
//{
//    foreach (var ui in uiPool)
//    {
//        if (!ui.activeInHierarchy)
//            return ui;
//    }
//    return null; // 사용 가능한 UI가 없으면 null 반환
//}

//// UI 업데이트 (버프 이미지, 툴팁 등 변경)
//private void UpdateBuffUI(GameObject ui, BuffEffect buffEffect)
//{
//    Image buffImage = ui.transform.Find("BuffImage").GetComponent<Image>();
//    Text buffTooltip = ui.transform.Find("BuffTooltip").GetComponent<Text>();

//    // 버프 이미지와 툴팁을 설정
//    buffImage.sprite = LoadSpriteFromPath(buffEffect.ImagePath);
//    buffTooltip.text = buffEffect.Tooltip;
//}

//// 이미지 경로로부터 스프라이트 로드 (Resources 폴더 사용 시)
//private Sprite LoadSpriteFromPath(string path)
//{
//    return Resources.Load<Sprite>(path);
//}

//// 모든 UI를 숨김 (필요시 전체 UI 초기화)
//public void HideAllUI()
//{
//    foreach (var ui in uiPool)
//    {
//        ui.SetActive(false);
//    }
//}