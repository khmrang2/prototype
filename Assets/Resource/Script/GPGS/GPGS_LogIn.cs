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
        SignIn();   //���۰��� ����(�α���)�Լ� ȣ��
    }

    public void SignIn()    //���۰��� �α��� �Լ�
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication); //gpgs �̱��� Ŭ������ �α��� �Լ� ȣ��(������ �ݹ� �Լ� ProcessAuthentication ���)
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        //�α��� �������� ȣ�� �� �ݹ��Լ� ProcessAuthentication, ���� SignInStatus status�� �α��� ���� ���� ��ȯ��
        if (status == SignInStatus.Success)
        {
            //�α��ο� �����ߴٸ� �۵�

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();     //������ �޾ƿ���
            string id = PlayGamesPlatform.Instance.GetUserId();             //���� id�� �޾ƿ���
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();   //���� ������ �޾ƿ���

            logText.text = "Success \n" + name; //�α׿� "Success"�� ������ ���
        }
        else
        {
            //�α��ο� �����ߴٸ�
            logText.text = "Sign in Failed!";   //�α׿� "Sign in Failed!" ���


            // �� �� �α��� ���н� ó���� ���� ���
        }
    }
}
