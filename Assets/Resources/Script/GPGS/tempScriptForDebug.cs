using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class tempScriptForDebug : MonoBehaviour
{
    [SerializeField] private UpgradeBtnManager UpgradeBtnManager;   //버튼들의 관리를 위한 메니저 스크립트

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI upgradeStoneText;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI ATKText;
    public TextMeshProUGUI BcText;
    public TextMeshProUGUI PPText;


    private void Start()
    {
        goldText.text = "0";
        upgradeStoneText.text = "0";
        HpText.text = "0";
        ATKText.text = "0";
        BcText.text = "0";
        PPText.text = "0";

        goldText.text = DataControl.LoadEncryptedDataFromPrefs("Gold");
        Debug.Log("첫 시작 " + goldText.text + " 골드 현재량 ");
        upgradeStoneText.text = DataControl.LoadEncryptedDataFromPrefs("UpgradeStone");
        HpText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP");
        ATKText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK");
        BcText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT");
        PPText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP");
    }

    private void Update()
    {
        goldText.text = DataControl.LoadEncryptedDataFromPrefs("Gold");
        upgradeStoneText.text = DataControl.LoadEncryptedDataFromPrefs("UpgradeStone");
        HpText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP");
        ATKText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK");
        BcText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT");
        PPText.text = DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP");
    }

    public void AddGold()
    {
        int tempGold = int.Parse(goldText.text) + 100;
        //DataControl.SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
        PlayerStatusInMain.Instance.getGold(100);
        goldText.text = tempGold.ToString(); 
    }


    public void MinusGold()
    {
        int tempGold = int.Parse(goldText.text) - 100;
        DataControl.SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
    }



    public void plusHP()
    {
        int temp = int.Parse(HpText.text) + 1;
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_HP", temp.ToString());
    }


    public void plusATK()
    {
        int temp = int.Parse(ATKText.text) + 1;
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_ATK", temp.ToString());
    }


    public void plusBC()
    {
        int temp = int.Parse(BcText.text) + 1;
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_BALLCOUNT", temp.ToString());
    }


    public void plusPP()
    {
        int temp = int.Parse(PPText.text) + 1;
        DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_PINHP", temp.ToString());
    }


    public void initUpgradeNum()
    {
        DataControl.SaveEncryptedDataToPrefs("UpgradableNum", 0.ToString());
        UpgradeBtnManager.RefreshUpgradeBtn();
    }


}
