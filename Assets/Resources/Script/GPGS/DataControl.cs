using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using System;
using System.ComponentModel;


public class DataSettings   //저장될 데이터 클래스
{
    public int gold = 0;
    public int hp = 0;
    public int atk = 0;
    public int pinHp = 0;
    public int ballCount = 0;
    public int upgradeNum = 0;
}



public class DataControl : MonoBehaviour
{


    public DataSettings settings = new DataSettings();
    //불러와지고 저장될 데이터를 담을 클래스 settings 선언

    private string fileName = "savefile.dat";
    //클라우드에 저장될 파일의 이름 설정


    // 암호화 키 (16바이트)
    private static readonly byte[] key = Encoding.UTF8.GetBytes("pX9rOL3mC3sEivmz");


    // 암호화 IV (16바이트)
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("KPlYJeefFqyw7WBe");



    //초기 데이터에서 입력될 각 데이터들의 키 이름
    private static string PlayerHPName = "PlayerCharacter_HP";                 //플레이어 케릭터의 체력의 키 이름
    private static string PlayerATKName = "PlayerCharacter_ATK";               //플레이어 케릭터의 공격력의 키 이름
    private static string PlayerBALLCOUNTName = "PlayerCharacter_BALLCOUNT";   //플레이어 케릭터의 공 수의 키 이름
    private static string PlayerPINHPName = "PlayerCharacter_PINHP";           //플레이어 케릭터의 핀 체력의 키 이름

    private static string GoldName = "Gold";                                   //재화 중 골드의 키 이름

    private static string UpgradableNumName = "UpgradableNum";                 //업그레이드 해금 정보의 키 이름
    //private string EquipDataName;                                     //장비 해금 정보의 키 이름



    //초기 데이터에서 입력될 각 데이터들의 값
    private static int PlayerHP = 100;                                         //플레이어 케릭터의 초기 체력값
    private static int PlayerATK = 5;                                          //플레이어 케릭터의 초기 공격력값
    private static int PlayerBALLCOUNT = 3;                                    //플레이어 케릭터의 초기 공 수의 값
    private static int PlayerPINHP = 3;                                        //플레이어 케릭터의 초기 핀 체력값

    private static int gold = 0;                                               //초기 골드값

    private static int UpgradableNum = 0;                                      //업그레이드 해금 정보의 초기값


    //세이브의 성공 여부를 나타내는 bool 변수
    private bool isSaveSuccess;



    #region gpgs 클라우드 데이터 저장


    public void SaveData() // 외부에서 세이브 호출용 메소드
    {
        //세이브 성공 여부 판단을 위한 bool 변수
        isSaveSuccess = false;

        OpenSaveGame();
    }







    private void OpenSaveGame() //세이브 메소드
    {
        //player prefs로부터 저장할 데이터 받아오기
        GetDataSettings();


        //gpgs의 싱글톤 인스턴스를 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            onsavedGameOpend);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 onsavedGameOpend 실행
    }


    private void onsavedGameOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        //데이터 요청 성공 여부와 불러와진 데이터를 통해 작동

        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs 싱글톤 호출

