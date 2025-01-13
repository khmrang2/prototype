using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class TabUIManager : MonoBehaviour
{
    // 스크린 배열
    [SerializeField] private GameObject[] screens;
    // 탭 배열
    [SerializeField] private GameObject[] tabButtons;


    // 최초 실행시에 screens 배열 메모리 할당.
    // 최초 화면 구성은 플레이 화면으로.
    private void Start()
    {
        ShowScreen(2);
    }

    // ShowScreen(int btnId)
    //  : 탭 버튼들의 입력을 감지하여 해당 버튼에 할당된 화면으로 전환한다.
    //  : 감지되면, 버튼의 아이콘 위치(activePositionOffset), 배경을 잘 바꿔준다.
    // @param : int btnId : btnId
    //  : 는 각 버튼들이 invoke 해주는 index id 이다.
    public void ShowScreen(int btnId)
    {
        // 예외 처리.
        if(btnId < 0 || btnId > 5)
        {
            Debug.LogWarning("[TabUIManager] : Wrong btnId" + btnId);
            return;
        }

        // btnId의 invoke된 화면을 제외한 화면들 비활성화.
        for(int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].GetComponent<TabButton>().Deactivate();
            screens[i].SetActive(false);
        }
        // btnId 화면 활성화.
        screens[btnId].SetActive(true);
        tabButtons[btnId].GetComponent<TabButton>().Activate();
    }
}

