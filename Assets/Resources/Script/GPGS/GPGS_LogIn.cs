using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GoogleManager : MonoBehaviour
{
    public TextMeshProUGUI logText;

    void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;   //gpgs ���� �����â Ű��
        PlayGamesPlatform.Activate();   //gpgs �۵� ����
       

        //������ ����ϴ� ������ �ִٸ�
        if (PlayerPrefs.HasKey("SavedAccountKey")) 
        {
            AutoSignIn();
        }
        else
        {
            //ó�� �α��� �õ�����

            ManualSignIn();
        }

    }

    public void AutoSignIn()    //���۰��� �ڵ� �α��� �Լ�
    {
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

            
            string id = PlayGamesPlatform.Instance.GetUserId();             //���� id�� �޾ƿ���
            PlayerPrefs.SetString("SavedAccountKey", id);                   //�ڵ� �α��� ���� Ȯ���� ���� id���� player prefs�� ����
            PlayerPrefs.Save();


            logText.text = "Success"; //�α׿� "Success" ���
        }
        else
        {
            //�α��ο� �����ߴٸ�
            logText.text = "Sign in Failed! " + status.ToString();   //�α׿� "Sign in Failed!" ���


            //�ٸ� ���� ��� ������ ���� ���� �α��� ȣ��
            ManualSignIn();

            // �� �� �α��� ���н� ó���� ���� ���
        }
    }
}
