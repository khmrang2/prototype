using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public ScrollSnap stageScroll;      // 현재 선택된 스테이지를 가져오기 위해 가져올 친구
    public DataControl datactr;         // 내가 어디 스테이지 이상을 못가게 할건지

    private int targetStage;            // 현재 플레이할 스테이지 번호
    public void StartStage()
    {
        int selectedIndex = stageScroll.getCurStage();

        // 현재 선택한 스테이지가 잠금 상태면 무시
        if (selectedIndex > DataController.Instance.currentStageIndex)
        {
            Debug.Log("이 스테이지는 아직 잠겼습니다!");
            return;
        }

        string sceneName = $"Stage_{selectedIndex + 1}";
        SceneManager.LoadScene(sceneName);
    }
}