        if (status == SavedGameRequestStatus.Success)
        {
            //세이브 요청에 성공했다면

            Debug.Log("save success!");
            //로그 출력

            var update = new SavedGameMetadataUpdate.Builder().Build();
            //구글 플레이 서비스에 저장을 위한 메타데이터 

            //json
            var json = JsonUtility.ToJson(settings);
            //jsonUtility를 통해 저장하려는 데이터를 문자열로 변경

            byte[] data = Encoding.UTF8.GetBytes(json);
            //문자열로 변경된 데이터를 바이트로 변환 (UTF-8 사용)


            // 저장 함수 실행
            saveGameClient.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.Log("Save No.....");

            //세이브 성공 여부 확인용 변수의 값을 거짓으로 변경
            isSaveSuccess = false;
        }
    }

    // 저장 확인 
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //저장 메소드 작동 시 콜백을 통해 실행될 메소드

        if (status == SavedGameRequestStatus.Success)
        {
            // 저장완료부분
            Debug.Log("Save End");
            
            //세이브 성공 여부 확인용 변수의 값을 참으로 변경
            isSaveSuccess = true;

        }
        else
        {
            Debug.Log("Save nonononononono...");
            
            //세이브 성공 여부 확인용 변수의 값을 거짓으로 변경
            isSaveSuccess = false;
        }
    }






    #endregion








    #region gpgs 클라우드 데이터 로드


    public void LoadData()  //외부에서 로드 호출용 메소드
    {
        //서버 연결 확인용 bool 변수
        isSaveSuccess = false;

        OpenLoadGame();
    }





    private void OpenLoadGame()
    {
        //gpgs의 싱글톤 인스턴스를 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            LoadGameData);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 실행
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs 싱글톤 인스턴스 호출
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs에 보낸 요청이 성공했다면
            Debug.Log("Load success");

            //gpgs로부터 바이트 형식으로 저장된 데이터를 받아오고 콜백 함수 OnSavedGameDataRead 실행
            savedGameClient.ReadBinaryData(data, OnSavedGameDataRead);

            //성공했으니 확인용 변수를 true로
            isSaveSuccess = true;

        }
        else
        {
            Debug.Log("Load fail...");
            isSaveSuccess = false;
        }
    }


    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadData)
    {
        //받아온 바이트 형태의 데이터를 문자열로 변환
        string data = System.Text.Encoding.UTF8.GetString(loadData);

        if (data == "")
        {
            //받아온 데이터가 공백이라면
            Debug.Log("no saved data, saving initial data");

            //기존에 저장된 데이터가 없다는 뜻이고 이는 곧 플레이어가 완전 첫 실행이라는 뜻이므로 초기값 세팅
            SetInitialData();

            //해당 정보 저장
            SaveData();
        }
        else
        {
            //받아온 데이터가 공백이 아니라면
            Debug.Log("Loading data");

            //JSON
            settings = JsonUtility.FromJson<DataSettings>(data);
            //logText.text = "Load complete";

            //gpgs로부터 불러와진 데이터로 player prefs 최신화
            SetDataSettings();

        }
    }


    #endregion













    #region gpgs 클라우드 데이터 제거




    public void DeleteData()
    {
        DeleteGameData();
    }



    private void DeleteGameData()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;
        //gpgs의 싱글톤 인스턴스를 호출

        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            DeleteSaveGame);
        //클라우드 데이터 접근 요청 메소드, 지정한 파일명으로 저장, 케쉬가 최신이 아니라면 네트워크를 통해 데이터 가져옴, 충돌시를 대비해 최신 데이터를 처리, 콜백함수 실행
    }


    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        //gpgs의 싱글톤 인스턴스 호출
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (status == SavedGameRequestStatus.Success)
        {
            //gpgs에 보낸 요청이 받아들여졌다면

            //gpgs에 저장된 데이터 삭제
            saveGameClient.Delete(data);

            Debug.Log("Delete Complete");
            //logText.text = "Delete complete";
        }
        else
        {
            Debug.Log("Delete fail");
            //logText.text = "Delete failed";
        }
    }


    #endregion










    #region PlayerPrefs와 암호화 관련



    //gpgs로부터 받아온 data settings 데이터로 player prefs의 값들을 변경 
    private void SetDataSettings()
    {
        SaveEncryptedDataToPrefs(GoldName, settings.gold.ToString());
        SaveEncryptedDataToPrefs(PlayerHPName, settings.hp.ToString());
        SaveEncryptedDataToPrefs(PlayerATKName, settings.atk.ToString());
        SaveEncryptedDataToPrefs(PlayerPINHPName, settings.pinHp.ToString());
        SaveEncryptedDataToPrefs(PlayerBALLCOUNTName, settings.ballCount.ToString());
        SaveEncryptedDataToPrefs(UpgradableNumName,settings.upgradeNum.ToString());

    }



    //player prefs의 값들을 gpgs에 저장하기 위해 data settings로 가져오기
    private void GetDataSettings()
    {
        settings.gold = int.Parse(LoadEncryptedDataFromPrefs(GoldName));
        settings.hp = int.Parse(LoadEncryptedDataFromPrefs(PlayerHPName));
        settings.atk = int.Parse(LoadEncryptedDataFromPrefs(PlayerATKName));
        settings.ballCount = int.Parse(LoadEncryptedDataFromPrefs(PlayerBALLCOUNTName));
        settings.pinHp = int.Parse(LoadEncryptedDataFromPrefs(PlayerPINHPName));
        settings.upgradeNum = int.Parse(LoadEncryptedDataFromPrefs(UpgradableNumName));
    }




    //player prefs에 암호화시켜 데이터를 저장하는 메소드, player prefs에 keyName을 키로 삼아 문자열 data을 저장


    //이 함수 사용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static void SaveEncryptedDataToPrefs(string keyName, string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            //저장된 키값과 초기화 벡터값을 받아옴
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // 암호화 키와 초기화 벡터를 이용하여, 암호화를 진행할 encryptor 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encryptedData = null;

            // 일반 데이터를 암호화
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            // 암호화 데이터를 문자열로 변환하여 저장 
            string encryptedString = Convert.ToBase64String(encryptedData);
            PlayerPrefs.SetString(keyName, encryptedString);
            PlayerPrefs.Save();

            //Debug.Log(encryptedString);
        }
    }




    //player prefs에 암호화되어 저장된 데이터를 받아오는 메소드, 저장시 사용한 keyName값으로 데이터를 찾아 문자열 값으로 반환

    //이 함수 사용!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static string LoadEncryptedDataFromPrefs(string keyName)
    {
        //player prefs로부터 keyName값을 통해 암호화된 데이터를 encryptedString에 저장
        string encryptedString = PlayerPrefs.GetString(keyName);

        if (!string.IsNullOrEmpty(encryptedString))
        {
            //만일 받아온 값이 비어있지 않다면(= 값이 있다면)
            using (Aes aesAlg = Aes.Create())
            {

                //Debug.Log(encryptedString);

                //저장된 키값과 초기화 벡터값을 받아옴
                aesAlg.Key = key;
                aesAlg.IV = iv;

                //암호화 키와 초기화 벡터를 이용하여 복호화를 진행할 decryptor 생성
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 데이터 복호화
                byte[] encryptedData = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                // 복호화된 데이터를 이용하여 저장된 데이터 반환
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        else
        {
            //받아온 값이 비어있다면
            return null;
        }

    }



    #endregion







    #region 초기 데이터 세팅 관련

    
    //호출 시 플레이어 케릭터의 스탯, 업그레이드 해금 및 정바 해금 정보의 초기값 세팅
    public static void SetInitialData()
    {
        //플레이어 케릭터의 초기 스탯 세팅
        SaveEncryptedDataToPrefs(PlayerHPName, PlayerHP.ToString());
        SaveEncryptedDataToPrefs(PlayerATKName, PlayerATK.ToString());
        SaveEncryptedDataToPrefs(PlayerBALLCOUNTName, PlayerBALLCOUNT.ToString());
        SaveEncryptedDataToPrefs(PlayerPINHPName, PlayerPINHP.ToString());

        //초기 골드값 세팅
        SaveEncryptedDataToPrefs(GoldName, gold.ToString());

        //업그레이드 해금 정보의 초기값 세팅
        SaveEncryptedDataToPrefs(UpgradableNumName, UpgradableNum.ToString());
    }




    #endregion





    #region 서버 연결 확인 관련
    
    
    //세이브 및 로드 성공 확인용 메소드
    public bool CheckSaveSuccess()
    {
        if (isSaveSuccess)
        {
            //세이브가 성공적으로 이루어져 세이브 성공 확인 변수의 값이 참이라면

            return true;    //true 반환
        }
        else
        {
            //세이브가 성공적으로 이루어지지 못해 true 이외의 값을 갖게 되면

            return false;   //false 반환
        }
    }


    #endregion


}
