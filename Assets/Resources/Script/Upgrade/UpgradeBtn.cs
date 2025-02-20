using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    public Button btn;    // ���� ��ư
    public UpgradeBtnManager manager;    // ��ư �Ŵ��� // ��ȣ�� �ֱ� ���ؼ�

    [SerializeField] private GameObject upgradePopUp;   //��ư ��ġ �� ������ �˾�
    private UpgradePopupManager popupManager;           //������ ����� ���޵� ������ ������ ���� Ŭ����

    [SerializeField] private string upgradeName;  //�ش� ��ư�� ���� �̷���� ���׷��̵��� �̸�  // �˾��� ����
    [SerializeField] private int upgradeStat;     //�ش� ��ư�� ���� �̷���� ���׷��̵��� ��ġ
    [SerializeField] private int upgradeCost;    //�ش� ��ư�� ���� �̷���� ���׷��̵��� ���

    [SerializeField] private bool isUpgradable;  //�ش� ���׷��̵尡 ���� �������� ��Ÿ���� ��, ������ ���� ������Ƽ�� ����
    [SerializeField] private int upgradeNum;    //���׷��̵尣�� ������ ���� ������, �ܰ��� �ر� �� ���ű�� ����� ���� ����, ������ ���� ������Ƽ�� ����

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
        //�˾��� ������ ������ ���� UpgradePopupManager�� ȣ��
        popupManager = upgradePopUp.GetComponent<UpgradePopupManager>();

        // ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
        btn.onClick.AddListener(showButtons);
    }



    //�˾� Ȱ��ȭ
    public void showButtons()
    {
        //�˾��� ������ ����
        popupManager.UpgradeName = upgradeName;
        popupManager.UpgradeStat = upgradeStat;
        popupManager.UpgradeCost = upgradeCost;

        //�˾� ����
        popupManager.SetUpgradePopup();

        //�˾� Ȱ��ȭ
        upgradePopUp.SetActive(true);

        // ��ư Ʈ��ŷ.
        //manager.trackingBtn(this);
    }


    //�˾� �ݱ�
    public void hideButtons()
    {
        // �� ��ư Ȱ��ȭ
        upgradePopUp.SetActive(false);

    }



}
