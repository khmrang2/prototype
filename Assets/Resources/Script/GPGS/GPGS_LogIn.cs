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
        PlayGamesPlatform.DebugLogEnabled = true;   //gpgs 전용 디버그창 키기
        PlayGamesPlatform.Activate();   //gpgs 작동 시작
       

        //기존에 사용하던 계정이 있다면
        if (PlayerPrefs.HasKey("SavedAccountKey")) 
        {
            AutoSignIn();
        }
        else
        {
            //처음 로그인 시도러면

            ManualSignIn();
        }

    }

    public void AutoSignIn()    //구글계정 자동 로그인 함수
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication); //gpgs 싱글톤 클래스의 로그인 함수 호출(변수로 콜백 함수 ProcessAuthentication 등록)
    }


    public void ManualSignIn()  //구글 계정 수동 로그인 함수
    {
        
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication); //로그인 계정 선택 화면을 표시, 계정 선택 후 콜백 함수 ProcessAuthentication 호출
    }




    internal void ProcessAuthentication(SignInStatus status)
    {
        //로그인 과정에서 호출 될 콜백함수 ProcessAuthentication, 변수 SignInStatus status는 로그인 성공 여부 반환값
        if (status == SignInStatus.Success)
        {
            //로그인에 성공했다면 작동

            
            string id = PlayGamesPlatform.Instance.GetUserId();             //유저 id값 받아오기
            PlayerPrefs.SetString("SavedAccountKey", id);                   //자동 로그인 여부 확인을 위해 id값을 player prefs에 저장
            PlayerPrefs.Save();


            logText.text = "Success"; //로그에 "Success" 출력
        }
        else
        {
            //로그인에 실패했다면
            logText.text = "Sign in Failed! " + status.ToString();   //로그에 "Sign in Failed!" 출력


            //다른 게정 사용 유도를 위해 수동 로그인 호출
            ManualSignIn();

            // 그 외 로그인 실패시 처리할 사항 기술
        }
    }
}
