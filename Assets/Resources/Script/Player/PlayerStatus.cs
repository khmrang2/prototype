using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    //�÷��̾� ���� ���, ���� �� �������� ������ ���� ������Ƽ�� ����, inspector â���� Ȯ�� �� ������ �����ϵ��� [SerializeField] ���

    [SerializeField] private int playerHP;          //�÷��̾� �ɸ����� ü��
    [SerializeField] private int playerATK;         //�÷��̾� �ɸ����� ���ݷ�
    [SerializeField] private int playerPinHP;       //���� ü��
    [SerializeField] private int playerBallCnt;     //����ڰ� ȭ�� ��ġ �� ������ ���� ��



    //�÷��̾� ���� ������ ���� ������Ƽ
    public int PlayerHP {  get { return playerHP; } set { playerHP = value; } }
    public int PlayerATK { get { return playerATK; } set { playerATK = value; } }
    public int PlayerPinHP { get {return playerPinHP; } set {playerPinHP = value; } }
    public int PlayerBallCnt { get { return playerBallCnt; } set { playerBallCnt = value; } }




    //���� ���� ������ �� �ֵ��� Awake ���
    void Awake()
    {
        //����׿� �޼ҵ�, playerprefs�� �÷��̾� ���ݰ��� �������� �ʴ´ٸ� ������ �޼ҵ�� ����(1ȸ �����ϸ� �ٽ� �ּ� ó��)
        //�� ����� DataControl�� SetInitialData();�� �����Ǿ� ������ main�� merge �� �ش� �ڵ�� ����ٶ�
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_HP", 100.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_ATK", 5.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_PINHP", 3.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_BALLCOUNT", 3.ToString());



        //������Ʈ �ε� �Ϸ� �� PlayerPrefs�κ��� ���� �ҷ�����
        InitPlayerStatus();
    }


    //�÷��̾� ������ PLayerPrefs�κ��� �޾ƿ��� �޼ҵ�
    private void InitPlayerStatus()
    {
        //ü�� �޾ƿ���
        PlayerHP = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));

        //���ݷ� �޾ƿ���
        PlayerATK = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK"));

        //�� ü�� �޾ƿ���
        PlayerPinHP = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP"));

        //�� �� �޾ƿ���
        PlayerBallCnt = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT"));

    }


}
