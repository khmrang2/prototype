using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.UI;

public class GoogleManager : MonoBehaviour
{
    public TextMeshProUGUI logText;   //로딩 안내용 텍스트 오브젝트

    private DataControl dataControl;    //설치 후 처음 실행 시 서버로부터 데이터를 받아오기 위한 DataControl 클래스
    public Button startBtn; //로그인 및 데이터 로드 완료 후 활성화 될 게임 시작 버튼

    public GameObject ErrorPopup;

    void Awake()
    {
        dataControl = GetComponent<DataControl>();
        startBtn.interactable = false;  //데이터 로딩 중 씬 전환 방지를 위한 버튼 비활성화
        logText.text = "";  //로딩 텍스트 초기화
        ErrorPopup.SetActive(false);

        PlayGamesPlatform.DebugLogEnabled = true;   //gpgs 전용 디버그창 키기
        PlayGamesPlatform.Activate();   //gpgs 작동 시작
       

        //기존에 사용하던 계정이 있다면
        if (PlayerPrefs.HasKey("SavedAccountKey")) 
        {
            AutoSignIn();   //자동 로그인
        }
        else
        {
            //처음 로그인 시도러면

            ManualSignIn(); //계정 선택 화면 팝업
        }

    }

    public void AutoSignIn()    //구글계정 자동 로그인 함수
    {
        logText.text = "Starting login..."; //로딩 텍스트에 로그인 시작 알림
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

            logText.text = " Login success"; //로딩 텍스트에 로그인 성공 알림

            string id = PlayGamesPlatform.Instance.GetUserId();             //유저 id값 받아오기
            PlayerPrefs.SetString("SavedAccountKey", id);                   //자동 로그인 여부 확인을 위해 id값을 player prefs에 저장
            PlayerPrefs.Save();

            
            logText.text = " Checking server data..."; //서버로부터 데이터를 받아오기 위해 서버 데이터 확인중 알림
            
            
            //로그인 및 계정 인증이 완료된 후 작동하는 로드 함수 호출 코루틴
            StartCoroutine(EnsureLoginAndLoadData());

            logText.text = "";  //로딩 텍스트를 띄울 필요 없으므로 공백처리

        }
        else
        {
            //로그인에 실패했다면
            logText.text = "Sign in Failed! " + status.ToString();   //로딩 텍스트에 "Sign in Failed!" 출력            
            
            //오류 팝업 띄우기
            ErrorPopup.SetActive(true);
            
        }
    }




    private IEnumerator EnsureLoginAndLoadData()
    {
        while (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            logText.text = "not yet Authenticated, waiting...";
            yield return new WaitForSeconds(0.5f);
        }

        logText.text = "Authenticated! starting data load.";
        dataControl.LoadDataWithCallback((success) =>
        {
            if (!success)
            {
                ErrorPopup.SetActive(true);
            }
            else
            {
                logText.text = "Load complete!";
                startBtn.interactable = true;
            }
        });
    }
}
