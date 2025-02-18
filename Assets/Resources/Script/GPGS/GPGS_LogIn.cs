using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.UI;

public class GoogleManager : MonoBehaviour
{
    public TextMeshProUGUI logText;   //�ε� �ȳ��� �ؽ�Ʈ ������Ʈ

    private DataControl dataControl;    //��ġ �� ó�� ���� �� �����κ��� �����͸� �޾ƿ��� ���� DataControl Ŭ����
    public Button startBtn; //�α��� �� ������ �ε� �Ϸ� �� Ȱ��ȭ �� ���� ���� ��ư

    public GameObject ErrorPopup;

    void Awake()
    {
        dataControl = GetComponent<DataControl>();
        startBtn.interactable = false;  //������ �ε� �� �� ��ȯ ������ ���� ��ư ��Ȱ��ȭ
        logText.text = "";  //�ε� �ؽ�Ʈ �ʱ�ȭ
        ErrorPopup.SetActive(false);

        PlayGamesPlatform.DebugLogEnabled = true;   //gpgs ���� �����â Ű��
        PlayGamesPlatform.Activate();   //gpgs �۵� ����
       

        //������ ����ϴ� ������ �ִٸ�
        if (PlayerPrefs.HasKey("SavedAccountKey")) 
        {
            AutoSignIn();   //�ڵ� �α���
        }
        else
        {
            //ó�� �α��� �õ�����

            ManualSignIn(); //���� ���� ȭ�� �˾�
        }

    }

    public void AutoSignIn()    //���۰��� �ڵ� �α��� �Լ�
    {
        logText.text = "Starting login..."; //�ε� �ؽ�Ʈ�� �α��� ���� �˸�
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication); //gpgs �̱��� Ŭ������ �α��� �Լ� ȣ��(������ �ݹ� �Լ� ProcessAuthentication ���)
    }


    public void ManualSignIn()  //���� ���� ���� �α��� �Լ�
    {
        
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication); //�α��� ���� ���� ȭ���� ǥ��, ���� ���� �� �ݹ� �Լ� ProcessAuthentication ȣ��
    }




    internal void ProcessAuthentication(SignInStatus status)
    {
        //�α��� �������� ȣ�� �� �ݹ��Լ� ProcessAuthentication, ���� SignInStatus status�� �α��� ���� ���� ��ȯ��
        if (status == SignInStatus.Success)
        {
            //�α��ο� �����ߴٸ� �۵�

            logText.text = "Login success"; //�ε� �ؽ�Ʈ�� �α��� ���� �˸�

            string id = PlayGamesPlatform.Instance.GetUserId();             //���� id�� �޾ƿ���
            PlayerPrefs.SetString("SavedAccountKey", id);                   //�ڵ� �α��� ���� Ȯ���� ���� id���� player prefs�� ����
            PlayerPrefs.Save();


            logText.text = "Checking user data..."; //�ε� �ؽ�Ʈ�� ��ġ �� ù �������� Ȯ�� �� �˸�

            if (!PlayerPrefs.HasKey("Played"))
            {
                //��ġ �� ù �����̶��
                
                logText.text = "Checking server data..."; //���� ������ Ȯ���� �˸�

                dataControl.LoadData(); //�������� ������ Ȯ��, ������ �����Ͱ� �ִٸ� �ҷ�����

                logText.text = "Load complete!"; //�ε� �ؽ�Ʈ�� ������ �ε� �Ϸ� �˸�


                //��ġ �� ù ������ �ƴ��� �˸��� ���� playerPrefs�� ǥ��
                PlayerPrefs.SetString("Played", "true");
                PlayerPrefs.Save();

                startBtn.interactable = true;   //�� ��ȯ�ص� �������Ƿ� ��ư Ȱ��ȭ
            }
            else
            {
                startBtn.interactable = true;   //�� ��ȯ�ص� �������Ƿ� ��ư Ȱ��ȭ
            }

            logText.text = "";  //�ε� �ؽ�Ʈ�� ��� �ʿ� �����Ƿ� ����ó��

        }
        else
        {
            //�α��ο� �����ߴٸ�
            logText.text = "Sign in Failed! " + status.ToString();   //�ε� �ؽ�Ʈ�� "Sign in Failed!" ���            
            
            //���� �˾� ����
            ErrorPopup.SetActive(true);
            
        }
    }
}
