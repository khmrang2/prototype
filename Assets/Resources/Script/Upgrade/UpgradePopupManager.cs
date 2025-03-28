using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePopupManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI popupName;     //�˾��� ���׷��̵� �� �κ� �ؽ�Ʈ     
    [SerializeField] private TextMeshProUGUI popupCost;     //�˾��� ��� �κ� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI popupCostGear;

    [SerializeField] private GameObject FailPopup;      //���� ���� �� �� �˾�

    [SerializeField] private int uogradableNum;     //���� ������ ���׷��̵��� ��ȣ

    [SerializeField] private UpgradeBtnManager UpgradeBtnManager;   //��ư���� ������ ���� �޴��� ��ũ��Ʈ
  
    [SerializeField] private DataControl datactr;       //���׷��̵� ���� ���� ������ ���� ���� ���� ��ũ��Ʈ

    [SerializeField] private SaveAndLoadError SaveandLoaderror;

    private string upgradeName;     //�˾��� ǥ�õ� ���׷��̵� �̸�, ������ ���� ������Ƽ�� ����
    private int upgradeStatName;    //���׷��̵� ���� �� ���� �� ����, ������ ���� ������Ƽ�� ����
    private int upgradeStat;        //���׷��̵� ���� �� ���� �� ���� ��ġ, ������ ���� ������Ƽ�� ����
    private int upgradeCost;        //�˾��� ǥ�õ� ���׷��̵� ���, ������ ���� ������Ƽ�� ����
    private int upgradeCostG;

    public string UpgradeName {  get { return upgradeName; } set { upgradeName = value; } }
    public int UpgradeStatName { get { return upgradeStatName; } set { upgradeStatName = value; } }
    public int UpgradeStat { get { return upgradeStat; } set { upgradeStat = value; } }
    public int UpgradeCost { get { return upgradeCost; } set { upgradeCost = value; } }
    public int UpgradeCostGear { get { return upgradeCostG; } set { upgradeCostG = value; } }
	public AudioSource upgrade_button_click_sound;
	public AudioSource close_button_click_sound;


    //�ʱ�ȭ
    private void Start()
    {
        FailPopup.SetActive(false);
        ClosePopup();
    }



    //�˾� �ݱ�
    public void ClosePopup()
    {
		PlayOneShotSound(close_button_click_sound);
        this.gameObject.SetActive(false);
    }


    public void BuyUpgrade()
    {
        //���� ���� ��� �޾ƿ���
        int gold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
        int gear = int.Parse(DataControl.LoadEncryptedDataFromPrefs("UpgradeStone"));

        if (gold >= upgradeCost && gear >= upgradeCostG)
        {
            //���׷��̵� ���Ű� �����ϴٸ�

            //��� ���� ó��
            PlayerStatusInMain.Instance.PayGold(upgradeCost, (success) => { });
            PlayerStatusInMain.Instance.payUpgradeStone(upgradeCostG, (success) => { });

            //���޹��� ���׷��̵� ������ ���� ���� ���� ó��
            switch (upgradeStatName)
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
                    //���� �˾� ����
                    SaveandLoaderror.ShowErrorScreen();
                }
                else
                {
                    Debug.Log("upgrade save complete");

                    UpgradeBtnManager.RefreshUpgradeBtn();

                    Debug.Log("upgrade complete!");
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
        popupCost.text = "골드: "+upgradeCost.ToString() + "G";
        if(upgradeCostG > 0) popupCostGear.text = "강화기어: " + upgradeCostG.ToString() + "개";
        else popupCostGear.text = "";
        
    }

	private void PlayOneShotSound(AudioSource source)
    {
        if (source == null || source.clip == null) return;

        // 임시 오브젝트 생성
        GameObject tempAudioObj = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioObj.AddComponent<AudioSource>();
        tempAudio.clip = source.clip;
        tempAudio.outputAudioMixerGroup = source.outputAudioMixerGroup; // 믹서 연결 유지
        tempAudio.volume = source.volume;
        tempAudio.spatialBlend = 0f; // 2D
        tempAudio.Play();

        Destroy(tempAudioObj, tempAudio.clip.length);
    }
}
