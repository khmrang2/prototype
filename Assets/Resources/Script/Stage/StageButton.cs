using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public ScrollSnap stageScroll;      // 현재 선택된 스테이지를 가져오기 위해 가져올 친구

    private int targetStage;            // 현재 플레이할 스테이지 번호
    public void StartStage()
    {
        targetStage = stageScroll.getCurStageUI();
        //Debug.Log($"{targetStage}를 현재 보고 있고, {int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayingStageNum"))}가 있음.");

        string sceneName = $"Stage{targetStage}";
        //Debug.Log($"✅ 스테이지 {targetStage} 진입 시도 → 씬 이름: {sceneName}");
        Debug.Log($"시작 버튼 눌림 : 씬 전환 : {sceneName}을 불러옴");
        SceneTransitionManager.CaptureScreenAndLoad(sceneName);
    }
}
