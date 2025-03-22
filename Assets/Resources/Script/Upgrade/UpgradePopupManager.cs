using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePopupManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupName;     //�˾��� ���׷��̵� �� �κ� �ؽ�Ʈ     
    [SerializeField] private TextMeshProUGUI popupCost;     //�˾��� ��� �κ� �ؽ�Ʈ

    [SerializeField] private GameObject FailPopup;      //���� ���� �� �� �˾�

    [SerializeField] private int uogradableNum;     //���� ������ ���׷��̵��� ��ȣ

    [SerializeField] private UpgradeBtnManager UpgradeBtnManager;   //��ư���� ������ ���� �޴��� ��ũ��Ʈ
  
    [SerializeField] private DataControl datactr;       //���׷��̵� ���� ���� ������ ���� ���� ���� ��ũ��Ʈ

    [SerializeField] private SaveAndLoadError SaveandLoaderror;

    private string upgradeName;     //�˾��� ǥ�õ� ���׷��̵� �̸�, ������ ���� ������Ƽ�� ����
    private int upgradeStatName;    //���׷��̵� ���� �� ���� �� ����, ������ ���� ������Ƽ�� ����
    private int upgradeStat;        //���׷��̵� ���� �� ���� �� ���� ��ġ, ������ ���� ������Ƽ�� ����
    private int upgradeCost;        //�˾��� ǥ�õ� ���׷��̵� ���, ������ ���� ������Ƽ�� ����

    public string UpgradeName {  get { return upgradeName; } set { upgradeName = value; } }
    public int UpgradeStatName { get { return upgradeStatName; } set { upgradeStatName = value; } }
    public int UpgradeStat { get { return upgradeStat; } set { upgradeStat = value; } }
    public int UpgradeCost { get { return upgradeCost; } set { upgradeCost = value; } }



    //�ʱ�ȭ
    private void Start()
    {
        FailPopup.SetActive(false);
        ClosePopup();
    }



    //�˾� �ݱ�
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }


    public void BuyUpgrade()
    {
        //���� ���� ��� �޾ƿ���
        int gold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));


        if (gold >= upgradeCost)
        {
            //���׷��̵� ���Ű� �����ϴٸ�

            //��� ���� ó��
            gold -= upgradeCost;
            DataControl.SaveEncryptedDataToPrefs("Gold", gold.ToString());

            //���޹��� ���׷��̵� ������ ���� ���� ���� ó��
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

            //���׷��̵� ��ȣ ���� ó��
            DataControl.SaveEncryptedDataToPrefs("UpgradableNum",
                (int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradableNum")) + 1).ToString());


            //���׷��̵� ���ŷ� ���� ��ȭ�� ������ ����
            datactr.SaveDataWithCallback((success) =>
            {
                if (!success)
                {
                    // ������ ���� �������� ���ߴٸ� ���� ���
                    //������Ų ������ ������

                    //���� �˾� ����
                    SaveandLoaderror.ShowErrorScreen();

                }
                else
                {
                    Debug.Log("upgrade save complete");
                    
                    UpgradeBtnManager.RefreshUpgradeBtn();

                }

            });

       

 


            ClosePopup();
        }
        else 
        {
            //���׷��̵� ���Ű� �Ұ����ϴٸ�


            //���� ���� �˾� ����
            FailPopup.SetActive(true);

            Debug.Log("���� ����!");
            ClosePopup();
        }
    }

    //�˾� ����
    public void SetUpgradePopup()
    {
        popupName.text = upgradeName;
        popupCost.text = "비용: "+upgradeCost.ToString() + "G";
    }


}
