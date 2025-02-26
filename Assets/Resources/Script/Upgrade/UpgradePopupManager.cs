using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePopupManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupName;     //팝업의 업그레이드 명 부분 텍스트     
    [SerializeField] private TextMeshProUGUI popupCost;     //팝업의 비용 부분 텍스트

    [SerializeField] private GameObject FailPopup;      //구매 실패 시 뜰 팝업

    [SerializeField] private int uogradableNum;     //구매 가능한 업그레이드의 번호

    [SerializeField] private UpgradeBtnManager UpgradeBtnManager;   //버튼들의 관리를 위한 메니저 스크립트

    private string upgradeName;     //팝업에 표시될 업그레이드 이름, 보안을 위해 프로퍼티로 설정
    private int upgradeStatName;    //업그레이드 구매 시 증가 될 스탯, 보안을 위해 프로퍼티로 설정
    private int upgradeStat;        //업그레이드 구매 시 증가 될 스탯 수치, 보안을 위해 프로퍼티로 설정
    private int upgradeCost;        //팝업에 표시될 업그레이드 비용, 보안을 위해 프로퍼티로 설정

    public string UpgradeName {  get { return upgradeName; } set { upgradeName = value; } }
    public int UpgradeStatName { get { return upgradeStatName; } set { upgradeStatName = value; } }
    public int UpgradeStat { get { return upgradeStat; } set { upgradeStat = value; } }
    public int UpgradeCost { get { return upgradeCost; } set { upgradeCost = value; } }



    //초기화
    private void Start()
    {
        FailPopup.SetActive(false);
        ClosePopup();
    }



    //팝업 닫기
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }


    public void BuyUpgrade()
    {
        //현제 보유 골드 받아오기
        int gold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));


        if (gold >= upgradeCost)
        {
            //업그레이드 구매가 가능하다면

            //골드 감소 처리
            gold -= upgradeCost;
            DataControl.SaveEncryptedDataToPrefs("Gold", gold.ToString());

            //전달받은 업그레이드 내용을 토대로 스탯 증가 처리
            switch(upgradeStatName)
            {
                case 0:
                    DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_HP",
                        (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP")) + upgradeStat).ToString());
                    break;

                case 1:
                    DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_ATK",
                        (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK")) + upgradeStat).ToString());
                    break;

                case 2:
                    DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_BALLCOUNT",
                        (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT")) + upgradeStat).ToString());
                    break;

                case 3:
                    DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_PINHP",
                        (int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP")) + upgradeStat).ToString());
                    break;
            }

            //업그레이드 번호 증가 처리
            DataControl.SaveEncryptedDataToPrefs("UpgradableNum",
                (int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradableNum")) + 1).ToString());


            //현재 구매 가능한 업그레이드의 번호가 바뀌었으니 이를 업그레이드 씬에 적용하기 위해 새로고침
            UpgradeBtnManager.RefreshUpgradeBtn();

            Debug.Log("구매 성공!");


            ClosePopup();
        }
        else 
        {
            //업그레이드 구매가 불가능하다면


            //구매 실패 팝업 띄우기
            FailPopup.SetActive(true);

            Debug.Log("구매 실패!");
            ClosePopup();
        }
    }

    //팝업 설정
    public void SetUpgradePopup()
    {
        popupName.text = upgradeName;
        popupCost.text = "cost: "+upgradeCost.ToString() + "G";
    }


}
