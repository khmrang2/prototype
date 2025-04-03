using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // 스테이지들 관리 해야함.
    // 1. 리스트 만들어서, 리스트보다 작은 프리팹은 자물쇠 활성화 시키기.
    // 2. int Parse(스테이지넘버)를 주기.
    public InstrumentHandController InstrumentHandController;
    public ScrollSnap scrollSnap;
    public List<GameObject> stageList;

    // 현재 플레이어가 열 수 있는 마지막 스테이지 번호
    private int unlockedStageNum = 0;
    private int lastSelectedStage = -1; // 중복 호출 방지용

    [Header("스테이지 시스템 : 테스트를 위한 디버깅 체크")]
    public bool debuging = false;
    public int stage = 0;

    void Start()
    {
        // PlayerStatusInMain에서 현재 열린 스테이지 번호 불러옴
        if (PlayerStatusInMain.Instance != null)
        {
            unlockedStageNum = PlayerStatusInMain.Instance.GetCurStage();
            if(debuging) unlockedStageNum = stage;
        }
        else
        {
            Debug.LogError("❌ PlayerStatusInMain 인스턴스를 찾을 수 없습니다.");
        }

        scrollSnap.OnStageSelected += OnStageChanged;
        InstrumentHandController.SetStage(unlockedStageNum); // 초기 바늘 위치 세팅

        stageLockUI();
    }

    // 그냥 있는 스테이지 content에서 현재 클리어하지 못하는 스테이지임을 가시화한다.
    private void stageLockUI()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            Transform lockObj = stageList[i].transform.Find("Lock");

            if (lockObj == null)
            {
                continue;
            }

            bool isLocked = i >= unlockedStageNum;
            lockObj.gameObject.SetActive(isLocked);
        }
    }

    /// <summary>
    /// 외부에서 호출 시, 해당 스테이지가 열려 있는지 여부를 알려줌
    /// </summary>
    public bool IsStageUnlocked(int index)
    {
        return index <= unlockedStageNum;
    }
    private void OnStageChanged(int selectedStage)
    {
        if (selectedStage == lastSelectedStage) return;
        lastSelectedStage = selectedStage;

        if (selectedStage <= unlockedStageNum)
        {
            InstrumentHandController.SetStage(selectedStage);
        }
        else
        {
            InstrumentHandController.ShowTempStage(selectedStage, unlockedStageNum);
        }
    }
}
