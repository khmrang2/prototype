using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    public Button btn;    // 기존 버튼
    public UpgradeBtnManager manager;    // 버튼 매니져 // 신호를 주기 위해서

    [SerializeField] private GameObject upgradePopUp;   //버튼 터치 시 등장할 팝업
    private UpgradePopupManager popupManager;           //등장할 펍업에 전달될 데이터 관리를 위한 클래스

    [SerializeField] private string upgradeName;  //해당 버튼을 통해 이루어질 업그레이드의 이름  // 팝업에 등장
    [SerializeField] private int upgradeStat;     //해당 버튼을 통해 이루어질 업그레이드의 수치
    [SerializeField] private int upgradeCost;    //해당 버튼을 통해 이루어질 업그레이드의 비용

    [SerializeField] private bool isUpgradable;  //해당 업그레이드가 구매 가능한지 나타내는 값, 보안을 위해 프로퍼티로 구현
    [SerializeField] private int upgradeNum;    //업그레이드간의 구별을 위한 고유값, 단계적 해금 및 구매기록 저장과 연계 예정, 보안을 위해 프로퍼티로 구현

    public bool IsUpgradable { get { return isUpgradable; } set { isUpgradable = value; } }

    public int UpgradeNum { get { return upgradeNum; } }


    public enum UpgradeStatName
    {
        HP,
        ATK,
        BallCount,
        PinHP
    }


    void Start()
    {
        //팝업의 데이터 관리를 위해 UpgradePopupManager을 호출
        popupManager = upgradePopUp.GetComponent<UpgradePopupManager>();

        // 기존 버튼에 클릭 이벤트 추가
        btn.onClick.AddListener(showButtons);
    }



    //팝업 활성화
    public void showButtons()
    {
        //팝업에 데이터 전달
        popupManager.UpgradeName = upgradeName;
        popupManager.UpgradeStat = upgradeStat;
        popupManager.UpgradeCost = upgradeCost;

        //팝업 설정
        popupManager.SetUpgradePopup();

        //팝업 활성화
        upgradePopUp.SetActive(true);

        // 버튼 트래킹.
        //manager.trackingBtn(this);
    }


    //팝업 닫기
    public void hideButtons()
    {
        // 새 버튼 활성화
        upgradePopUp.SetActive(false);

    }



}
