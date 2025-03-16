using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//업그레이드 버튼 별로 증가시킬 스탯의 종류를 지정하기 위한 열거형(enum) 정의
public enum UpgradeStatName
{
    HP = 0,
    ATK = 1,
    BallCount = 2,
    PinHP = 3
}


public class UpgradeBtn : MonoBehaviour
{

    [Header("Upgrade button Images")]

    [SerializeField] private Sprite PurchasableUpgrade;
    [SerializeField] private Sprite PurchasedUpgrade;
    [SerializeField] private Sprite UnpurchasableUpgrade;


    [Header("Upgrade button parameters")]

    public Button btn;    //스스로의 활성 & 비활성을 처리하기 위한 버튼
    //public UpgradeBtnManager manager;    // 버튼 매니져 // 신호를 주기 위해서

    [SerializeField] private GameObject upgradePopUp;   //버튼 터치 시 등장할 팝업
    private UpgradePopupManager popupManager;           //등장할 펍업에 전달될 데이터 관리를 위한 클래스


    [Header("Upgrade button settings")]

    [SerializeField] private string upgradeName;  //해당 버튼을 통해 이루어질 업그레이드의 이름  // 팝업에 등장
    [SerializeField] private int upgradeStat;     //해당 버튼을 통해 이루어질 업그레이드의 수치
    [SerializeField] private int upgradeCost;    //해당 버튼을 통해 이루어질 업그레이드의 비용

    [SerializeField] private bool isUpgradable;  //해당 업그레이드가 구매 가능한지 나타내는 값, 보안을 위해 프로퍼티로 구현
    [SerializeField] private int upgradeNum;    //업그레이드간의 구별을 위한 고유값, 단계적 해금 및 구매기록 저장과 연계 예정, 보안을 위해 프로퍼티로 구현

    public bool IsUpgradable { get { return isUpgradable; } set { isUpgradable = value; } }

    public int UpgradeNum { get { return upgradeNum; } }

    public UpgradeStatName upgradeStatName;     //업그레이드 버튼 별로 증가시킬 스탯의 종류를 지정하기 위한 열거형





    #region 초기화
    void Start()
    {
        //팝업의 데이터 관리를 위해 UpgradePopupManager을 호출
        popupManager = upgradePopUp.GetComponent<UpgradePopupManager>();

        // 기존 버튼에 클릭 이벤트 추가
        btn.onClick.AddListener(showButtons);

        //업그레이드 가능 여부에 따라 스스로를 활성 or 비활성화
        CheckUpgradable();

    }

    #endregion



    #region 팝업 관련 메소드

    //팝업 활성화
    public void showButtons()
    {

        //팝업에 데이터 전달
        popupManager.UpgradeName = upgradeName;
        popupManager.UpgradeStat = upgradeStat;
        popupManager.UpgradeCost = upgradeCost;
        popupManager.UpgradeStatName = (int)upgradeStatName;


        //팝업 설정
        popupManager.SetUpgradePopup();

        //팝업 활성화
        upgradePopUp.SetActive(true);

    }


    //팝업 닫기
    public void hideButtons()
    {
        // 새 버튼 활성화
        upgradePopUp.SetActive(false);

    }

    #endregion



    #region 업그레이드 버튼 활성화 관련 메소드


    //해당 업그레이드가 구매 가능한지 확인하는 메소드
    public void CheckUpgradable()
    {
        //현제 구매 가능한 업그레이드의 번호를 받아옴
        int upNum = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradableNum"));

        //이 업그레이드의 번호와 현제 구매 가능한 업그레이드의 번호가 같은지 확인
        if (upNum == UpgradeNum) 
        {
            //같다면 해당 버튼의 상호작용 여부를 true로 변경
            IsUpgradable = true;

            //이미지 변경
            ChangeButtonImage(upNum);

            //상호작용 가능 처리
            btn.interactable = true;

        }
        else
        {
            //다르다면 해당 버튼의 상호작용 여부를 false로 변경
            IsUpgradable = false;

            //이미지 변경
            ChangeButtonImage(upNum);

            //상호작용 불가능 처리
            btn.interactable = false;
        }
    }



    //업그레이드 구매 가능 여부에 따른 버튼 이미지 변화 메소드
    private void ChangeButtonImage(int upgradableNum)
    {

        //버튼의 이미지를 담당하는 자식 오브젝트 bg 찾기
        Transform bgTransform = transform.Find("bg");


        if (IsUpgradable)
        {
            //업그레이드가 가능하다면 구매 가능 스프라이트로 변경
            
            if (bgTransform != null)
            {
                Image bgImage = bgTransform.GetComponent<Image>();
                if (bgImage != null)
                {
                    bgImage.sprite = PurchasableUpgrade; // Source Image 변경
                }
                else
                {
                    Debug.LogError("bg 오브젝트에 Image 컴포넌트가 없음!");
                }
            }
            else
            {
                Debug.LogError("bg 오브젝트를 찾을 수 없음!");
            }


        }
        else
        {
            //업그레이드가 불가능하다면
            //이미 구매한 업그레이드인지, 아직 구매 불가능한 업그레이드인지 확인

            if (UpgradeNum < upgradableNum) 
            {
                //이미 구매한 업그레이드라면 해당 이미지로 변경

                if (bgTransform != null)
                {
                    Image bgImage = bgTransform.GetComponent<Image>();
                    if (bgImage != null)
                    {
                        bgImage.sprite = PurchasedUpgrade; // Source Image 변경
                    }
                    else
                    {
                        Debug.LogError("bg 오브젝트에 Image 컴포넌트가 없음!");
                    }
                }
                else
                {
                    Debug.LogError("bg 오브젝트를 찾을 수 없음!");
                }
            }
            else if(UpgradeNum > upgradableNum)
            {
                //아직 구매할 수 없는 업그레이드라면 해당 이미지로 변경

                if (bgTransform != null)
                {
                    Image bgImage = bgTransform.GetComponent<Image>();
                    if (bgImage != null)
                    {
                        bgImage.sprite = UnpurchasableUpgrade; // Source Image 변경
                    }
                    else
                    {
                        Debug.LogError("bg 오브젝트에 Image 컴포넌트가 없음!");
                    }
                }
                else
                {
                    Debug.LogError("bg 오브젝트를 찾을 수 없음!");
                }
            }

        }
    }


    #endregion


}
