using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    // 스테이지들 관리 해야함.
    // 1. 리스트 만들어서, 리스트보다 작은 프리팹은 자물쇠 활성화 시키기.
    // 2. int Parse(스테이지넘버)를 주기.
    public InstrumentHandController InstrumentHandController;
    public ScrollSnap scrollSnap;
    public List<GameObject> stageList;
    
    // StageDataScript 참조 추가
    public StageDataScript stageDataScript;
    // 스테이지 UI를 생성할 부모 객체
    public RectTransform stageContentParent;

    // 현재 플레이어가 열 수 있는 마지막 스테이지 번호
    private int unlockedStageNum = 0;
    private int lastSelectedStage = -1; // 중복 호출 방지용
    
    // 현재 선택된 스테이지 인덱스
    private int currentSelectedStageIndex = 0;
    // 현재 선택된 스테이지 데이터
    private StageData currentSelectedStageData;

    [Header("스테이지 시스템 : 테스트를 위한 디버깅 체크")]
    public bool debuging = false;
    public int stage = 0;

    void Start()
    {
        // 스테이지 데이터 초기화
        InitializeStageData();
        
        // 스테이지 UI 생성
        CreateStageUI();
        
        // 초기 스테이지 선택
        SetInitialStage();
        
        // 스테이지 잠금 UI 업데이트
        UpdateStageLockUI();
        
        // 이벤트 등록
        scrollSnap.OnStageSelected += OnStageChanged;
    }
    
    private void OnDestroy()
    {
        // 이벤트 해제
        if (scrollSnap != null)
        {
            scrollSnap.OnStageSelected -= OnStageChanged;
        }
    }
    
    // 스테이지 데이터 초기화
    private void InitializeStageData()
    {
        // PlayerStatusInMain에서 현재 열린 스테이지 번호 불러옴
        if (PlayerStatusInMain.Instance != null)
        {
            unlockedStageNum = PlayerStatusInMain.Instance.GetCurStage();
            if (debuging) unlockedStageNum = stage;
        }
        else
        {
            Debug.LogError("❌ PlayerStatusInMain 인스턴스를 찾을 수 없습니다.");
        }
        
        // StageDataScript 로드
        LoadStageData();
    }

    // 스테이지 데이터를 로드하는 메소드
    private void LoadStageData()
    {
        if (stageDataScript == null)
        {
            // Resources 폴더에서 StageDataList 에셋을 로드
            stageDataScript = Resources.Load<StageDataScript>("Script/Stage/StageDataList");
            
            if (stageDataScript == null)
            {
                Debug.LogError("❌ StageDataList를 Resources 폴더에서 찾을 수 없습니다.");
            }
        }
    }

    // 스테이지 UI를 동적으로 생성하는 함수
    private void CreateStageUI()
    {
        // 기존 자식 오브젝트 모두 제거 (이미 있는 경우)
        for (int i = stageContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(stageContentParent.GetChild(i).gameObject);
        }

        stageList = new List<GameObject>();

        // StageDataScript에서 스테이지 데이터를 가져와 UI 생성
        if (stageDataScript != null && stageDataScript.stages != null)
        {
            foreach (StageData stageData in stageDataScript.stages)
            {
                // 스테이지 프리팹 인스턴스화
                GameObject stageObj = Instantiate(stageDataScript.stagePrefab, stageContentParent);
                stageList.Add(stageObj);

                // UI 설정
                _UIStage uiStage = stageObj.GetComponent<_UIStage>();
                if (uiStage != null)
                {
                    uiStage.setUpStageUI(stageData.stageName, stageData.stageNumber, stageData.stageThumbnail);
                }

                // 잠금 UI 추가 (기본적으로 비활성화)
                Transform lockObj = stageObj.transform.Find("Lock");
                if (lockObj != null)
                {
                    lockObj.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("❌ StageDataScript가 할당되지 않았거나 스테이지 데이터가 없습니다.");
        }

        // ScrollSnap 컴포넌트에 콘텐츠 업데이트 알림
        if (scrollSnap != null)
        {
            scrollSnap.UpdateContent();
        }
    }

    // 스테이지 잠금 UI 업데이트
    private void UpdateStageLockUI()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            Transform lockObj = stageList[i].transform.Find("Lock");
            if (lockObj == null) continue;

            int stageNumber = GetStageNumberFromIndex(i);
            bool isLocked = stageNumber > unlockedStageNum;
            lockObj.gameObject.SetActive(isLocked);
        }
    }

    /// <summary>
    /// 외부에서 호출 시, 해당 스테이지가 열려 있는지 여부를 알려줌
    /// </summary>
    public bool IsStageUnlocked(int index)
    {
        // 스테이지 인덱스를 스테이지 번호로 변환
        int stageNumber = GetStageNumberFromIndex(index);
        bool isUnlocked = stageNumber <= unlockedStageNum;
        
        if (debuging)
        {
            Debug.Log($"스테이지 인덱스 {index}(스테이지 번호 {stageNumber}) 잠금 상태: {(isUnlocked ? "열림" : "잠김")}. 현재 열린 최대 스테이지: {unlockedStageNum}");
        }
        
        return isUnlocked;
    }
    
    /// <summary>
    /// 스테이지 인덱스로부터 실제 스테이지 번호를 가져옵니다.
    /// </summary>
    private int GetStageNumberFromIndex(int index)
    {
        if (index >= 0 && index < stageDataScript.stages.Count)
        {
            return stageDataScript.stages[index].stageNumber;
        }
        return -1; // 유효하지 않은 인덱스
    }
    
    /// <summary>
    /// 스테이지 번호로부터 인덱스를 가져옵니다.
    /// </summary>
    private int GetIndexFromStageNumber(int stageNumber)
    {
        for (int i = 0; i < stageDataScript.stages.Count; i++)
        {
            if (stageDataScript.stages[i].stageNumber == stageNumber)
            {
                return i;
            }
        }
        return -1; // 해당 번호의 스테이지를 찾지 못함
    }
    
    /// <summary>
    /// 현재 선택된 스테이지 인덱스를 반환합니다.
    /// </summary>
    public int GetCurrentStageIndex()
    {
        return currentSelectedStageIndex;
    }
    
    /// <summary>
    /// 현재 선택된 스테이지의 번호를 반환합니다.
    /// </summary>
    public int GetCurrentStageNumber()
    {
        if (currentSelectedStageIndex >= 0 && currentSelectedStageIndex < stageDataScript.stages.Count)
        {
            return stageDataScript.stages[currentSelectedStageIndex].stageNumber;
        }
        return 0;
    }
    
    /// <summary>
    /// 현재 선택된 스테이지의 이름을 반환합니다.
    /// </summary>
    public string GetCurrentStageName()
    {
        if (currentSelectedStageIndex >= 0 && currentSelectedStageIndex < stageDataScript.stages.Count)
        {
            return stageDataScript.stages[currentSelectedStageIndex].stageName;
        }
        return string.Empty;
    }
    
    /// <summary>
    /// 현재 선택된 스테이지 데이터를 반환합니다.
    /// </summary>
    public StageData GetCurrentStageData()
    {
        return currentSelectedStageData;
    }
    
    private void OnStageChanged(int selectedStage)
    {
        if (selectedStage == lastSelectedStage) return;
        lastSelectedStage = selectedStage;
        
        // 현재 선택된 스테이지 인덱스 업데이트
        currentSelectedStageIndex = selectedStage;
        
        // 현재 선택된 스테이지 데이터 업데이트
        if (stageDataScript != null && selectedStage >= 0 && selectedStage < stageDataScript.stages.Count)
        {
            currentSelectedStageData = stageDataScript.stages[selectedStage];
        }

        // 바늘 위치 업데이트
        int stageNumber = GetStageNumberFromIndex(selectedStage);
        if (stageNumber <= unlockedStageNum)
        {
            InstrumentHandController.SetStage(selectedStage);
        }
        else
        {
            InstrumentHandController.ShowTempStage(selectedStage, GetIndexFromStageNumber(unlockedStageNum));
        }
    }

    // 초기 스테이지 선택 메소드 추가
    private void SetInitialStage()
    {
        // 잠금 해제된 마지막 스테이지의 인덱스 찾기
        int unlockedStageIndex = GetIndexFromStageNumber(unlockedStageNum);
        
        if (unlockedStageIndex >= 0 && unlockedStageIndex < stageList.Count)
        {
            // 바늘 위치 설정
            InstrumentHandController.SetStage(unlockedStageIndex);
            
            // 현재 선택된 스테이지 인덱스 설정
            currentSelectedStageIndex = unlockedStageIndex;
            
            // 현재 선택된 스테이지 데이터 업데이트
            if (stageDataScript != null && unlockedStageIndex < stageDataScript.stages.Count)
            {
                currentSelectedStageData = stageDataScript.stages[unlockedStageIndex];
            }
            
            // ScrollSnap에 초기 페이지 설정
            if (scrollSnap != null)
            {
                scrollSnap.SetPage(unlockedStageIndex);
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ 잠금 해제된 스테이지 번호 {unlockedStageNum}에 해당하는 인덱스를 찾을 수 없습니다.");
        }
    }
}
