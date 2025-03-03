using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//���׷��̵� ��ư ���� ������ų ������ ������ �����ϱ� ���� ������(enum) ����
public enum UpgradeStatName
{
    HP = 0,
    ATK = 1,
    BallCount = 2,
    PinHP = 3
}


public class UpgradeBtn : MonoBehaviour
{
    public Button btn;    //�������� Ȱ�� & ��Ȱ���� ó���ϱ� ���� ��ư
    //public UpgradeBtnManager manager;    // ��ư �Ŵ��� // ��ȣ�� �ֱ� ���ؼ�

    [SerializeField] private GameObject upgradePopUp;   //��ư ��ġ �� ������ �˾�
    private UpgradePopupManager popupManager;           //������ ����� ���޵� ������ ������ ���� Ŭ����

    [SerializeField] private string upgradeName;  //�ش� ��ư�� ���� �̷���� ���׷��̵��� �̸�  // �˾��� ����
    [SerializeField] private int upgradeStat;     //�ش� ��ư�� ���� �̷���� ���׷��̵��� ��ġ
    [SerializeField] private int upgradeCost;    //�ش� ��ư�� ���� �̷���� ���׷��̵��� ���

    [SerializeField] private bool isUpgradable;  //�ش� ���׷��̵尡 ���� �������� ��Ÿ���� ��, ������ ���� ������Ƽ�� ����
    [SerializeField] private int upgradeNum;    //���׷��̵尣�� ������ ���� ������, �ܰ��� �ر� �� ���ű�� ����� ���� ����, ������ ���� ������Ƽ�� ����

    public bool IsUpgradable { get { return isUpgradable; } set { isUpgradable = value; } }

    public int UpgradeNum { get { return upgradeNum; } }


    public UpgradeStatName upgradeStatName;     //���׷��̵� ��ư ���� ������ų ������ ������ �����ϱ� ���� ������





    #region �ʱ�ȭ
    void Start()
    {
        //�˾��� ������ ������ ���� UpgradePopupManager�� ȣ��
        popupManager = upgradePopUp.GetComponent<UpgradePopupManager>();

        // ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
        btn.onClick.AddListener(showButtons);

        //���׷��̵� ���� ���ο� ���� �����θ� Ȱ�� or ��Ȱ��ȭ
        CheckUpgradable();

    }

    #endregion



    #region �˾� ���� �޼ҵ�

    //�˾� Ȱ��ȭ
    public void showButtons()
    {

        //�˾��� ������ ����
        popupManager.UpgradeName = upgradeName;
        popupManager.UpgradeStat = upgradeStat;
        popupManager.UpgradeCost = upgradeCost;
        popupManager.UpgradeStatName = (int)upgradeStatName;


        //�˾� ����
        popupManager.SetUpgradePopup();

        //�˾� Ȱ��ȭ
        upgradePopUp.SetActive(true);

    }


    //�˾� �ݱ�
    public void hideButtons()
    {
        // �� ��ư Ȱ��ȭ
        upgradePopUp.SetActive(false);

    }

    #endregion



    #region ���׷��̵� ��ư Ȱ��ȭ ���� �޼ҵ�


    //�ش� ���׷��̵尡 ���� �������� Ȯ���ϴ� �޼ҵ�
    public void CheckUpgradable()
    {
        //���� ���� ������ ���׷��̵��� ��ȣ�� �޾ƿ�
        int upNum = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradableNum"));

        //�� ���׷��̵��� ��ȣ�� ���� ���� ������ ���׷��̵��� ��ȣ�� ������ Ȯ��
        if (upNum == UpgradeNum) 
        {
            //���ٸ� �ش� ��ư�� ��ȣ�ۿ� ���θ� true�� ����
            IsUpgradable = true;

            //�̹��� ����
            ChangeButtonImage();

            //��ȣ�ۿ� ���� ó��
            btn.interactable = true;

        }
        else
        {
            //�ٸ��ٸ� �ش� ��ư�� ��ȣ�ۿ� ���θ� false�� ����
            IsUpgradable = false;

            //�̹��� ����
            ChangeButtonImage();

            //��ȣ�ۿ� �Ұ��� ó��
            btn.interactable = false;
        }
    }



    //���׷��̵� ���� ���� ���ο� ���� ��ư �̹��� ��ȭ �޼ҵ�
    private void ChangeButtonImage()
    {
        if (IsUpgradable)
        {
            //���׷��̵尡 �����ϴٸ� 
            Debug.Log("�̹��� �������� ����");
        }
        else
        {
            //���׷��̵尡 �Ұ����ϴٸ�
            Debug.Log("�̹��� �Ұ������� ����");
        }
    }


    #endregion


}
