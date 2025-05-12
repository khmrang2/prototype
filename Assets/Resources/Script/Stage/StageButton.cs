using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public StageManager stageManager;   // 스테이지 매니저 참조

    private void Start()
    {
        // StageManager 참조가 없으면 자동으로 찾기
        if (stageManager == null)
        {
            stageManager = FindObjectOfType<StageManager>();
        }
    }
    
    public void StartStage()
    {
        if (stageManager == null)
        {
            Debug.LogError("StageManager 참조가 없습니다.");
            return;
        }
        
        // StageManager에서 현재 선택된 스테이지 정보 가져오기
        int currentStageIndex = stageManager.GetCurrentStageIndex();
        int targetStage = stageManager.GetCurrentStageNumber();
        
        // 잠긴 스테이지는 진입 불가
        if (!stageManager.IsStageUnlocked(currentStageIndex))
        {
            Debug.Log($"스테이지 {targetStage}는 잠겨있어 진입할 수 없습니다.");
            return;
        }

        string sceneName = $"Stage{targetStage}";
        Debug.Log($"시작 버튼 눌림 : 씬 전환 : {sceneName}을 불러옴");
        SceneTransitionManager.CaptureScreenAndLoad(sceneName);
    }
}
