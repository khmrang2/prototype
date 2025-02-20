using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePopupManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupName;     //팝업의 업그레이드 명 부분 텍스트     
    [SerializeField] private TextMeshProUGUI popupCost;     //팝업의 비용 부분 텍스트

    [SerializeField] private GameObject FailPopup;      //구매 실패 시 뜰 팝업

    private string upgradeName;     //팝업에 표시될 업그레이드 이름, 보안을 위해 프로퍼티로 설정
    private int upgradeStat;        //업그레이드 구매 시 증가 될 스탯, 보안을 위해 프로퍼티로 설정
    private int upgradeCost;        //팝업에 표시될 업그레이드 비용, 보안을 위해 프로퍼티로 설정

    public string UpgradeName {  get { return upgradeName; } set { upgradeName = value; } }
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
            gold -= upgradeCost;
            //업그레이드 구매가 가능하다면
            DataControl.SaveEncryptedDataToPrefs("Gold", gold.ToString());

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
