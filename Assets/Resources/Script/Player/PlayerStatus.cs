using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    //플레이어 스탯 목록, 보안 및 오류사항 감지를 위해 프포퍼티로 구성, inspector 창에서 확인 및 수정이 가능하도록 [SerializeField] 사용

    [SerializeField] private int playerHP;          //플레이어 케릭터의 체력
    [SerializeField] private int playerATK;         //플레이어 케릭터의 공격력
    [SerializeField] private int playerPinHP;       //핀의 체력
    [SerializeField] private int playerBallCnt;     //사용자가 화면 터치 시 떨어질 공의 수



    //플레이어 스탯 접근을 위한 프로퍼티
    public int PlayerHP {  get { return playerHP; } set { playerHP = value; } }
    public int PlayerATK { get { return playerATK; } set { playerATK = value; } }
    public int PlayerPinHP { get {return playerPinHP; } set {playerPinHP = value; } }
    public int PlayerBallCnt { get { return playerBallCnt; } set { playerBallCnt = value; } }




    //가장 먼저 구동될 수 있도록 Awake 사용
    void Awake()
    {
        //디버그용 메소드, playerprefs에 플레이어 스텟값이 존재하지 않는다면 이하의 메소드들 실행(1회 실행하면 다시 주석 처리)
        //이 기능은 DataControl의 SetInitialData();로 구현되어 있으니 main과 merge 후 해당 코드로 변경바람
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_HP", 100.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_ATK", 5.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_PINHP", 3.ToString());
        //DataControl.SaveEncryptedDataToPrefs("PlayerCharacter_BALLCOUNT", 3.ToString());



        //오브젝트 로드 완료 시 PlayerPrefs로부터 스탯 불러오기
        InitPlayerStatus();
    }


    //플레이어 스탯을 PLayerPrefs로부터 받아오는 메소드
    private void InitPlayerStatus()
    {
        //체력 받아오기
        PlayerHP = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));

        //공격력 받아오기
        PlayerATK = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_ATK"));

        //핀 체력 받아오기
        PlayerPinHP = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_PINHP"));

        //공 수 받아오기
        PlayerBallCnt = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_BALLCOUNT"));

    }


}
