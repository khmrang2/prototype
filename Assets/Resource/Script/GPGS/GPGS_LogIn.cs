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
        SignIn();   //구글계정 연동(로그인)함수 호출
    }

    public void SignIn()    //구글계정 로그인 함수
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication); //gpgs 싱글톤 클래스의 로그인 함수 호출(변수로 콜백 함수 ProcessAuthentication 등록)
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        //로그인 과정에서 호출 될 콜백함수 ProcessAuthentication, 변수 SignInStatus status는 로그인 성공 여부 반환값
        if (status == SignInStatus.Success)
        {
            //로그인에 성공했다면 작동

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();     //유저명 받아오기
            string id = PlayGamesPlatform.Instance.GetUserId();             //유저 id값 받아오기
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();   //유저 아이콘 받아오기

            logText.text = "Success \n" + name; //로그에 "Success"와 유저명 출력
        }
        else
        {
            //로그인에 실패했다면
            logText.text = "Sign in Failed!";   //로그에 "Sign in Failed!" 출력


            // 그 외 로그인 실패시 처리할 사항 기술
        }
    }
}
