using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePopupManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupName;     //�˾��� ���׷��̵� �� �κ� �ؽ�Ʈ     
    [SerializeField] private TextMeshProUGUI popupCost;     //�˾��� ��� �κ� �ؽ�Ʈ

    [SerializeField] private GameObject FailPopup;      //���� ���� �� �� �˾�

    private string upgradeName;     //�˾��� ǥ�õ� ���׷��̵� �̸�, ������ ���� ������Ƽ�� ����
    private int upgradeStat;        //���׷��̵� ���� �� ���� �� ����, ������ ���� ������Ƽ�� ����
    private int upgradeCost;        //�˾��� ǥ�õ� ���׷��̵� ���, ������ ���� ������Ƽ�� ����

    public string UpgradeName {  get { return upgradeName; } set { upgradeName = value; } }
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
            gold -= upgradeCost;
            //���׷��̵� ���Ű� �����ϴٸ�
            DataControl.SaveEncryptedDataToPrefs("Gold", gold.ToString());

            Debug.Log("���� ����!");


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
        popupCost.text = "cost: "+upgradeCost.ToString() + "G";
    }


}
