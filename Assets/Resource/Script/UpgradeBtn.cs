using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    public Button btn;    // 기존 버튼
    public UpgradeBtnManager manager;    // 버튼 매니져 // 신호를 주기 위해서
    public GameObject yesButton; // 프리팹에 존재하는 yes버튼
    public GameObject noButton; // 미리 준비된 버튼 2

    void Start()
    {
        // 초기 상태에서 새 버튼 비활성화
        yesButton.SetActive(false);
        noButton.SetActive(false);

        // 기존 버튼에 클릭 이벤트 추가
        btn.onClick.AddListener(showButtons);
    }

    public void showButtons()
    {
        // 새 버튼 활성화
        yesButton.SetActive(true);
        noButton.SetActive(true);
        // 버튼 트래킹.
        manager.trackingBtn(this);
    }

    public void hideButtons()
    {
        // 새 버튼 활성화
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }
}
